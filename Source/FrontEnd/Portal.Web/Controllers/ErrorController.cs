// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Web.Mvc;

namespace Portal.Web.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            Response.StatusCode = 500;
            return View("500");
        }

        public ActionResult Http400()
        {
            Response.StatusCode = 400;
            return View("400");
        }

        public ActionResult Http403()
        {
            Response.StatusCode = 403;
            return View("403");
        }

        public ActionResult Http404()
        {
            Response.StatusCode = 404;
            return View("404");
        }
    }
}