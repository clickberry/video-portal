// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Portal.Api.Models;
using Portal.BLL.Services;
using Portal.Domain.PortalContext;
using Portal.Domain.ProfileContext;
using Portal.DTO.User;
using Portal.Exceptions.CRUD;
using Portal.Resources.Api;

namespace Portal.Api.Controllers.Accounts
{
    /// <summary>
    ///     Handles account emails & password.
    /// </summary>
    [AuthorizeHttp(Roles = DomainRoles.User)]
    [ValidationHttp]
    [Route("accounts/emails")]
    public sealed class EmailsController : ApiControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IPasswordService _passwordService;
        private readonly IUserService _userService;

        public EmailsController(IUserService userService,
            IPasswordService passwordService,
            IAuthenticationService authenticationService)
        {
            _userService = userService;
            _passwordService = passwordService;
            _authenticationService = authenticationService;
        }

        // POST api/accounts/emails
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Post(ProfileEmailModel model)
        {
            DomainUser user = await _userService.GetAsync(UserId);

            // Checks whether user already has same e-mail
            if (string.Equals(user.Email, model.Email, StringComparison.OrdinalIgnoreCase))
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            // Change email
            await _userService.ChangeEmailAsync(user.Id, model.Email);

            // Update claims
            user.Email = model.Email;
            _authenticationService.UpdateIdentityClaims(user);

            return Request.CreateResponse(HttpStatusCode.Created, model);
        }

        // GET api/accounts/emails
        public async Task<HttpResponseMessage> Get()
        {
            DomainUser user = await _userService.GetAsync(UserId);

            var emails = new List<ProfileEmail>();
            if (!string.IsNullOrEmpty(user.Email))
            {
                emails.Add(new ProfileEmail { Email = user.Email });
            }

            return Request.CreateResponse(HttpStatusCode.OK, emails);
        }

        // PUT api/accounts/emails
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Put(ChangePasswordModel value)
        {
            bool passwordMatch = false;

            try
            {
                passwordMatch = await _passwordService.ChangePasswordAsync(UserId, value.NewPassword, value.OldPassword);
            }
            catch (BadRequestException)
            {
                if (!passwordMatch)
                {
                    ModelState.AddModelError("OldPassword", ResponseMessages.AccountsInvalidOldPassword);
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/emails
        [CheckAccessHttp]
        public HttpResponseMessage Delete(ProfileEmailModel model)
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest);
        }
    }
}