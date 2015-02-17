// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Attributes.WebApi;
using Configuration;
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
    [Route("projects/{id}/external")]
    public sealed class ExternalController : ProjectEntityControllerBase
    {
        private readonly IExternalVideoService _externalVideoService;
        private readonly IPortalFrontendSettings _settings;

        public ExternalController(
            IExternalVideoService externalVideoService,
            IProjectService projectService,
            IPortalFrontendSettings settings)
            : base(projectService)
        {
            _externalVideoService = externalVideoService;
            _settings = settings;
        }

        // POST api/projects/{id}/external
        [CheckAccessHttp]
        [StatProjectState(StatActionType.Video)]
        public async Task<HttpResponseMessage> Post(string id, ExternalVideoModel model)
        {
            // Get project entity
            await GetProjectAsync(id);

            DomainExternalVideo externalVideo = await _externalVideoService.AddAsync(id, model);

            var result = new ExternalVideo
            {
                ProductName = externalVideo.ProductName,
                VideoUri = externalVideo.VideoUri,
                AcsNamespace = _settings.AcsNamespace
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, result);
            return response;
        }

        // GET api/projects/{id}/external
        public async Task<HttpResponseMessage> Get(string id)
        {
            // Get project
            await GetProjectAsync(id);

            // Get
            DomainExternalVideo externalVideo = await _externalVideoService.GetAsync(id);
            if (externalVideo == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ResponseMessages.ResourceNotFound);
            }

            var result = new ExternalVideo
            {
                ProductName = externalVideo.ProductName,
                VideoUri = externalVideo.VideoUri,
                AcsNamespace = _settings.AcsNamespace
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        // PUT api/projects/{id}/external
        public HttpResponseMessage Put(string id, ExternalVideoModel model)
        {
            return Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, ResponseMessages.MethodNotAllowed);
        }

        // DELETE api/projects/{id}/external
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Delete(string id)
        {
            // Get project
            await GetProjectAsync(id);

            // Delete
            await _externalVideoService.DeleteAsync(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}