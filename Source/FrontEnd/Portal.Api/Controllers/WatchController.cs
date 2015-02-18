// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Attributes.WebApi;
using Asp.Infrastructure.Filters;
using AutoMapper;
using Portal.BLL.Services;
using Portal.Common.Helpers;
using Portal.Domain;
using Portal.Domain.Admin;
using Portal.Domain.PortalContext;
using Portal.DTO.Watch;
using Portal.Mappers;
using Portal.Resources.Api;

namespace Portal.Api.Controllers
{
    [ValidationHttp]
    [Route("watch/{projectId?}")]
    public class WatchController : ApiControllerBase
    {
        private const string UserIdAllConstant = "$all";

        private readonly IMapper _mapper;
        private readonly IProjectLikesService _projectLikesService;
        private readonly IWatchProjectService _watchProjectRepository;

        public WatchController(IWatchProjectService watchProjectRepository, IMapper mapper, IProjectLikesService projectLikesService)
        {
            _watchProjectRepository = watchProjectRepository;
            _mapper = mapper;
            _projectLikesService = projectLikesService;
        }

        /// <summary>
        ///     Gets a project by id.
        /// </summary>
        /// <param name="projectId">Project identifier.</param>
        /// <returns>Project entity.</returns>
        [StatWatchingWebApi]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> Get(string projectId)
        {
            Watch project = await _watchProjectRepository.GetByIdAsync(
                projectId,
                UserId);

            // Post-processing
            if (User.IsInRole(DomainRoles.User))
            {
                try
                {
                    project.IsLiked = await _projectLikesService.IsLikedAsync(projectId, UserId);
                    project.IsDisliked = await _projectLikesService.IsDislikedAsync(projectId, UserId);
                }
                catch (Exception e)
                {
                    Trace.TraceError("Failed to get like state for project '{0}': {1}", project.Id, e);
                }
            }

            // For statistics
            Request.Properties.Add("isReady", project.State == WatchState.Ready);

            return Request.CreateResponse(HttpStatusCode.OK, project);
        }

        /// <summary>
        ///     Gets a queryable collection of projects.
        /// </summary>
        /// <returns></returns>
        [ODataValidation]
        [ODataValidationExceptionFilter]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> Get(ODataQueryOptions<Watch> options)
        {
            var validationSettings = new ODataValidationSettings
            {
                MaxTop = 100,
                AllowedArithmeticOperators = AllowedArithmeticOperators.None,
                AllowedFunctions = AllowedFunctions.None,
                AllowedLogicalOperators =
                    AllowedLogicalOperators.Equal | AllowedLogicalOperators.And | AllowedLogicalOperators.GreaterThan | AllowedLogicalOperators.GreaterThanOrEqual | AllowedLogicalOperators.LessThan |
                    AllowedLogicalOperators.LessThanOrEqual,
                AllowedQueryOptions = AllowedQueryOptions.Filter | AllowedQueryOptions.OrderBy |
                                      AllowedQueryOptions.Skip | AllowedQueryOptions.Top | AllowedQueryOptions.InlineCount
            };

            validationSettings.AllowedOrderByProperties.Add("Name");
            validationSettings.AllowedOrderByProperties.Add("Created");
            validationSettings.AllowedOrderByProperties.Add("HitsCount");


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


            // Special instructions for UserId filtering
            DataFilterRule userIdFilter = filter.Filters.FirstOrDefault(f => string.Compare(f.Name, NameOfHelper.PropertyName<Watch>(x => x.UserId),
                StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal);
            if (userIdFilter == null || userIdFilter.Value == null)
            {
                // For backward compatibility we treats not specified or empty UserId as current user
                if (string.IsNullOrEmpty(UserId))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, ResponseMessages.UnathorizedRequest));
                }

                if (userIdFilter == null)
                {
                    userIdFilter = new DataFilterRule { Name = NameOfHelper.PropertyName<Watch>(x => x.UserId), Type = DataFilterTypes.Equal, Value = UserId };
                    filter.Filters.Add(userIdFilter);
                }
                else
                {
                    userIdFilter.Value = UserId;
                }
            }
            else if (string.Compare(userIdFilter.Value.ToString(), UserIdAllConstant, StringComparison.OrdinalIgnoreCase) == 0)
            {
                // special constant $all means not filtering by UserId
                userIdFilter.Value = null;
            }


            // Retrieving projects
            DataResult<Watch> watchProjects;
            try
            {
                watchProjects = await _watchProjectRepository.GetSequenceAsync(filter, UserId);
            }
            catch (NotSupportedException)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest));
            }

            if (filter.Count)
            {
                var pageResult = new PageResult<Watch>(watchProjects.Results, null, watchProjects.Count);
                return Request.CreateResponse(HttpStatusCode.OK, pageResult);
            }

            return Request.CreateResponse(HttpStatusCode.OK, watchProjects.Results);
        }
    }
}