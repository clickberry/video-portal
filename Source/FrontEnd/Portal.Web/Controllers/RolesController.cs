// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Web.Mvc;
using Asp.Infrastructure.Attributes;
using Portal.Domain.PortalContext;
using Portal.Web.Constants;

namespace Portal.Web.Controllers
{
    [ValidationHttp]
    [AuthorizeMvc(Roles = DomainRoles.SuperAdministrator)]
    public class RolesController : Controller
    {
        //
        // GET: /roles

        [Route("roles", Name = RouteNames.Roles)]
        public ActionResult RolesSpa()
        {
            return View();
        }
    }
}