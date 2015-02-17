// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Attributes.WebApi;
using Asp.Infrastructure.Extensions;
using Portal.Api.Models;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.Domain.PortalContext;
using Portal.Domain.ProjectContext;
using Portal.DTO.Projects;
using Portal.Resources.Api;

namespace Portal.Api.Controllers.Projects
{
    /// <summary>
    ///     Handles user projects.
    /// </summary>
    [AuthorizeHttp(Roles = DomainRoles.User)]
    [ValidationHttp]
    [RoutePrefix("projects")]
    public sealed class ProjectsController : ApiControllerBase
    {
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IExternalVideoService _externalVideoService;
        private readonly IProductIdExtractor _productIdExtractor;
        private readonly IProjectAvsxService _projectAvsxService;
        private readonly IProjectScreenshotService _projectScreenshotService;
        private readonly IProjectService _projectService;
        private readonly IProjectUriProvider _projectUriProvider;

        public ProjectsController(
            IProjectService projectService,
            IExternalVideoService externalVideoService,
            IProjectAvsxService projectAvsxService,
            IProjectScreenshotService projectScreenshotService,
            IEmailNotificationService emailNotificationService,
            IProjectUriProvider projectUriProvider,
            IProductIdExtractor productIdExtractor)
        {
            _projectService = projectService;
            _externalVideoService = externalVideoService;
            _projectAvsxService = projectAvsxService;
            _projectScreenshotService = projectScreenshotService;
            _emailNotificationService = emailNotificationService;
            _projectUriProvider = projectUriProvider;
            _productIdExtractor = productIdExtractor;
        }

        //
        // POST /api/projects

        /// <summary>
        ///     Creates a new project.
        /// </summary>
        /// <param name="model">Project model.</param>
        /// <returns>Project identifier.</returns>
        [CheckAccessHttp]
        [StatProjectUploadingWebApi]
        [StatProjectCreating]
        [Route]
        public async Task<HttpResponseMessage> Post(ProjectModel model)
        {
            try
            {
                await CongratUserIfNeeded();
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to notify user by email: {0}", e);
            }

            ProductType product = _productIdExtractor.Get(UserAgent);

            var project = new DomainProject
            {
                UserId = UserId,
                Name = model.Name,
                Description = model.Description,
                Access = model.Access,
                ProductType = product,
                ProjectType = model.ProjectType,
                ProjectSubtype = model.ProjectSubtype,
                EnableComments = model.EnableComments
            };

            project = await _projectService.AddAsync(project);

            var responseProject = new Project
            {
                Id = project.Id,
                Description = project.Description,
                Access = project.Access,
                Name = project.Name,
                Created = project.Created,
                PublicUrl = _projectUriProvider.GetUri(project.Id),
                ProjectType = project.ProjectType,
                ProjectSubtype = project.ProjectSubtype,
                EnableComments = project.EnableComments
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, responseProject);
            response.SetLastModifiedDate(project.Modified);

            return response;
        }

        //
        // POST /api/projects/external

        /// <summary>
        ///     Creates a new external project.
        /// </summary>
        /// <param name="model">Project model.</param>
        /// <returns>Project identifier.</returns>
        [CheckAccessHttp]
        [StatProjectUploadingWebApi]
        [StatProjectExternal]
        [Route("external")]
        public async Task<HttpResponseMessage> Post(ExternalProjectModel model)
        {
            try
            {
                await CongratUserIfNeeded();
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to notify user by email: {0}", e);
            }

            // Add project
            ProductType product = _productIdExtractor.Get(UserAgent);

            var project = new DomainProject
            {
                UserId = UserId,
                Name = model.Name,
                Description = model.Description,
                Access = model.Access,
                ProductType = product,
                ProjectType = model.ProjectType,
                ProjectSubtype = model.ProjectSubtype,
                EnableComments = model.EnableComments
            };

            project = await _projectService.AddAsync(project);

            // Add external video
            await _externalVideoService.AddAsync(project.Id, new DomainExternalVideo
            {
                ProductName = model.ProductName,
                VideoUri = model.VideoUri
            });

            // Add avsx file if exists
            if (model.Avsx != null)
            {
                var avsx = new DomainAvsx
                {
                    ContentType = model.Avsx.ContentType,
                    FileName = model.Avsx.Name,
                    FileUri = model.Avsx.Uri,
                    FileLength = model.Avsx.Length
                };

                await _projectAvsxService.AddAsync(project.Id, avsx);
                // For statistics
                Request.Properties.Add("isAvsx", true);
            }

            // Add screenshot file if exists
            if (model.Screenshot != null)
            {
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

                await _projectScreenshotService.AddAsync(project.Id, screenshot);
                // For statistics
                Request.Properties.Add("isScreenshot", true);
            }

            var responseProject = new Project
            {
                Id = project.Id,
                Description = project.Description,
                Access = project.Access,
                Name = project.Name,
                Created = project.Created,
                PublicUrl = _projectUriProvider.GetUri(project.Id),
                ProjectType = project.ProjectType,
                ProjectSubtype = project.ProjectSubtype,
                EnableComments = project.EnableComments
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, responseProject);
            response.SetLastModifiedDate(project.Modified);

            return response;
        }

        private async Task CongratUserIfNeeded()
        {
            await _emailNotificationService.SendFirstProjectEmail(UserId, UserAgent);
        }

        //
        // GET /api/projects

        /// <summary>
        ///     Returns a projects collection.
        /// </summary>
        /// <returns>Collection of projects.</returns>
        [Queryable]
        [Route]
        public async Task<IQueryable<Project>> Get()
        {
            IEnumerable<DomainProject> projects = await _projectService.GetListAsync(new DomainProject { UserId = UserId });

            return projects.Select(p => new Project
            {
                Id = p.Id,
                Created = p.Created,
                Description = p.Description,
                Name = p.Name,
                Access = p.Access,
                PublicUrl = _projectUriProvider.GetUri(p.Id),
                ProjectType = p.ProjectType,
                ProjectSubtype = p.ProjectSubtype,
                EnableComments = p.EnableComments
            }).AsQueryable();
        }

        //
        // GET /api/projects/{id}

        /// <summary>
        ///     Returns a project by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Project information.</returns>
        [Route("{id}")]
        public async Task<HttpResponseMessage> Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ResponseMessages.ProjectInvalidId);
            }

            var project = new DomainProject
            {
                UserId = UserId,
                Id = id
            };

            project = await _projectService.GetAsync(project);

            var responseProject = new Project
            {
                Id = project.Id,
                Description = project.Description,
                Name = project.Name,
                Access = project.Access,
                Created = project.Created,
                PublicUrl = _projectUriProvider.GetUri(project.Id),
                ProjectType = project.ProjectType,
                ProjectSubtype = project.ProjectSubtype,
                EnableComments = project.EnableComments
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, responseProject);
            response.SetLastModifiedDate(project.Modified);

            return response;
        }

        //
        // PUT /api/projects/{id}

        /// <summary>
        ///     Updates a project.
        /// </summary>
        /// <param name="id">Project identifier.</param>
        /// <param name="model">Project model.</param>
        /// <returns>Operation status code.</returns>
        [CheckAccessHttp]
        [Route("{id}")]
        public async Task<HttpResponseMessage> Put(string id, ProjectModel model)
        {
            var project = new DomainProject { Id = id, UserId = UserId };
            DomainProject originalProject = await _projectService.GetAsync(project);

            // updating original project
            originalProject.Name = model.Name;
            originalProject.Description = model.Description;
            originalProject.Access = model.Access;
            originalProject.EnableComments = model.EnableComments;
            project = await _projectService.EditAsync(originalProject);

            var responseProjects = new Project
            {
                Created = project.Created,
                Description = project.Description,
                Id = project.Id,
                Access = project.Access,
                Name = project.Name,
                PublicUrl = _projectUriProvider.GetUri(project.Id),
                ProjectType = project.ProjectType,
                ProjectSubtype = project.ProjectSubtype,
                EnableComments = project.EnableComments
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, responseProjects);
            response.SetLastModifiedDate(project.Modified);

            return response;
        }

        [Route]
        public HttpResponseMessage Put()
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest);
        }

        //
        // DELETE /api/projects/{id}

        /// <summary>
        ///     Deletes a project by identifier.
        /// </summary>
        /// <param name="id">Project identifier.</param>
        /// <returns>Operation status code.</returns>
        [CheckAccessHttp]
        [StatProjectDeletionWebApi]
        [Route("{id}")]
        public async Task<HttpResponseMessage> Delete(string id)
        {
            var projectQuery = new DomainProject
            {
                UserId = UserId,
                Id = id
            };

            await _projectService.DeleteAsync(projectQuery);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        ///     Deletes all projects.
        /// </summary>
        /// <returns>Operation status code.</returns>
        [Route]
        public HttpResponseMessage Delete()
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest);
        }
    }
}