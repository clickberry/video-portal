// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;
using Asp.Infrastructure.Attributes.Mvc;
using Configuration;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.Domain.PortalContext;
using Portal.Domain.ProfileContext;
using Portal.DTO.Watch;
using Portal.Exceptions.CRUD;
using Portal.Web.Constants;
using Portal.Web.Models;

namespace Portal.Web.Controllers
{
    public class VideoController : VideoControllerBase
    {
        private readonly IProjectLikesService _projectLikesService;
        private readonly IPortalFrontendSettings _settings;
        private readonly IUserAgentVerifier _userAgentVerifier;
        private readonly IUserAvatarProvider _userAvatarProvider;
        private readonly IUserService _userService;
        private readonly IWatchProjectService _watchProjectRepository;

        public VideoController(IWatchProjectService watchProjectRepository,
            IProjectScreenshotService screenshotService,
            IProjectUriProvider projectUriProvider,
            IPortalFrontendSettings settings,
            IUserAgentVerifier userAgentVerifier,
            IProjectLikesService projectLikesService,
            IUserService userService,
            IUserAvatarProvider userAvatarProvider)
            : base(screenshotService, projectUriProvider, settings)
        {
            _watchProjectRepository = watchProjectRepository;
            _userAgentVerifier = userAgentVerifier;
            _projectLikesService = projectLikesService;
            _settings = settings;
            _userService = userService;
            _userAvatarProvider = userAvatarProvider;
        }

        //
        // GET: /video/
        [Route("video")]
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToRoute(RouteNames.UserVideos, new { id = UserId });
        }

        //
        // GET: /video/{id}

        [Route("video/{id}", Name = RouteNames.Video)]
        [StatWatchingMvc]
        public async Task<ActionResult> Index(string id)
        {
            // Social bots
            if (_userAgentVerifier.IsSocialBot(Request.UserAgent) && ContainsOverrideParams())
            {
                VideoModel videoModel = GetVideoModel(id);
                return View("OgTags", videoModel);
            }

            // Get video
            Watch project;
            try
            {
                project = await _watchProjectRepository.GetByIdAsync(id, UserId);

                // Post-processing
                if (User.IsInRole(DomainRoles.User))
                {
                    project.IsLiked = await _projectLikesService.IsLikedAsync(id, UserId);
                    project.IsDisliked = await _projectLikesService.IsDislikedAsync(id, UserId);
                }

                // For statistics
                HttpContext.Items.Add("isReady", project.State == WatchState.Ready);
            }
            catch (ForbiddenException)
            {
                // Go home if forbidden
                return RedirectToAction("Index", "Home");
            }
            catch (NotFoundException)
            {
                return HttpNotFound();
            }
            catch (UnauthorizedException)
            {
                // Go home if unauthorized
                // In case user is unauthorized we should show him 401-error-page. 
                // But there isn't one so for now it would be redirect on Home page.
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to retrieve project {0}: {1}", id, e);
                return HttpNotFound();
            }

            VideoModel model = await GetVideoModel(project, id);

            // Get current user avatar url
            DomainUser user = null;
            try
            {
                user = await _userService.GetAsync(UserId);
            }
            catch (NotFoundException)
            {
            }

            model.UserAvatarUrl = _userAvatarProvider.GetAvatar(user);

            // Show banners for non-mobile devices
            if (!_userAgentVerifier.IsMobileDevice(Request.UserAgent))
            {
                ViewBag.VideoViewBanner = _settings.VideoViewBanner;
            }

            return View("Index", model);
        }
    }
}