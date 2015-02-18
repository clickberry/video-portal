// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Web.Routing;
using Configuration;

namespace Portal.Web.Controllers
{
    public abstract class SpaControllerBase : ControllerBase
    {
        private readonly IPortalFrontendSettings _settings;

        protected SpaControllerBase(IPortalFrontendSettings settings)
            : base(settings)
        {
            _settings = settings;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var playerUrl = new UriBuilder(new Uri(_settings.PlayerUrl));
            var jwFlashPlayerUrl = new UriBuilder(new Uri(_settings.JwFlashPlayerUrl));
            var youtubeHtml5PlayerUrl = new UriBuilder(new Uri(_settings.YoutubeHtml5PlayerUrl));

            if (Request.Url != null)
            {
                playerUrl.Scheme = Request.Url.Scheme;
                jwFlashPlayerUrl.Scheme = Request.Url.Scheme;
                youtubeHtml5PlayerUrl.Scheme = Request.Url.Scheme;
            }

            ViewBag.PlayerUrl = playerUrl.ToString();
            ViewBag.JwFlashPlayerUrl = jwFlashPlayerUrl.ToString();
            ViewBag.YoutubeHtml5PlayerUrl = youtubeHtml5PlayerUrl.ToString();
            ViewBag.JiraIssueCollector = _settings.JiraIssueCollectorUri;
        }
    }
}