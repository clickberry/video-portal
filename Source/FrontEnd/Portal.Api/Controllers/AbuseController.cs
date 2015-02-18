// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Portal.Domain.ProfileContext;
using Portal.DTO.User;
using Portal.DTO.Watch;
using Portal.Mappers;
using Portal.Resources.Api;

namespace Portal.Api.Controllers
{
    [ValidationHttp]
    [Route("abuse/{id}")]
    public class AbuseController : ApiControllerBase
    {
        private readonly ICassandraStatisticsService _cassandraStatisticsService;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IMapper _mapper;
        private readonly IProjectAbuseService _projectAbuseService;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly IWatchProjectService _watchProjectService;

        public AbuseController(ICassandraStatisticsService cassandraStatisticsService,
            IWatchProjectService watchProjectService,
            IProjectService projectService,
            IProjectAbuseService projectAbuseService,
            IUserService userService,
            IEmailNotificationService emailNotificationService,
            IMapper mapper)
        {
            _cassandraStatisticsService = cassandraStatisticsService;
            _watchProjectService = watchProjectService;
            _projectService = projectService;
            _projectAbuseService = projectAbuseService;
            _userService = userService;
            _emailNotificationService = emailNotificationService;
            _mapper = mapper;
        }


        // GET: /api/abuse/{id}

        /// <summary>
        ///     Users who report abuse for project.
        /// </summary>
        /// <returns></returns>
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
                users = await _projectAbuseService.GetProjectReportersSequenceAsync(id, filter);
            }
            catch (NotSupportedException)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest));
            }

            return Request.CreateResponse(HttpStatusCode.OK, users);
        }


        // POST: /api/abuse/{id}

        /// <summary>
        ///     Increments abuse counter for project.
        /// </summary>
        /// <param name="id">Project id</param>
        /// <returns></returns>
        [AuthorizeHttp(Roles = DomainRoles.User)]
        public async Task<HttpResponseMessage> Post(string id)
        {
            Watch project = await _watchProjectService.GetByIdAsync(id, UserId);

            // Add to statistics in background
            Task.Run(() => _cassandraStatisticsService.AddAbuseAsync(StatisticsSpaces.Projects, id, UserId))
                .ContinueWith(r => _projectService.IncrementAbuseCounterAsync(id), TaskContinuationOptions.OnlyOnRanToCompletion).NoWarning();

            // Send activation e-mail
            try
            {
                DomainUser reporter = await _userService.GetAsync(UserId);
                await _emailNotificationService.SendAbuseNotificationAsync(project, reporter);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to send abuse report e-mail from user {0}: {1}", UserId, e);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}