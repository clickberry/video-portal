// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Web.Mvc;
using Asp.Infrastructure.Attributes;
using Portal.Domain.PortalContext;
using Portal.Web.Constants;

namespace Portal.Web.Controllers
{
    [AuthorizeMvc(Roles = DomainRoles.AllAdministrators)]
    [ValidationHttp]
    [RoutePrefix("admin")]
    public class AdminController : Controller
    {
        // GET: /admin

        [Route("videos/{*url}", Name = RouteNames.AdminVideos)]
        [Route("users/{*url}", Name = RouteNames.AdminUsers)]
        [Route("clients/{*url}", Name = RouteNames.AdminClients)]
        [Route("{*url}", Name = RouteNames.Admin)]
        public ActionResult Index()
        {
            return View();
        }
    }
}