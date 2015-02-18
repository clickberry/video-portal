// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

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
using Portal.BLL.Concrete.Infrastructure;
using Portal.BLL.Concrete.Statistics;
using Portal.BLL.Services;
using Portal.BLL.Statistics;
using Portal.Domain;
using Portal.Domain.PortalContext;
using Portal.DTO.User;
using Portal.DTO.Watch;
using Portal.Mappers;
using Portal.Resources.Api;

namespace Portal.Api.Controllers
{
    [ValidationHttp]
    [RoutePrefix("dislikes")]
    public class DislikesController : ApiControllerBase
    {
        private readonly ICassandraStatisticsService _cassandraStatisticsService;
        private readonly IMapper _mapper;
        private readonly IProjectLikesService _projectLikesService;
        private readonly IProjectService _projectService;
        private readonly IWatchProjectService _watchProjectService;

        public DislikesController(ICassandraStatisticsService cassandraStatisticsService,
            IWatchProjectService watchProjectService,
            IProjectLikesService projectLikesService,
            IProjectService projectService,
            IMapper mapper)
        {
            _cassandraStatisticsService = cassandraStatisticsService;
            _watchProjectService = watchProjectService;
            _projectLikesService = projectLikesService;
            _projectService = projectService;
            _mapper = mapper;
        }

        // GET: /api/dislikes

        /// <summary>
        ///     Gets user dislikes.
        /// </summary>
        /// <returns></returns>
        [AuthorizeHttp(Roles = DomainRoles.User)]
        [Route]
        [ODataValidation(MinTopValue = 1)]
        [ODataValidationExceptionFilter]
        public async Task<HttpResponseMessage> Get(ODataQueryOptions<Watch> options)
        {
            var validationSettings = new ODataValidationSettings
            {
                MaxTop = 100,
                AllowedArithmeticOperators = AllowedArithmeticOperators.None,
                AllowedFunctions = AllowedFunctions.None,
                AllowedLogicalOperators = AllowedLogicalOperators.None,
                AllowedQueryOptions = AllowedQueryOptions.Skip | AllowedQueryOptions.Top
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
            IEnumerable<Watch> watchProjects;
            try
            {
                watchProjects = await _projectLikesService.GetUserDislikesSequenceAsync(UserId, filter);
            }
            catch (NotSupportedException)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest));
            }

            return Request.CreateResponse(HttpStatusCode.OK, watchProjects);
        }


        // GET: /api/dislikes/{id}

        /// <summary>
        ///     Project dislikers.
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        [ODataValidation(MinTopValue = 1)]
        [ODataValidationExceptionFilter]
        public async Task<HttpResponseMessage> Get(string id, ODataQueryOptions<UserInfo> options)
        {
            await _watchProjectService.CheckProjectAsync(id, UserId);

            var validationSettings = new ODataValidationSettings
            {
                MaxTop = 100,
                AllowedArithmeticOperators = AllowedArithmeticOperators.None,
                AllowedFunctions = AllowedFunctions.None,
                AllowedLogicalOperators = AllowedLogicalOperators.None,
                AllowedQueryOptions = AllowedQueryOptions.Skip | AllowedQueryOptions.Top
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
            IEnumerable<UserInfo> users;
            try
            {
                users = await _projectLikesService.GetProjectDislikersSequenceAsync(id, filter);
            }
            catch (NotSupportedException)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest));
            }

            return Request.CreateResponse(HttpStatusCode.OK, users);
        }


        // POST: /api/dislikes/{id}

        /// <summary>
        ///     Dislikes project.
        /// </summary>
        /// <param name="id">Project id</param>
        /// <returns></returns>
        [AuthorizeHttp(Roles = DomainRoles.User)]
        [Route("{id}")]
        public async Task<HttpResponseMessage> Post(string id)
        {
            await _watchProjectService.CheckProjectAsync(id, UserId);

            // Add to statistics in background
            Task.Run(() => _cassandraStatisticsService.AddDislikeAsync(StatisticsSpaces.Projects, id, UserId))
                .ContinueWith(async r =>
                {
                    // counting project dislikers approximately
                    long dislikesCount = await _projectLikesService.GetProjectDislikesCountAsync(id);

                    // updating number of project dislikes
                    return _projectService.UpdateDislikesCounterAsync(id, dislikesCount);
                }).NoWarning();

            return Request.CreateResponse(HttpStatusCode.OK);
        }


        // DELETE: /api/dislikes/{id}

        /// <summary>
        ///     Deletes project dislike.
        /// </summary>
        /// <param name="id">Project id</param>
        /// <returns></returns>
        [AuthorizeHttp(Roles = DomainRoles.User)]
        [Route("{id}")]
        public async Task<HttpResponseMessage> Delete(string id)
        {
            await _watchProjectService.CheckProjectAsync(id, UserId);

            // Delete like from cassandra in background
            Task.Run(() => _cassandraStatisticsService.DeleteDislikeAsync(StatisticsSpaces.Projects, id, UserId))
                .ContinueWith(async r =>
                {
                    // counting project dislikers approximately
                    long dislikesCount = await _projectLikesService.GetProjectDislikesCountAsync(id);

                    // updating number of project dislikes
                    return _projectService.UpdateDislikesCounterAsync(id, dislikesCount);
                }).NoWarning();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}