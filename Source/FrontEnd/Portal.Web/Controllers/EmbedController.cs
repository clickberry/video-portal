// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Asp.Infrastructure.Attributes.Mvc;
using Configuration;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.Domain.PortalContext;
using Portal.DTO.Watch;
using Portal.Exceptions.CRUD;
using Portal.Web.Constants;

namespace Portal.Web.Controllers
{
    public class EmbedController : VideoControllerBase
    {
        private readonly IWatchProjectService _watchProjectRepository;

        public EmbedController(
            IWatchProjectService watchProjectRepository,
            IProjectScreenshotService screenshotService,
            IProjectUriProvider projectUriProvider,
            IPortalFrontendSettings settings)
            : base(screenshotService, projectUriProvider, settings)
        {
            _watchProjectRepository = watchProjectRepository;
        }

        //
        // GET: /embed

        [Route("embed/{id?}", Name = RouteNames.Embed)]
        [StatWatchingMvc]
        public async Task<ActionResult> Index(string id)
        {
            Watch project;

            try
            {
                // TODO: Hack! we should create separate SPA fro admin.
                project = await _watchProjectRepository.GetByIdAsync(id, UserId);

                // For statistics
                HttpContext.Items.Add("isReady", project.State == WatchState.Ready);
            }
            catch (ForbiddenException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            catch (UnauthorizedException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            catch (NotFoundException)
            {
                return HttpNotFound();
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to retrieve project {0}: {1}", id, e);
                return HttpNotFound();
            }

            return (ActionResult)View("Index", await GetVideoModel(project, id));
        }
    }
}