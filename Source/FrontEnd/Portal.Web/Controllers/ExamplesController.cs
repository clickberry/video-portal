// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Web.Mvc;
using Asp.Infrastructure.Attributes;
using Configuration;
using Portal.Domain.PortalContext;
using Portal.Web.Constants;

namespace Portal.Web.Controllers
{
    [AuthorizeMvc(Roles = DomainRoles.ExamplesManager)]
    public class ExamplesController : SpaControllerBase
    {
        public ExamplesController(IPortalFrontendSettings settings) : base(settings)
        {
        }

        //
        // GET: /examples

        [Route("examples", Name = RouteNames.Examples)]
        public ActionResult Index()
        {
            return View("ExamplesSPA");
        }
    }
}