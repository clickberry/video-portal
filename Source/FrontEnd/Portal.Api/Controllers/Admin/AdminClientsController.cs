// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Filters;
using AutoMapper;
using Portal.Api.Models;
using Portal.BLL.Concrete.Infrastructure;
using Portal.BLL.Concrete.Infrastructure.MediaTypeFormatters;
using Portal.BLL.Services;
using Portal.Domain;
using Portal.Domain.Admin;
using Portal.Domain.PortalContext;
using Portal.DTO.Admin;
using Portal.Mappers;
using Portal.Resources.Api;

namespace Portal.Api.Controllers.Admin
{
    [ValidationHttp]
    [Route("admin/clients/{id?}")]
    [AuthorizeHttp(Roles = DomainRoles.AllAdministrators)]
    public class AdminClientsController : ApiControllerBase
    {
        private readonly IAdminClientService _adminClientService;
        private readonly ICsvPublishServiceFactory _csvPublishServiceFactory;
        private readonly IMapper _mapper;

        public AdminClientsController(IAdminClientService adminClientService,
            IMapper mapper,
            ICsvPublishServiceFactory csvPublishServiceFactory)
        {
            _adminClientService = adminClientService;
            _mapper = mapper;
            _csvPublishServiceFactory = csvPublishServiceFactory;
        }

        public async Task<HttpResponseMessage> Get(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, await _adminClientService.GetAsync(id));
        }

        //
        // GET /api/admin/clients
        [ODataValidation]
        [ODataValidationExceptionFilter]
        public async Task<HttpResponseMessage> Get(ODataQueryOptions<AdminClient> options)
        {
            var validationSettings = new ODataValidationSettings
            {
                MaxTop = 100,
                AllowedArithmeticOperators = AllowedArithmeticOperators.None,
                AllowedFunctions = AllowedFunctions.None,
                AllowedLogicalOperators =
                    AllowedLogicalOperators.Equal | AllowedLogicalOperators.LessThan | AllowedLogicalOperators.LessThanOrEqual | AllowedLogicalOperators.GreaterThan |
                    AllowedLogicalOperators.GreaterThanOrEqual | AllowedLogicalOperators.And,
                AllowedQueryOptions =
                    AllowedQueryOptions.Filter | AllowedQueryOptions.OrderBy | AllowedQueryOptions.Skip | AllowedQueryOptions.Top | AllowedQueryOptions.Format | AllowedQueryOptions.InlineCount
            };

            validationSettings.AllowedOrderByProperties.Add("Name");
            validationSettings.AllowedOrderByProperties.Add("Created");

            try
            {
                options.Validate(validationSettings);
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.InvalidQueryOptions);
            }

            // Determine media formatter
            IContentNegotiator negotiator = Configuration.Services.GetContentNegotiator();
            ContentNegotiationResult negotiationResult = negotiator.Negotiate(typeof (AdminClient), Request, Configuration.Formatters);
            if (negotiationResult == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, ResponseMessages.NotAcceptable);
            }

            // Parsing filter parameters
            DataQueryOptions filter;
            try
            {
                filter = _mapper.Map<ODataQueryOptions, DataQueryOptions>(options);
            }
            catch (AutoMapperMappingException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.InvalidQueryOptions);
            }


            // CSV
            if (negotiationResult.Formatter is ClientForAdminCsvFormatter)
            {
                // get csv service
                ICsvPublishService csvService = _csvPublishServiceFactory.GetService<AdminClient>();

                // send the link to the future csv file
                string fileName = string.Format("{0}.csv", Guid.NewGuid());
                Uri fileUri = csvService.GetAccessCsvUri(fileName);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Accepted);
                response.Headers.Location = fileUri;

                // disable paging
                filter.Take = null;

                // start csv publishing in background thread
                Task.Run(() => csvService.PublishAsync(filter, fileName, new CancellationTokenSource())).NoWarning();

                return response;
            }


            // retrieving clients
            DataResult<Task<DomainClientForAdmin>> result;
            IEnumerable<Task<AdminClient>> clientTasks;
            try
            {
                // filtering clients
                result = _adminClientService.GetAsyncSequence(filter);
                clientTasks = result.Results.Select(t => t.ContinueWith(u => _mapper.Map<DomainClientForAdmin, AdminClient>(u.Result)));
            }
            catch (NotSupportedException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest);
            }

            // loading and shaping all clients
            List<AdminClient> clients = (await Task.WhenAll(clientTasks)).ToList();

            if (filter.Count)
            {
                var pageResult = new PageResult<AdminClient>(clients, null, result.Count);
                return Request.CreateResponse(HttpStatusCode.OK, pageResult);
            }

            return Request.CreateResponse(HttpStatusCode.OK, clients);
        }

        [AuthorizeHttp(Roles = DomainRoles.SuperAdministrator)]
        public async Task<HttpResponseMessage> Put(string id, AdminClientUpdateModel model)
        {
            DomainClientForAdmin client = await _adminClientService.SetStateAsync(id, model.State);

            return Request.CreateResponse(_mapper.Map<DomainClientForAdmin, AdminClient>(client));
        }
    }
}