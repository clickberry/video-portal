// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData.Query;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Filters;
using AutoMapper;
using Portal.BLL.Services;
using Portal.Domain;
using Portal.DTO.Trends;
using Portal.Mappers;
using Portal.Resources.Api;

namespace Portal.Api.Controllers
{
    [ValidationHttp]
    [RoutePrefix("watch/trends")]
    public class TrendsController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProjectViewsService _viewsService;

        public TrendsController(IProjectViewsService viewsService, IMapper mapper)
        {
            _viewsService = viewsService;
            _mapper = mapper;
        }


        // GET: /api/watch/trends/week

        [Route("week")]
        [ODataValidation(MinTopValue = 1)]
        [ODataValidationExceptionFilter]
        public async Task<HttpResponseMessage> GetWeek(ODataQueryOptions<TrendingWatch> options)
        {
            var validationSettings = new ODataValidationSettings
            {
                MaxTop = 100,
                AllowedArithmeticOperators = AllowedArithmeticOperators.None,
                AllowedFunctions = AllowedFunctions.None,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal | AllowedLogicalOperators.And,
                AllowedQueryOptions = AllowedQueryOptions.Filter | AllowedQueryOptions.Skip | AllowedQueryOptions.Top
            };

            // Validating OData
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

            // Retrieving projects
            IEnumerable<TrendingWatch> watchProjects;
            try
            {
                watchProjects = await _viewsService.GetWeeklyTrendsSequenceAsync(filter);
            }
            catch (NotSupportedException)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest));
            }

            return Request.CreateResponse(HttpStatusCode.OK, watchProjects);
        }
    }
}