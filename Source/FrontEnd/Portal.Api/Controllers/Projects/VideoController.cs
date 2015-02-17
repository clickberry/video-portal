// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Extensions;
using Portal.BLL.Services;
using Portal.Domain.PortalContext;
using Portal.Domain.ProjectContext;
using Portal.DTO.Projects;

namespace Portal.Api.Controllers.Projects
{
    [AuthorizeHttp(Roles = DomainRoles.User)]
    [ValidationHttp]
    [Route("projects/{id}/video")]
    public class VideoController : ProjectEntityControllerBase
    {
        private readonly IProjectVideoService _videoService;

        public VideoController(IProjectService projectService, IProjectVideoService videoService)
            : base(projectService)
        {
            _videoService = videoService;
        }

        // GET api/projects/{id}/video
        public async Task<HttpResponseMessage> Get(string id)
        {
            // Get project
            await GetProjectAsync(id);

            DomainVideo video = await _videoService.GetAsync(id);

            var projectVideo = new ProjectVideo
            {
                Name = video.FileName,
                Uri = video.FileUri,
                ContentType = video.ContentType,
                Length = video.FileLength
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, projectVideo);
            response.SetLastModifiedDate(video.Modified);

            return response;
        }

        // DELETE api/projects/{id}/video
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Delete(string id)
        {
            await GetProjectAsync(id);

            await _videoService.DeleteAsync(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}