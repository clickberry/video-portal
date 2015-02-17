// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IdentityModel.Services;
using System.Security.Claims;
using System.Web.Mvc;
using Configuration;

namespace Portal.Web.Controllers
{
    [RoutePrefix("extension")]
    public class ExtensionController : ControllerBase
    {
        private readonly IPortalFrontendSettings _settings;

        public ExtensionController(IPortalFrontendSettings settings) : base(settings)
        {
            _settings = settings;

            // Set current player version
            ViewBag.ExtensionUri = _settings.ExtensionUri;
            ViewBag.ExtensionUri = _settings.ExtensionUri;
        }

        //
        // GET: /extension/
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /extension/youtube
        [Route("youtube")]
        public ActionResult Youtube()
        {
            ViewBag.ExtensionUri = _settings.YoutubePlayerUrl;
            return View("Integration");
        }

        //
        // GET: /extension/youtube-html
        [Route("youtube-html")]
        public ActionResult YoutubeHtml()
        {
            ViewBag.YoutubeHtml5PlayerUrl = _settings.YoutubeHtml5PlayerUrl;
            return View("YoutubeHtml");
        }

        //
        // GET: /extension/dailymotion
        [Route("dailymotion")]
        public ActionResult Dailymotion()
        {
            ViewBag.ExtensionUri = _settings.DailymotionPlayerUrl;
            return View("Integration");
        }

        //
        // GET: /extension/jwplayer
        [Route("jwplayer")]
        public ActionResult Jwplayer()
        {
            ViewBag.ExtensionUri = _settings.JwFlashPlayerUrl;
            return View("Integration");
        }

        //
        // GET: /extension/login
        [HttpGet]
        [Route("login")]
        public ActionResult Login()
        {
            return RedirectToAction("Index");
        }

        //
        // POST: /extension/login
        [HttpPost, ValidateInput(false)]
        [Route("login")]
        public ActionResult Login(FormCollection collection)
        {
            var claimsPrincipal = User.Identity as ClaimsIdentity;
            if (claimsPrincipal == null || !claimsPrincipal.IsAuthenticated)
            {
                return View("_IpAccessDeclined");
            }

            FederatedAuthentication.WSFederationAuthenticationModule.SignOut(true);
            ViewBag.Token = collection["wresult"];

            return View("Login");
        }

        [Route("permissions")]
        public ActionResult Permissions()
        {
            return View();
        }
    }
}