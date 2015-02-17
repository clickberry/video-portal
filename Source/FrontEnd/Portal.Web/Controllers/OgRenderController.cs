// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using Portal.BLL.Infrastructure;
using Portal.Web.Constants;

namespace Portal.Web.Controllers
{
    public class OgRenderController : Controller
    {
        private readonly IUserAgentVerifier _userAgentVerifier;

        public OgRenderController(IUserAgentVerifier userAgentVerifier)
        {
            _userAgentVerifier = userAgentVerifier;
        }

        [Route("og", Name = RouteNames.OgRender)]
        public ActionResult Index()
        {
            NameValueCollection qs = Request.QueryString;

            if (_userAgentVerifier.IsSocialBot(Request.UserAgent))
            {
                if (qs.AllKeys.Contains("type"))
                {
                    ViewBag.Type = qs.Get("type");
                }

                if (qs.AllKeys.Contains("image"))
                {
                    ViewBag.Image = qs.Get("image");
                }

                if (qs.AllKeys.Contains("title"))
                {
                    ViewBag.Title = qs.Get("title");
                }

                if (qs.AllKeys.Contains("description"))
                {
                    ViewBag.Description = qs.Get("description");
                }

                return View();
            }

            if (qs.AllKeys.Contains("url"))
            {
                Uri uri;
                if (Uri.TryCreate(qs["url"], UriKind.Absolute, out uri))
                {
                    return Redirect(uri.AbsoluteUri);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}