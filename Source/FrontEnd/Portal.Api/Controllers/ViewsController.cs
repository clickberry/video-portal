// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Portal.BLL.Concrete.Infrastructure;
using Portal.BLL.Concrete.Statistics;
using Portal.BLL.Services;
using Portal.BLL.Statistics;
using Portal.Domain.PortalContext;
using Portal.Domain.StatisticContext;

namespace Portal.Api.Controllers
{
    [ValidationHttp]
    [Route("views/{id}")]
    public class ViewsController : ApiControllerBase
    {
        private readonly ICassandraStatisticsService _cassandraStatisticsService;
        private readonly IProjectService _projectService;
        private readonly IWatchProjectService _watchProjectService;

        public ViewsController(ICassandraStatisticsService cassandraStatisticsService, IWatchProjectService watchProjectService, IProjectService projectService)
        {
            _cassandraStatisticsService = cassandraStatisticsService;
            _watchProjectService = watchProjectService;
            _projectService = projectService;
        }


        // GET: /api/views/{id}

        /// <summary>
        ///     Returns cumulative number of views for project.
        /// </summary>
        /// <param name="id">Project id</param>
        /// <returns></returns>
        public async Task<long> Get(string id)
        {
            await _watchProjectService.CheckProjectAsync(id, UserId);

            DomainItemCounts counts = await _cassandraStatisticsService.GetItemCountsAsync(StatisticsSpaces.Projects, id);
            return counts.Views;
        }


        // POST: /api/views/{id}

        /// <summary>
        ///     Increments view counter for project.
        /// </summary>
        /// <param name="id">Project id</param>
        /// <returns></returns>
        [AuthorizeHttp(Roles = DomainRoles.AllViewers, IsAuthenticationRequired = false)]
        public async Task<HttpResponseMessage> Post(string id)
        {
            if (User.IsInRole(DomainRoles.Administrator) || User.IsInRole(DomainRoles.SuperAdministrator))
            {
                // preventing admin views
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            await _watchProjectService.CheckProjectAsync(id, UserId);

            // Add to statistics in background
            Task.Run(() => _cassandraStatisticsService.AddViewAsync(StatisticsSpaces.Projects, id, UserId))
                .ContinueWith(r => _projectService.IncrementHitsCounterAsync(id), TaskContinuationOptions.OnlyOnRanToCompletion).NoWarning();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}