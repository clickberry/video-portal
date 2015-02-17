// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Asp.Infrastructure.Attributes.Mvc;
using Configuration;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.Domain.MailerContext;
using Portal.Resources.Emails;
using Portal.Web.Constants;
using Portal.Web.Models;

namespace Portal.Web.Controllers
{
    /// <summary>
    ///     Handles general static pages
    /// </summary>
    public class HomeController : SpaControllerBase
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly IPortalFrontendSettings _settings;
        private readonly IUserAgentVerifier _userAgentVerifier;
        //
        // GET: /

        public HomeController(IPortalFrontendSettings settings, IUserAgentVerifier userAgentVerifier, IEmailSenderService emailSenderService)
            : base(settings)
        {
            _settings = settings;
            _userAgentVerifier = userAgentVerifier;
            _emailSenderService = emailSenderService;
        }

        //
        // GET: /

        [Route("profile", Name = RouteNames.Profile)]
        [Route("trends/{*url}", Name = RouteNames.Trends)]
        [Route("latest/{*url}", Name = RouteNames.Latest)]
        [Route("tag/{*url}", Name = RouteNames.Tag)]
        [Route("editorspick/{*url}", Name = RouteNames.EditorsPick)]
        [Route("user/{id}/videos/{projectId}", Name = RouteNames.UserVideo)]
        [Route("user/{id}/videos/{*url}", Name = RouteNames.UserVideos)]
        [Route("user/likes/{*url}", Name = RouteNames.UserLikes)]
        [Route("account/recovery/{*url}", Name = RouteNames.AccountRecovery)]
        [Route(Name = RouteNames.Home)]
        public ActionResult Index()
        {
            ViewBag.FrontPageBanners = _settings.FrontPageBanners;
            ViewBag.ContentBannerLeft = _settings.ContentBannerLeft;
            ViewBag.ContentBannerRight = _settings.ContentBannerRight;

            if (!_userAgentVerifier.IsMobileDevice(Request.UserAgent))
            {
                ViewBag.VideoViewBanner = _settings.VideoViewBanner;
            }

            return View();
        }

        // POST: /

        [Route]
        [HttpPost]
        [StatUserLoginMvc]
        public ActionResult Index(string username, string password)
        {
            // It's just a fake html page to use it in hidden iframe for login form to allow browsers save login/password correctly. 
            // Details https://medium.com/opinionated-angularjs/7bbf0346acec
            return new EmptyResult();
        }

        //
        // GET: /agreement

        [Route("agreement")]
        public ActionResult Agreement()
        {
            return RedirectToActionPermanent("Terms");
        }

        //
        // GET: /terms

        [Route("terms", Name = RouteNames.Terms)]
        public ActionResult Terms()
        {
            return View();
        }

        //
        // GET: /about

        [Route("about", Name = RouteNames.About)]
        public ActionResult About()
        {
            return View();
        }

        //
        // GET: /download/cic-pc

        [GoogleAnalytics]
        [Route("download/desktop", Name = RouteNames.DownloadCicPc)]
        [Route("download/clickberry-ic-pcedition.exe")]
        public ActionResult DownloadCicPc()
        {
            Device device = _userAgentVerifier.GetDevice(Request.UserAgent);
            switch (device.Platform)
            {
                case PlatformType.Macintosh:
                    return Redirect(_settings.DownloadLinks.CicMac);

                default:
                    return Redirect(_settings.DownloadLinks.CicPc);
            }
        }

        //
        // GET: /download/addon

        [GoogleAnalytics]
        [Route("download/addon", Name = RouteNames.DownloadAddon)]
        [Route("dwnldExt")]
        public ActionResult DownloadAddon()
        {
            Device device = _userAgentVerifier.GetDevice(Request.UserAgent);
            if (device.IsMobile)
            {
                return View("UnsupportedBrowser");
            }

            switch (device.Browser)
            {
                case BrowserType.Chrome:
                    return Redirect(_settings.DownloadLinks.AddonChrome);

                case BrowserType.Firefox:
                    return Redirect(_settings.DownloadLinks.AddonFirefox);

                case BrowserType.OperaNew:
                    return Redirect(_settings.DownloadLinks.AddonOpera);

                default:
                    return View("UnsupportedBrowser");
            }
        }

        //
        // GET: /contacts

        [Route("contacts", Name = RouteNames.Contacts)]
        public ActionResult Feedback()
        {
            return View();
        }

        //
        // POST: /contacts

        [Route("contacts")]
        [HttpPost]
        public async Task<ActionResult> PostFeedback(FeedbackModel model)
        {
            var email = new SendEmailDomain
            {
                Address = model.Email,
                UserId = UserId,
                Body = model.Message,
                Emails = new List<string> { _settings.EmailAddressSupport },
                Subject = Emails.SubjectFeedback
            };

            if (!ModelState.IsValid)
            {
                return View("Feedback");
            }

            try
            {
                await _emailSenderService.SendEmailAsync(email);
            }
            catch (Exception)
            {
                return View(new FeedbackResultModel { Status = FeedbackStatus.Error });
            }

            return View(new FeedbackResultModel { Status = FeedbackStatus.Success });
        }

        //
        // GET: /sdk

        [Route("sdk", Name = RouteNames.Sdk)]
        public ActionResult Sdk()
        {
            return View();
        }
    }
}