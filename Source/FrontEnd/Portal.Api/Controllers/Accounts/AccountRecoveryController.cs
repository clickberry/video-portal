// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Configuration;
using Portal.Api.Models;
using Portal.BLL.Services;
using Portal.Domain.AccountContext;
using Portal.Domain.ProfileContext;
using Portal.Resources.Api;

namespace Portal.Api.Controllers.Accounts
{
    /// <summary>
    ///     Manages user account recovery.
    /// </summary>
    [ValidationHttp]
    [RoutePrefix("accounts/recovery")]
    public sealed class AccountRecoveryController : ApiControllerBase
    {
        private readonly IPasswordRecoveryService _passwordRecoveryService;
        private readonly IPortalFrontendSettings _settings;
        private readonly IUserService _userService;

        public AccountRecoveryController(IUserService userService,
            IPasswordRecoveryService passwordRecoveryService,
            IPortalFrontendSettings settings)
        {
            _userService = userService;
            _passwordRecoveryService = passwordRecoveryService;
            _settings = settings;
        }

        //
        // POST: /api/accounts/recovery

        /// <summary>
        ///     Sends email with password recovery link.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route]
        public async Task<HttpResponseMessage> Post(PasswordRecoveryModel model)
        {
            DomainUser user = await _userService.FindByEmailAsync(model.Email);

            await _passwordRecoveryService.SendNewRecoveryMail(user, _settings.AccountSetPasswordPath);

            return Request.CreateResponse(HttpStatusCode.Created);
        }


        //
        // POST: /api/accounts/recovery/validate

        /// <summary>
        ///     Checks password recovery link.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("validate")]
        public async Task<HttpResponseMessage> Post(RecoveryLinkModel model)
        {
            RecoveryLink link = _passwordRecoveryService.GetLink(model.E, model.I);
            if (link.ExpirationDate == DateTime.MinValue)
            {
                // If wrong '?i=' param don't show LinkExpired page
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ResponseMessages.ResourceNotFound);
            }

            if (link.ExpirationDate <= DateTime.UtcNow)
            {
                // link expired
                return Request.CreateErrorResponse(HttpStatusCode.Gone, ResponseMessages.ResourceGone);
            }

            if (!await _passwordRecoveryService.CheckIfLinkIsValid(link))
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ResponseMessages.ResourceNotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }


        //
        // PUT: /api/accounts/recovery

        /// <summary>
        ///     Updates account password.
        /// </summary>
        /// <returns>Account information.</returns>
        [Route]
        public async Task<HttpResponseMessage> Put(ResetPasswordModel model)
        {
            // Check recovery link
            RecoveryLink link = _passwordRecoveryService.GetLink(model.E, model.I);
            if (link.ExpirationDate == DateTime.MinValue)
            {
                // If wrong '?i=' param don't show LinkExpired page
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ResponseMessages.ResourceNotFound);
            }
            if (link.ExpirationDate <= DateTime.UtcNow)
            {
                // link expired
                return Request.CreateErrorResponse(HttpStatusCode.Gone, ResponseMessages.ResourceGone);
            }

            // Change password
            await _passwordRecoveryService.ChangePassword(link, model.Password);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}