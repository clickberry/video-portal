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
    ///     Subscription url statistics.
    /// </summary>
    [AuthorizeHttp(Roles = DomainRoles.Client)]
    [Route("clients/subscriptions/{id}/stats/url")]
    public class ClientSubscriptionUrlStatisticsController : ApiControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly IUrlTrackingStatService _urlTrackingStatService;

        public ClientSubscriptionUrlStatisticsController(
            ICompanyService companyService,
            IUrlTrackingStatService urlTrackingStatService,
            IMapper mapper)
        {
            _companyService = companyService;
            _urlTrackingStatService = urlTrackingStatService;
            _mapper = mapper;
        }

        //
        // GET: /api/clients/subscriptions/{id}/stats/url

        /// <summary>
        ///     Gets url statistics.
        /// </summary>
        /// <returns></returns>
        [ODataValidationExceptionFilter]
        [ODataValidation]
        public async Task<HttpResponseMessage> Get(string id, string url, ODataQueryOptions<TrackingStat> options)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.InvalidQueryOptions));
            }

            var validationSettings = new ODataValidationSettings
            {
                MaxTop = 100,
                AllowedArithmeticOperators = AllowedArithmeticOperators.None,
                AllowedFunctions = AllowedFunctions.None,
                AllowedLogicalOperators =
                    AllowedLogicalOperators.Equal | AllowedLogicalOperators.LessThan | AllowedLogicalOperators.LessThanOrEqual | AllowedLogicalOperators.GreaterThan |
                    AllowedLogicalOperators.GreaterThanOrEqual | AllowedLogicalOperators.And,
                AllowedQueryOptions =
                    AllowedQueryOptions.Filter | AllowedQueryOptions.OrderBy | AllowedQueryOptions.Skip | AllowedQueryOptions.Top | AllowedQueryOptions.InlineCount
            };

            validationSettings.AllowedOrderByProperties.Add("Date");

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
            DataResult<DomainTrackingStat> stats;
            try
            {
                stats = await _urlTrackingStatService.GetUrlStatsAsync(id, url, filter);
            }
            catch (NotSupportedException)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest));
            }
            catch (ArgumentException)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest));
            }

            IEnumerable<TrackingStat> results = stats.Results.Select(r => _mapper.Map<DomainTrackingStat, TrackingStat>(r));

            if (filter.Count)
            {
                var pageResult = new PageResult<TrackingStat>(results, null, stats.Count);
                return Request.CreateResponse(HttpStatusCode.OK, pageResult);
            }

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }
    }
}