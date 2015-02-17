// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData.Query;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Filters;
using AutoMapper;
using Portal.Api.Models;
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
    [ValidationHttp]
    [Route("clients/subscriptions/{id}/stats/group/date")]
    public class ClientSubscriptionGroupedByDateStatisticsController : ApiControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly IUrlTrackingStatService _urlTrackingStatService;

        public ClientSubscriptionGroupedByDateStatisticsController(
            ICompanyService companyService,
            IUrlTrackingStatService urlTrackingStatService,
            IMapper mapper)
        {
            _companyService = companyService;
            _urlTrackingStatService = urlTrackingStatService;
            _mapper = mapper;
        }

        //
        // GET: /api/clients/subscriptions/{id}/stats/group/date?url={url}

        /// <summary>
        ///     Gets subscription statistics grouped by date.
        /// </summary>
        /// <returns></returns>
        [ODataValidationExceptionFilter]
        [ODataValidation]
        public async Task<IEnumerable<TrackingStatPerDate>> Get(string id, [FromUri] ClientSubscriptionStatUrlModel model, ODataQueryOptions<TrackingStatPerDate> options)
        {
            var validationSettings = new ODataValidationSettings
            {
                AllowedArithmeticOperators = AllowedArithmeticOperators.None,
                AllowedFunctions = AllowedFunctions.None,
                AllowedLogicalOperators =
                    AllowedLogicalOperators.Equal | AllowedLogicalOperators.LessThan | AllowedLogicalOperators.LessThanOrEqual | AllowedLogicalOperators.GreaterThan |
                    AllowedLogicalOperators.GreaterThanOrEqual | AllowedLogicalOperators.And,
                AllowedQueryOptions = AllowedQueryOptions.Filter
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
            DomainCompany company = await _companyService.FindBySubscriptionAsync(id);

            if (!company.Users.Contains(UserId))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, ResponseMessages.Forbidden));
            }

            // Find subscription
            CompanySubscription subscription = company.Subscriptions.FirstOrDefault(s => s.Id == id);
            if (subscription == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ResponseMessages.ResourceNotFound));
            }

            // Get stats
            IEnumerable<DomainTrackingStatPerDate> stats;
            try
            {
                stats = await _urlTrackingStatService.GetStatsPerDateAsync(id, model.Url, filter);
            }
            catch (NotSupportedException)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest));
            }
            catch (ArgumentException)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest));
            }

            IEnumerable<TrackingStatPerDate> results = stats.Select(r => _mapper.Map<DomainTrackingStatPerDate, TrackingStatPerDate>(r));
            return results;
        }
    }
}