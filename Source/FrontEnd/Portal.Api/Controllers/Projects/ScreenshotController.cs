// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
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
    [Route("projects/{id}/screenshot")]
    public sealed class ScreenshotController : ProjectEntityControllerBase
    {
        private readonly IProjectScreenshotService _projectScreenshotService;

        public ScreenshotController(IProjectService projectRepository, IProjectScreenshotService projectScreenshotService)
            : base(projectRepository)
        {
            _projectScreenshotService = projectScreenshotService;
        }

        // POST api/projects/{id}/screenshot
        [CheckAccessHttp]
        [ClearFilesHttp]
        [StatProjectState(StatActionType.Screenshot)]
        public async Task<HttpResponseMessage> Post(string id, ProjectScreenshotModel model)
        {
            await GetProjectAsync(id);

            var screenshot = new DomainScreenshot
            {
                FileName = model.Screenshot.Name,
                FileUri = model.Screenshot.Uri,
                FileId = Guid.NewGuid().ToString(),
                FileLength = model.Screenshot.Length,
                ContentType = model.Screenshot.ContentType,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };

            screenshot = await _projectScreenshotService.AddAsync(id, screenshot);

            var projectScreenshot = new ProjectScreenshot
            {
                ContentType = screenshot.ContentType,
                Name = screenshot.FileName,
                Uri = screenshot.FileUri,
                Length = screenshot.FileLength
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, projectScreenshot);
            response.SetLastModifiedDate(screenshot.Modified);

            return response;
        }

        // GET api/projects/{id}/screenshot
        public async Task<HttpResponseMessage> Get(string id)
        {
            await GetProjectAsync(id);

            DomainScreenshot screenshot = await _projectScreenshotService.GetAsync(id);

            var projectScreenshot = new ProjectScreenshot
            {
                Name = screenshot.FileName,
                Uri = screenshot.FileUri,
                Length = screenshot.FileLength,
                ContentType = screenshot.ContentType
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, projectScreenshot);
            response.SetLastModifiedDate(screenshot.Modified);

            return response;
        }

        // PUT api/projects/{id}/screenshot
        public HttpResponseMessage Put(string id, ProjectScreenshotModel model)
        {
            return Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, ResponseMessages.MethodNotAllowed);
        }

        // DELETE api/projects/{id}/screenshot
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Delete(string id)
        {
            await GetProjectAsync(id);

            await _projectScreenshotService.DeleteAsync(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}