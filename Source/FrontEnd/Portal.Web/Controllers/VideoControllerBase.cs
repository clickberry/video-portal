// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Configuration;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.Domain.ProfileContext;
using Portal.Domain.ProjectContext;
using Portal.DTO.Watch;
using Portal.Exceptions.CRUD;
using Portal.Web.Constants;
using Portal.Web.Models;

namespace Portal.Web.Controllers
{
    public abstract class VideoControllerBase : SpaControllerBase
    {
        private const string TitleParam = "title";
        private const string DescriptionParam = "description";
        private const string ImageParam = "image";

        private readonly IProjectUriProvider _projectUriProvider;
        private readonly IProjectScreenshotService _screenshotService;

        protected VideoControllerBase(
            IProjectScreenshotService screenshotService,
            IProjectUriProvider projectUriProvider,
            IPortalFrontendSettings settings)
            : base(settings)
        {
            _screenshotService = screenshotService;
            _projectUriProvider = projectUriProvider;
        }

        /// <summary>
        ///     Gets a video viewmodel.
        /// </summary>
        /// <param name="watch">Watch project model.</param>
        /// <param name="id">Project identifier.</param>
        /// <returns></returns>
        protected async Task<VideoModel> GetVideoModel(Watch watch, string id)
        {
            VideoModel video = CreateVideoModel(id);
            video.Video = watch;
            video.Name = watch.Name;
            video.Description = watch.Description;

            OverrideParams(video);

            // If screenshot was not overriden
            if (string.IsNullOrEmpty(video.Image))
            {
                // Set default image
                video.Image = watch.Screenshots.Count == 0 ? string.Empty : watch.Screenshots[0].Uri;

                // Try to receive screenshot from the service
                if (String.IsNullOrEmpty(video.Image) || watch.Generator != (int)ProductType.TaggerIPhone)
                {
                    try
                    {
                        DomainScreenshot screenshot = await _screenshotService.GetAsync(id);

                        video.Image = screenshot.FileUri;
                    }
                    catch (NotFoundException)
                    {
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError("Failed to receive project {0} screenshot: {1}", id, e);
                    }
                }
            }

            SetImage(video);

            return video;
        }

        protected VideoModel GetVideoModel(string id)
        {
            VideoModel video = CreateVideoModel(id);
            OverrideParams(video);
            SetImage(video);

            return video;
        }

        protected bool ContainsOverrideParams()
        {
            return !String.IsNullOrEmpty(Request.Params[TitleParam]) &&
                   !String.IsNullOrEmpty(Request.Params[DescriptionParam]) &&
                   !String.IsNullOrEmpty(Request.Params[ImageParam]);
        }

        private VideoModel CreateVideoModel(string id)
        {
            string queryString = Request.QueryString.ToString();
            if (queryString.Length > 0)
            {
                queryString = string.Format(@"?{0}", queryString);
            }

            // HTTP addresses
            var video = new VideoModel
            {
                VideoUrl = _projectUriProvider.GetUri(id) + queryString,
                EmbedUrl = Url.RouteUrl(RouteNames.Embed, new { id, autoplay = 1 }, "http")
            };

            // HTTPS address
            var builder = new UriBuilder(video.VideoUrl) { Scheme = "https", Port = 443 };
            video.VideoSecureUrl = builder.Uri.ToString();

            builder = new UriBuilder(video.EmbedUrl) { Scheme = "https", Port = 443 };
            video.EmbedSecureUrl = builder.Uri.ToString();

            video.Robot = GetProvider(Request.UserAgent);

            return video;
        }

        private ProviderType GetProvider(string userAgent)
        {
            return userAgent.Contains("vkShare") ? ProviderType.Vk : ProviderType.Email;
        }

        private void OverrideParams(VideoModel video)
        {
            // Override title from query string
            if (!string.IsNullOrEmpty(Request.Params[TitleParam]))
            {
                video.Name = Request.Params[TitleParam];
            }

            // Override description from query string
            if (!string.IsNullOrEmpty(Request.Params[DescriptionParam]))
            {
                video.Description = Request.Params[DescriptionParam];
            }

            // Override image from query string
            if (!string.IsNullOrEmpty(Request.Params[ImageParam]))
            {
                Uri uri;

                if (Uri.TryCreate(Request.Params[ImageParam], UriKind.Absolute, out uri))
                {
                    video.Image = uri.AbsoluteUri;
                }
            }
        }

        private void SetImage(VideoModel video)
        {
            if (!string.IsNullOrEmpty(video.Image))
            {
                // HTTPS address
                video.ImageSecure = video.Image;

                // HTTP address
                var builder = new UriBuilder(video.Image) { Scheme = "http", Port = 80 };
                video.Image = builder.Uri.ToString();
            }
        }
    }
}