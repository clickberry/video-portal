// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Attributes.WebApi;
using Asp.Infrastructure.Extensions;
using Portal.Api.Models;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.Domain.PortalContext;
using Portal.Domain.ProfileContext;
using Portal.DTO.User;
using Portal.Mappers;

namespace Portal.Api.Controllers.Accounts
{
    /// <summary>
    ///     Manages user account.
    /// </summary>
    [AuthorizeHttp]
    [ValidationHttp]
    [Route("accounts/profile")]
    public sealed class ProfileController : ApiControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserAvatarProvider _avatarProvider;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly IProductIdExtractor _productIdExtractor;
        private readonly IUserService _userService;

        public ProfileController(
            IUserService userService,
            IAuthenticationService authenticationService,
            IEmailNotificationService emailNotificationService,
            IPasswordService passwordService,
            IProductIdExtractor productIdExtractor,
            IUserAvatarProvider avatarProvider,
            IMapper mapper)
        {
            _userService = userService;
            _authenticationService = authenticationService;
            _emailNotificationService = emailNotificationService;
            _passwordService = passwordService;
            _productIdExtractor = productIdExtractor;
            _avatarProvider = avatarProvider;
            _mapper = mapper;
        }

        //
        // POST: /api/accounts/profile

        /// <summary>
        ///     Register user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [CheckAccessHttp]
        [AllowAnonymous]
        [StatUserRegistrationWebApi]
        [StatUserLoginWebApi]
        public async Task<HttpResponseMessage> Post(RegisterUserModel model)
        {
            // Add profile
            var user = new DomainUser
            {
                ApplicationName = AppName,
                Name = model.UserName,
                Email = model.Email,
                Roles = new List<string> { DomainRoles.User },
                UserAgent = _productIdExtractor.Get(UserAgent)
            };

            user = await _userService.AddAsync(user);

            // Set user password
            await _passwordService.ChangePasswordAsync(user.Id, model.Password);

            try
            {
                await _emailNotificationService.SendRegistrationEmailAsync(user);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to send registration e-mail to {0}: {1}", model.Email, e);
            }

            Profile profile = _mapper.Map<DomainUser, Profile>(user);
            profile.AvatarUrl = _avatarProvider.GetAvatar(user);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, profile);
            response.SetLastModifiedDate(user.Modified);

            await _authenticationService.SetUserAsync(user, new TokenData { IdentityProvider = ProviderType.Email }, true);

            return response;
        }

        //
        // GET: /api/accounts/profile

        /// <summary>
        ///     Gets profile information.
        /// </summary>
        /// <returns>Account information.</returns>
        public async Task<HttpResponseMessage> Get()
        {
            DomainUser user = await _userService.GetAsync(UserId);

            Profile profile = _mapper.Map<DomainUser, Profile>(user);
            profile.AvatarUrl = _avatarProvider.GetAvatar(user);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, profile);
            response.SetLastModifiedDate(user.Modified);

            return response;
        }

        //
        // PUT: /api/accounts/profile

        /// <summary>
        ///     Edits profile information.
        /// </summary>
        /// <returns>Account information.</returns>
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Put(UserProfileModel model)
        {
            DomainUser user = await _userService.UpdateAsync(UserId, model);

            // Update user name
            _authenticationService.UpdateIdentityClaims(user);

            // Update email if required
            if (!string.IsNullOrEmpty(model.Email) && !string.Equals(user.Email, model.Email, StringComparison.OrdinalIgnoreCase))
            {
                // setting user email
                await _userService.ChangeEmailAsync(user.Id, model.Email);

                // update user claims
                user.Email = model.Email;
                _authenticationService.UpdateIdentityClaims(user);
            }

            Profile profile = _mapper.Map<DomainUser, Profile>(user);
            profile.AvatarUrl = _avatarProvider.GetAvatar(user);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, profile);
            response.SetLastModifiedDate(user.Modified);

            return response;
        }

        //
        // DELETE api/accounts/profile

        /// <summary>
        ///     Deletes current user.
        /// </summary>
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Delete()
        {
            _authenticationService.Clear();

            await _userService.DeleteAsync(UserId);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}