// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Web.Mvc;
using System.Web.Routing;

namespace Portal.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapMvcAttributeRoutes();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Errors", // Route name
                "error/{action}", // URL with parameters
                new { controller = "Error", action = "Index" } // Parameter defaults
                );
        }
    }
}