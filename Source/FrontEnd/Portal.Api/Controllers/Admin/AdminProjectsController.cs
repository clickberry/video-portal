// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

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
using Asp.Infrastructure.Attributes.WebApi;
using Asp.Infrastructure.Filters;
using AutoMapper;
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
    [Route("admin/projects/{id?}")]
    public class AdminProjectsController : ApiControllerBase
    {
        private readonly IAdminProjectService _adminProjectService;
        private readonly ICsvPublishServiceFactory _csvPublishServiceFactory;
        private readonly IMapper _mapper;


        public AdminProjectsController(IAdminProjectService adminProjectService,
            IMapper mapper,
            ICsvPublishServiceFactory csvPublishServiceFactory)
        {
            _adminProjectService = adminProjectService;
            _mapper = mapper;
            _csvPublishServiceFactory = csvPublishServiceFactory;
        }

        [AuthorizeHttp(Roles = DomainRoles.AllAdministrators)]
        public HttpResponseMessage Get(string id)
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest);
        }

        //
        // GET /api/admin/projects

        [AuthorizeHttp(Roles = DomainRoles.AllAdministrators)]
        [ODataValidation]
        [ODataValidationExceptionFilter]
        public async Task<HttpResponseMessage> Get(ODataQueryOptions<AdminProject> options)
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
            validationSettings.AllowedOrderByProperties.Add("ProductType");

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
            ContentNegotiationResult negotiationResult = negotiator.Negotiate(typeof (AdminProject), Request, Configuration.Formatters);
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
            if (negotiationResult.Formatter is ProjectForAdminCsvFormatter)
            {
                // get csv service
                ICsvPublishService csvService = _csvPublishServiceFactory.GetService<AdminProject>();

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


            // Filtering projects
            DataResult<Task<DomainProjectForAdmin>> result;
            IEnumerable<Task<AdminProject>> projectTasks;
            try
            {
                result = await _adminProjectService.GetAsyncSequenceAsync(filter);
                projectTasks = result.Results.Select(t => t.ContinueWith(u => _mapper.Map<DomainProjectForAdmin, AdminProject>(u.Result)));
            }
            catch (NotSupportedException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest);
            }


            // Loading and shaping all projects
            List<AdminProject> projects = (await Task.WhenAll(projectTasks)).ToList();

            if (filter.Count)
            {
                var pageResult = new PageResult<AdminProject>(projects, null, result.Count);
                return Request.CreateResponse(HttpStatusCode.OK, pageResult);
            }

            return Request.CreateResponse(HttpStatusCode.OK, projects);
        }

        //
        // DELETE /api/admin/projects/{id}
        [StatProjectDeletionWebApi]
        [CheckAccessHttp]
        [AuthorizeHttp(Roles = DomainRoles.SuperAdministrator)]
        public async Task<HttpResponseMessage> Delete(string id)
        {
            await _adminProjectService.DeleteAsync(new DomainProjectForAdmin { ProjectId = id });

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}