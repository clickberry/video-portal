// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using Configuration;
using Portal.Web.Constants;

namespace Portal.Web.Controllers
{
    [RoutePrefix("clients")]
    public class ClientsController : SpaControllerBase
    {
        private readonly IPortalFrontendSettings _settings;

        public ClientsController(IPortalFrontendSettings settings)
            : base(settings)
        {
            _settings = settings;
        }


        //
        // GET: /clients

        [Route("subscriptions/{*url}", Name = RouteNames.ClientSubscriptions)]
        [Route("profile", Name = RouteNames.ClientProfile)]
        [Route("signup/{*url}", Name = RouteNames.ClientSignup)]
        [Route("{*url}", Name = RouteNames.Client)]
        public ActionResult Index()
        {
            var linkTrackerUri = new Uri(_settings.LinkTrackerUri);

            ViewBag.LinkTrackerDomain = linkTrackerUri.Host;
            ViewBag.JwFlashPlayerUrl = _settings.JwFlashPlayerUrl;
            ViewBag.StripePublicKey = _settings.StripePublicKey;

            return View();
        }


        //
        // GET: /clients/integration

        [Route("integration", Name = RouteNames.ClientIntegration)]
        public ActionResult Integration()
        {
            return View();
        }
    }
}