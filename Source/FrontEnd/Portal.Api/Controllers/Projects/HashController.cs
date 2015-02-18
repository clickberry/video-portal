// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Attributes.WebApi;
using Portal.Api.Models;
using Portal.BLL.Services;
using Portal.Domain.PortalContext;
using Portal.Domain.StatisticContext;
using Portal.Resources.Api;

namespace Portal.Api.Controllers.Projects
{
    /// <summary>
    ///     Video hash controller.
    /// </summary>
    [AuthorizeHttp(Roles = DomainRoles.User)]
    [ValidationHttp]
    [Route("projects/{id}/video/hash")]
    public class HashController : ProjectEntityControllerBase
    {
        public HashController(IProjectService projectService)
            : base(projectService)
        {
        }

        // POST api/projects/{id}/video/hash
        [CheckAccessHttp]
        [StatProjectState(StatActionType.Video)]
        public async Task<HttpResponseMessage> Post(string id, ProjectVideoHashModel model)
        {
            // Check whether project with such id exists
            await GetProjectAsync(id);

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, ResponseMessages.ResourceNotFound);
        }

        // PUT api/projects/{id}/video/hash
        public HttpResponseMessage Put(string id, ProjectVideoHashModel model)
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest);
        }
    }
}