// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Attributes.WebApi;
using Portal.Api.Models;
using Portal.BLL.Services;
using Portal.Domain.ProfileContext;
using Portal.Exceptions.CRUD;
using Portal.Resources.Api;

namespace Portal.Api.Controllers.Accounts
{
    /// <summary>
    ///     Manages session.
    /// </summary>
    [AuthorizeHttp]
    [ValidationHttp]
    [Route("accounts/session")]
    public sealed class SessionController : ApiControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public SessionController(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        //
        // POST api/accounts/session

        /// <summary>
        ///     Authenticates user by login / password pair.
        /// </summary>
        /// <param name="model">User name and password.</param>
        /// <returns>Status code.</returns>
        [AllowAnonymous]
        [StatUserLoginWebApi]
        public async Task<HttpResponseMessage> Post(SessionLoginModel model)
        {
            DomainUser user;

            try
            {
                user = await _userService.CheckCredentialsAsync(model.Email, model.Password);
            }
            catch (BadRequestException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.AccountsInvalidUserNameOrPassword);
            }
            catch (NotFoundException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.AccountsInvalidUserNameOrPassword);
            }

            var token = await _authenticationService.SetUserAsync(user, new TokenData { IdentityProvider = ProviderType.Email }, true);
            return Request.CreateResponse(HttpStatusCode.OK, new AuthenticationTokenModel { Token = token });
        }

        //
        // DELETE api/accounts/session

        /// <summary>
        ///     Closes user session.
        /// </summary>
        public HttpResponseMessage Delete()
        {
            _authenticationService.Clear();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}