// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Extensions;
using Portal.Api.Models;
using Portal.BLL.Services;
using Portal.BLL.Subscriptions;
using Portal.Domain.PortalContext;
using Portal.Domain.ProfileContext;
using Portal.Domain.SubscriptionContext;
using Portal.DTO.Subscriptions;
using Portal.Exceptions.CRUD;
using Portal.Mappers;
using Portal.Resources.Api;

namespace Portal.Api.Controllers.Clients
{
    /// <summary>
    ///     Manages client accounts.
    /// </summary>
    [AuthorizeHttp(Roles = DomainRoles.Client)]
    [ValidationHttp]
    [Route("clients")]
    public class ClientsController : ApiControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IMapper _mapper;
        private readonly IPendingClientService _pendingClientService;
        private readonly IUserService _userService;

        public ClientsController(
            IUserService userService,
            IEmailNotificationService emailNotificationService,
            ICompanyService companyService,
            IPendingClientService pendingClientService,
            IMapper mapper)
        {
            _userService = userService;
            _emailNotificationService = emailNotificationService;
            _companyService = companyService;
            _pendingClientService = pendingClientService;
            _mapper = mapper;
        }


        //
        // POST: /api/clients

        /// <summary>
        ///     Register client.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [CheckAccessHttp]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Post(RegisterClientModel model)
        {
            // Check whether user with same e-mail already exists
            DomainUser user = null;
            try
            {
                user = await _userService.FindByEmailAsync(model.Email);
            }
            catch (NotFoundException)
            {
            }

            if (user != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, ResponseMessages.EntityAlreadyExists);
            }

            // Add pending client
            DomainPendingClient client = await _pendingClientService.AddAsync(model);

            // Send activation e-mail
            try
            {
                await _emailNotificationService.SendClientActivationEmailAsync(client);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to send registration e-mail to client {0}: {1}", client.Id, e);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }


        //
        // GET: /api/clients

        /// <summary>
        ///     Gets client profile information.
        /// </summary>
        /// <returns>Client profile information.</returns>
        public async Task<HttpResponseMessage> Get()
        {
            // Get profile
            DomainUser user = await _userService.GetAsync(UserId);

            // Get company
            DomainCompany company = await _companyService.FindByUserAsync(UserId);

            Client client = _mapper.Map<Tuple<DomainUser, DomainCompany>, Client>(new Tuple<DomainUser, DomainCompany>(user, company));

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, client);
            response.SetLastModifiedDate(user.Modified);

            return response;
        }


        //
        // PUT: /api/clients

        /// <summary>
        ///     Edits client profile information.
        /// </summary>
        /// <returns>Account information.</returns>
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Put(ClientModel model)
        {
            // Change e-mail
            await _userService.ChangeEmailAsync(UserId, model.Email);

            // Update user
            var userUpdate = new UserUpdateOptions
            {
                Country = model.Country,
                UserName = model.ContactPerson
            };

            DomainUser user = await _userService.UpdateAsync(UserId, userUpdate);

            // Update company
            var companyUpdate = new CompanyUpdateOptions
            {
                Address = model.Address,
                Country = model.Country,
                Ein = model.Ein,
                Name = model.CompanyName,
                Phone = model.PhoneNumber,
                ZipCode = model.ZipCode,
                Email = model.Email
            };

            DomainCompany company = await _companyService.UpdateByUserAsync(UserId, companyUpdate);

            Client client = _mapper.Map<Tuple<DomainUser, DomainCompany>, Client>(new Tuple<DomainUser, DomainCompany>(user, company));

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, client);
            return response;
        }


        //
        // DELETE api/clients

        /// <summary>
        ///     Deletes current client.
        /// </summary>
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Delete()
        {
            // Delete client role
            await _userService.DeleteRoleAsync(UserId, DomainRoles.Client);

            // Delete company
            await _companyService.DeleteByUserAsync(UserId);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}