// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Portal.Api.Models;
using Portal.BLL.Services;
using Portal.Domain.Admin;
using Portal.Domain.PortalContext;
using Portal.DTO.Admin;
using Portal.Mappers;

namespace Portal.Api.Controllers.Admin
{
    [AuthorizeHttp(Roles = DomainRoles.AllAdministrators)]
    [RoutePrefix("admin/clients/{clientId}/subscriptions")]
    public class AdminClientSubscriptionsController : ApiController
    {
        private readonly IAdminClientSubscriptionService _adminClientSubscriptionService;
        private readonly IMapper _mapper;

        public AdminClientSubscriptionsController(IAdminClientSubscriptionService adminClientSubscriptionService, IMapper mapper)
        {
            _adminClientSubscriptionService = adminClientSubscriptionService;
            _mapper = mapper;
        }

        [Route]
        public async Task<HttpResponseMessage> Get(string clientId)
        {
            IEnumerable<DomainAdminClientSubscription> domainSubscriptions = await _adminClientSubscriptionService.GetSubscriptionsAsync(clientId);
            IEnumerable<AdminClientSubscription> subscriptions = domainSubscriptions.Select(s => _mapper.Map<DomainAdminClientSubscription, AdminClientSubscription>(s));

            return Request.CreateResponse(HttpStatusCode.OK, subscriptions);
        }

        [Route("{subscriptionId}")]
        public async Task<HttpResponseMessage> Put(string clientId, string subscriptionId, AdminClientSubscriptionUpdateModel adminClientSubscriptionModel)
        {
            var domainSubscription = new DomainAdminClientSubscription
            {
                Id = subscriptionId,
                IsManuallyEnabled = adminClientSubscriptionModel.IsManuallyEnabled,
                State = adminClientSubscriptionModel.State
            };

            domainSubscription = await _adminClientSubscriptionService.EditSubscriptionAsync(clientId, domainSubscription);
            AdminClientSubscription subscription = _mapper.Map<DomainAdminClientSubscription, AdminClientSubscription>(domainSubscription);

            return Request.CreateResponse(HttpStatusCode.OK, subscription);
        }
    }
}