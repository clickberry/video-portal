// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Filters;
using AutoMapper;
using Portal.BLL.Subscriptions;
using Portal.Domain;
using Portal.Domain.PortalContext;
using Portal.Domain.SubscriptionContext;
using Portal.DTO.Subscriptions;
using Portal.Mappers;
using Portal.Resources.Api;

namespace Portal.Api.Controllers.Clients
{
    /// <summary>
    ///     Subscription statistics.
    /// </summary>
    [AuthorizeHttp(Roles = DomainRoles.Client)]
    [Route("clients/subscriptions/{id}/stats")]
    public class ClientSubscriptionStatisticsController : ApiControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly IUrlTrackingStatService _urlTrackingStatService;

        public ClientSubscriptionStatisticsController(
            ICompanyService companyService,
            IUrlTrackingStatService urlTrackingStatService,
            IMapper mapper)
        {
            _companyService = companyService;
            _urlTrackingStatService = urlTrackingStatService;
            _mapper = mapper;
        }

        //
        // GET: /api/clients/subscriptions/{id}/stats

        /// <summary>
        ///     Gets subscription statistics per url.
        /// </summary>
        /// <returns></returns>
        [ODataValidationExceptionFilter]
        [ODataValidation]
        public async Task<HttpResponseMessage> Get(string id, ODataQueryOptions<TrackingStatPerUrl> options)
        {
            var validationSettings = new ODataValidationSettings
            {
                MaxTop = 100,
                AllowedArithmeticOperators = AllowedArithmeticOperators.None,
                AllowedFunctions = AllowedFunctions.None,
                AllowedLogicalOperators =
                    AllowedLogicalOperators.Equal | AllowedLogicalOperators.And,
                AllowedQueryOptions =
                    AllowedQueryOptions.Filter | AllowedQueryOptions.OrderBy | AllowedQueryOptions.Skip | AllowedQueryOptions.Top | AllowedQueryOptions.InlineCount
            };

            // Validating query options
            try
            {
                options.Validate(validationSettings);
            }
            catch (Exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.InvalidQueryOptions));
            }


            // Parsing filter parameters
            DataQueryOptions filter;
            try
            {
                filter = _mapper.Map<ODataQueryOptions, DataQueryOptions>(options);
            }
            catch (AutoMapperMappingException)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.InvalidQueryOptions));
            }

            // Get company for user
            DomainCompany company = await _companyService.FindByUserAsync(UserId);

            // Find subscription
            CompanySubscription subscription = company.Subscriptions.FirstOrDefault(s => s.Id == id);
            if (subscription == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ResponseMessages.ResourceNotFound));
            }


            // Get stats
            DataResult<DomainTrackingStatPerUrl> stats;
            try
            {
                stats = await _urlTrackingStatService.GetStatsPerUrlAsync(id, filter);
            }
            catch (NotSupportedException)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest));
            }
            catch (ArgumentException)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest));
            }

            IEnumerable<TrackingStatPerUrl> results = stats.Results.Select(r => _mapper.Map<DomainTrackingStatPerUrl, TrackingStatPerUrl>(r));

            if (filter.Count)
            {
                var pageResult = new PageResult<TrackingStatPerUrl>(results, null, stats.Count);
                return Request.CreateResponse(HttpStatusCode.OK, pageResult);
            }

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }
    }
}