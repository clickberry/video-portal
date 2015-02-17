// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Attributes.WebApi;
using Portal.Api.Models;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.Domain.PortalContext;
using Portal.Domain.ProfileContext;
using Portal.Exceptions.CRUD;
using Portal.Resources.Api;

namespace Portal.Api.Controllers.Accounts
{
    /// <summary>
    ///     Manages sessions.
    /// </summary>
    [Authorize]
    [ValidationHttp]
    [EnableCors(origins: "https://az411958.vo.msecnd.net", headers: "*", methods: "*", SupportsCredentials = true)]
    [Route("accounts/ipsession")]
    public sealed class IpSessionController : ApiControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly ISocialNetworkNotificationFactory _notificationFactory;
        private readonly IProductIdExtractor _productIdExtractor;
        private readonly ITokenDataExtractorFactory _tokenDataExtractorFactory;
        private readonly IUserService _userService;

        public IpSessionController(
            IUserService userService,
            IAuthenticationService authenticationService,
            ITokenDataExtractorFactory tokenDataExtractorFactory,
            IProductIdExtractor productIdExtractor,
            IEmailNotificationService emailNotificationService,
            ISocialNetworkNotificationFactory notificationFactory)
        {
            _userService = userService;
            _authenticationService = authenticationService;
            _tokenDataExtractorFactory = tokenDataExtractorFactory;
            _productIdExtractor = productIdExtractor;
            _emailNotificationService = emailNotificationService;
            _notificationFactory = notificationFactory;
        }

        //
        // POST api/accounts/ipsession

        /// <summary>
        ///     Authenticates user by Windows Azure ACS token.
        /// </summary>
        /// <param name="model">Windows Azure ACS security token.</param>
        /// <returns>Status code.</returns>
        [StatUserRegistrationWebApi]
        [StatUserLoginWebApi]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Post(SessionIpModel model)
        {
            ITokenDataExtractor tokenDataExtractor = _tokenDataExtractorFactory.CreateTokenDataExtractor(model.Type);

            TokenData tokenData;

            try
            {
                tokenData = tokenDataExtractor.Get(model);
            }
            catch (BadRequestException)
            {
                ModelState.AddModelError("Token", ResponseMessages.AccountsInvalidToken);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            // Return result
            DomainUser user = null;

            try
            {
                user = await _userService.FindByIdentityAsync(tokenData.IdentityProvider, tokenData.UserIdentifier);
            }
            catch (NotFoundException)
            {
            }

            // Try to find user by email
            if (user == null && !string.IsNullOrEmpty(tokenData.Email))
            {
                try
                {
                    user = await _userService.FindByEmailAsync(tokenData.Email);
                }
                catch (NotFoundException)
                {
                }

                // Add ip memebership to user
                if (user != null)
                {
                    await _userService.AddMembersipAsync(user.Id, tokenData);
                }
            }

            var httpStatusCode = HttpStatusCode.OK;

            // Check whether registration is required
            if (user == null)
            {
                var profileData = new DomainUser
                {
                    ApplicationName = AppName,
                    Name = tokenData.Name,
                    Email = tokenData.Email,
                    UserAgent = _productIdExtractor.Get(UserAgent),
                    Roles = new List<string> { DomainRoles.User },
                    Memberships = new List<UserMembership>
                    {
                        new UserMembership
                        {
                            IdentityProvider = tokenData.IdentityProvider,
                            UserIdentifier = tokenData.UserIdentifier,
                        }
                    }
                };

                user = await _userService.AddAsync(profileData);

                // Send registration e-mail
                try
                {
                    await _emailNotificationService.SendRegistrationEmailAsync(user);
                }
                catch (Exception e)
                {
                    Trace.TraceError("Failed to send registration email to address {0} for user {1}: {2}", user.Email, user.Id, e);
                }

                // Send notification into social network
                ISocialNetworkNotifier notifier = _notificationFactory.GetNotifier(tokenData.IdentityProvider);
                if (notifier != null)
                {
                    try
                    {
                        await notifier.SendWelcomeMessageAsync(user, tokenData);
                    }
                    catch (BadGatewayException)
                    {
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError("Failed to send welcome message to user {0}: {1}", user.Id, e);
                    }
                }

                httpStatusCode = HttpStatusCode.Created;
            }

            var token = await _authenticationService.SetUserAsync(user, tokenData, true);
            return Request.CreateResponse(httpStatusCode, new AuthenticationTokenModel { Token = token });
        }

        //
        // DELETE api/accounts/ipsession

        /// <summary>
        ///     Closes user session.
        /// </summary>
        [AuthorizeHttp]
        public HttpResponseMessage Delete()
        {
            _authenticationService.Clear();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}