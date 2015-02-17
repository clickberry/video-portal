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
using Portal.BLL.Services;
using Portal.Common.Helpers;
using Portal.Domain;
using Portal.Domain.Admin;
using Portal.Domain.PortalContext;
using Portal.DTO.Admin;
using Portal.Mappers;
using Portal.Resources.Api;

namespace Portal.Api.Controllers.Admin
{
    [AuthorizeHttp(Roles = DomainRoles.AllAdministrators)]
    [ValidationHttp]
    [Route("admin/users/{id}/projects")]
    public class AdminUserProjectsController : ApiControllerBase
    {
        private readonly IAdminProjectService _adminProjectService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AdminUserProjectsController(IUserService userService, IAdminProjectService adminProjectService, IMapper mapper)
        {
            _userService = userService;
            _adminProjectService = adminProjectService;
            _mapper = mapper;
        }

        // GET api/users/5/projects
        [ODataValidation]
        [ODataValidationExceptionFilter]
        public async Task<HttpResponseMessage> Get(string id, ODataQueryOptions<AdminProject> options)
        {
            // Checks whether user exists
            await _userService.GetAsync(id);

            var validationSettings = new ODataValidationSettings
            {
                MaxTop = 100,
                AllowedArithmeticOperators = AllowedArithmeticOperators.None,
                AllowedFunctions = AllowedFunctions.None,
                AllowedLogicalOperators =
                    AllowedLogicalOperators.Equal | AllowedLogicalOperators.LessThan | AllowedLogicalOperators.LessThanOrEqual | AllowedLogicalOperators.GreaterThan |
                    AllowedLogicalOperators.GreaterThanOrEqual | AllowedLogicalOperators.And,
                AllowedQueryOptions = AllowedQueryOptions.Filter | AllowedQueryOptions.OrderBy | AllowedQueryOptions.Skip | AllowedQueryOptions.Top | AllowedQueryOptions.InlineCount
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


            // retrieving projects
            DataResult<Task<DomainProjectForAdmin>> result;
            IEnumerable<Task<AdminProject>> projectTasks;
            DataQueryOptions filter;

            // parsing filter parameters
            try
            {
                filter = _mapper.Map<ODataQueryOptions, DataQueryOptions>(options);
            }
            catch (AutoMapperMappingException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.InvalidQueryOptions);
            }


            filter.Filters.Add(new DataFilterRule
            {
                Name = NameOfHelper.PropertyName<DomainProjectForAdmin>(x => x.UserId),
                Type = DataFilterTypes.Equal,
                Value = id
            });


            try
            {
                // filtering projects
                result = await _adminProjectService.GetAsyncSequenceAsync(filter);
                projectTasks = result.Results.Select(t => t.ContinueWith(u => _mapper.Map<DomainProjectForAdmin, AdminProject>(u.Result)));
            }
            catch (NotSupportedException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest);
            }

            // loading and shaping all projects
            List<AdminProject> projects = (await Task.WhenAll(projectTasks)).ToList();

            if (filter.Count)
            {
                var pageResult = new PageResult<AdminProject>(projects, null, result.Count);
                return Request.CreateResponse(HttpStatusCode.OK, pageResult);
            }

            return Request.CreateResponse(HttpStatusCode.OK, projects);
        }
    }
}