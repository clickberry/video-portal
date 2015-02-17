// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Newtonsoft.Json;
using Portal.DTO.Watch;

namespace Portal.Web
{
    public static class VideoHelpers
    {
        /// <summary>
        ///     Gets a videos array.
        /// </summary>
        /// <param name="helper">Html helper.</param>
        /// <param name="videos">Videos.</param>
        /// <returns>Array.</returns>
        public static MvcHtmlString GetVideosArray(this HtmlHelper helper, List<WatchVideo> videos)
        {
            var sb = new StringBuilder("{");

            var groupedVideos = videos.OrderBy(p => p.Width).GroupBy(p => p.Width).Select(p => new
            {
                FrameWidth = p.Key,
                FrameHeight = p.First().Height,
                Items = p.ToList()
            }).ToList();

            for (int i = 0; i < groupedVideos.Count; i++)
            {
                var @group = groupedVideos[i];

                sb.AppendFormat(
                    @"""{0}"": {{width: {1}, height: {2}, src: {3} }}{4}",
                    @group.FrameWidth + "p",
                    @group.FrameWidth, @group.FrameHeight,
                    GetResolutions(@group.Items),
                    i != groupedVideos.Count - 1 ? ", " : string.Empty);
            }

            sb.Append("}");

            return new MvcHtmlString(sb.ToString());
        }

        private static string GetResolutions(List<WatchVideo> videos)
        {
            var sb = new StringBuilder("{");

            for (int i = 0; i < videos.Count; i++)
            {
                sb.AppendFormat(@"""{0}"": ""{1}""{2}", videos[i].ContentType, videos[i].Uri, i != videos.Count - 1 ? ", " : string.Empty);
            }

            sb.Append("}");

            return sb.ToString();
        }

        /// <summary>
        ///     Gets a screenshot uri.
        /// </summary>
        /// <param name="screenshots">Screenshots.</param>
        /// <returns>File uri.</returns>
        public static string GetScreenshot(this List<WatchScreenshot> screenshots)
        {
            return screenshots.Count == 0 ? string.Empty : screenshots[0].Uri;
        }

        /// <summary>
        ///     Gets a video size.
        /// </summary>
        /// <param name="video">Video.</param>
        /// <param name="width">Video width.</param>
        /// <returns>Video size.</returns>
        public static Size GetVideoSize(this Watch video, int width = 500)
        {
            // Extension
            if (video.External != null)
            {
                return new Size(width, (int)Math.Ceiling(width*0.63));
            }

            // Regular
            int height = video.Videos.Count != 0
                ? (int)Math.Ceiling((double)width*video.Videos[0].Height/video.Videos[0].Width)
                : 300;

            return new Size(width, height);
        }

        private static bool IsMatch(HtmlHelper htmlHelper, string pattern)
        {
            return new Regex(pattern, RegexOptions.IgnoreCase).IsMatch(htmlHelper.ViewContext.HttpContext.Request.UserAgent ?? string.Empty);
        }

        /// <summary>
        ///     Checks whether user agent is relate to ipad.
        /// </summary>
        /// <param name="htmlHelper">Html helper.</param>
        /// <returns>Result.</returns>
        public static bool IsIpad(this HtmlHelper htmlHelper)
        {
            const string pattern = @"ipad";
            return IsMatch(htmlHelper, pattern);
        }


        /// <summary>
        ///     Gets an external video data.
        /// </summary>
        /// <param name="video">Html helper.</param>
        /// <param name="jwFlashPlayerRoot">JwPlayer Flash player root.</param>
        /// <param name="youtubeHtml5PlayerRoot">Youtube Html5 player root.</param>
        public static MvcHtmlString GetExternalVideo(this Watch video, string jwFlashPlayerRoot, string youtubeHtml5PlayerRoot)
        {
            var response = video.External == null
                ? null
                : new
                {
                    productName = video.External.ProductName,
                    videoUrl = video.External.VideoUri,
                    screenshotUrl = video.ScreenshotUrl,
                    acsNamespace = video.External.AcsNamespace,
                    flashPlayerUrl = "/extension",
                    jwPlayerRoot = jwFlashPlayerRoot,
                    youtubeHtml5PlayerRoot
                };

            return new MvcHtmlString(JsonConvert.SerializeObject(response));
        }
    }
}