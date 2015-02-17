// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Filters;
using Portal.Api.Models;
using Portal.BLL.Subscriptions;
using Portal.Domain.PortalContext;
using Portal.Domain.SubscriptionContext;
using Portal.DTO.Subscriptions;
using Portal.Mappers;
using Portal.Resources.Api;

namespace Portal.Api.Controllers.Clients
{
    /// <summary>
    ///     Manages client subscriptions.
    /// </summary>
    [ValidationHttp]
    [AuthorizeHttp(Roles = DomainRoles.Client)]
    [Route("clients/subscriptions/{id?}")]
    public class ClientSubscriptionsController : ApiControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly ISubscriptionService _subscriptionService;

        public ClientSubscriptionsController(
            ICompanyService companyService,
            ISubscriptionService subscriptionService,
            IMapper mapper)
        {
            _companyService = companyService;
            _subscriptionService = subscriptionService;
            _mapper = mapper;
        }

        //
        // GET: /api/clients/subscriptions

        /// <summary>
        ///     Gets client subscriptions.
        /// </summary>
        /// <returns></returns>
        [CustomQueryable]
        [ODataValidationExceptionFilter]
        public async Task<IEnumerable<Subscription>> Get()
        {
            // Get company for user
            DomainCompany company = await _companyService.FindByUserAsync(UserId);

            List<CompanySubscription> activeSubscriptions = company.Subscriptions;

            return _mapper.Map<List<CompanySubscription>, List<Subscription>>(activeSubscriptions);
        }


        //
        // GET: /api/clients/subscriptions/{id}

        /// <summary>
        ///     Gets client subscription.
        /// </summary>
        /// <returns></returns>
        public async Task<Subscription> Get(string id)
        {
            // Get company for user
            DomainCompany company = await _companyService.FindByUserAsync(UserId);

            CompanySubscription subscription = company.Subscriptions.FirstOrDefault(s => s.Id == id);
            if (subscription == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ResponseMessages.ResourceNotFound));
            }

            return _mapper.Map<CompanySubscription, Subscription>(subscription);
        }


        //
        // POST: /api/clients/subscriptions

        /// <summary>
        ///     Adds client subscription.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Post(CreateSubscriptionModel model)
        {
            // Create subscription
            CompanySubscription subscription = await _subscriptionService.AddAsync(UserId, model);

            Subscription subscriptionResponse = _mapper.Map<CompanySubscription, Subscription>(subscription);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, subscriptionResponse);

            return response;
        }


        //
        // PUT: /api/clients/subscriptions/{id}

        /// <summary>
        ///     Updates client subscription.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Put(string id, SubscriptionModel model)
        {
            // Update subscription
            CompanySubscription subscription = await _subscriptionService.UpdateAsync(UserId, id, model);

            Subscription profileResponse = _mapper.Map<CompanySubscription, Subscription>(subscription);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, profileResponse);

            return response;
        }


        //
        // DELETE: /api/clients/subscriptions/{id}

        /// <summary>
        ///     Deletes client subscription.
        /// </summary>
        /// <returns></returns>
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Delete(string id)
        {
            // Delete subscription
            await _subscriptionService.DeleteAsync(UserId, id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}