// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
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
using Portal.DTO.Projects;
using Portal.Mappers;
using Portal.Resources.Api;

namespace Portal.Api.Controllers
{
    [Route("examples")]
    public class ExamplesController : ApiControllerBase
    {
        private readonly IExampleProjectService _exampleProjectService;
        private readonly IMapper _mapper;

        public ExamplesController(IExampleProjectService exampleProjectService, IMapper mapper)
        {
            _exampleProjectService = exampleProjectService;
            _mapper = mapper;
        }

        [ODataValidation]
        [ODataValidationExceptionFilter]
        public async Task<HttpResponseMessage> Get(ODataQueryOptions<ExampleProject> options)
        {
            var validationSettings = new ODataValidationSettings
            {
                MaxTop = 100,
                AllowedArithmeticOperators = AllowedArithmeticOperators.None,
                AllowedFunctions = AllowedFunctions.None,
                AllowedLogicalOperators = AllowedLogicalOperators.None,
                AllowedQueryOptions = AllowedQueryOptions.Skip | AllowedQueryOptions.Top | AllowedQueryOptions.OrderBy
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
            var projects = await _exampleProjectService.GetSequenceAsync(filter);

            return Request.CreateResponse(HttpStatusCode.OK, projects);
        }
    }
}