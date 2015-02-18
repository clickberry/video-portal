// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Attributes.WebApi;
using Asp.Infrastructure.Extensions;
using Portal.Api.Infrastructure.Attributes;
using Portal.Api.Models;
using Portal.BLL.Services;
using Portal.Domain.PortalContext;
using Portal.Domain.ProjectContext;
using Portal.Domain.StatisticContext;
using Portal.DTO.Projects;
using Portal.Resources.Api;

namespace Portal.Api.Controllers.Projects
{
    [AuthorizeHttp(Roles = DomainRoles.User)]
    [ValidationHttp]
    [Route("projects/{id}/avsx")]
    public sealed class AvsxController : ProjectEntityControllerBase
    {
        private readonly IProjectAvsxService _projectAvsxService;

        public AvsxController(IProjectAvsxService projectAvsxService, IProjectService projectService)
            : base(projectService)
        {
            _projectAvsxService = projectAvsxService;
        }

        // POST api/projects/{id}/avsx
        [CheckAccessHttp]
        [ClearFilesHttp]
        [StatProjectState(StatActionType.Avsx)]
        public async Task<HttpResponseMessage> Post(string id, ProjectAvsxModel model)
        {
            // Get project entity
            await GetProjectAsync(id);

            // Add avsx
            var avsx = new DomainAvsx
            {
                ContentType = model.Avsx.ContentType,
                FileName = model.Avsx.Name,
                FileUri = model.Avsx.Uri,
                FileLength = model.Avsx.Length
            };

            avsx = await _projectAvsxService.AddAsync(id, avsx);

            var responseAvsx = new ProjectAvsx
            {
                Name = avsx.FileName,
                Uri = avsx.FileUri,
                Length = avsx.FileLength,
                ContentType = avsx.ContentType
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, responseAvsx);

            response.SetLastModifiedDate(avsx.Modified);

            return response;
        }

        // GET api/projects/{id}/avsx
        public async Task<HttpResponseMessage> Get(string id)
        {
            // Get project
            await GetProjectAsync(id);

            // Get avsx
            DomainAvsx avsx = await _projectAvsxService.GetAsync(id);
            if (avsx == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ResponseMessages.ResourceNotFound);
            }

            var responseAvsx = new ProjectAvsx
            {
                Name = avsx.FileName,
                Uri = avsx.FileUri,
                Length = avsx.FileLength,
                ContentType = avsx.ContentType
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, responseAvsx);
            response.SetLastModifiedDate(avsx.Modified);

            return response;
        }

        // PUT api/projects/{id}/avsx
        public HttpResponseMessage Put(string id, ProjectAvsxModel model)
        {
            return Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, ResponseMessages.MethodNotAllowed);
        }

        // DELETE api/projects/{id}/avsx
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Delete(string id)
        {
            // Get project
            await GetProjectAsync(id);

            // Delete avsx
            await _projectAvsxService.DeleteAsync(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}