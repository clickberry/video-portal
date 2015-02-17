// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Portal.Web.App_Start;

namespace Portal.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        private void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;
            AreaRegistration.RegisterAllAreas();

            FederationMetadataConfig.Intialize();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ViewEngineConfig.Configure();
            EventAggregatorConfig.Configure(DependencyResolver.Current);

            // Setup outbound connections settings
            // http://blogs.msdn.com/b/windowsazurestorage/archive/2010/11/06/how-to-get-most-out-of-windows-azure-tables.aspx#_Toc276237444
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;
        }

        private void Application_Error(object sender, EventArgs e)
        {
            // Get last error
            Exception exception = Server.GetLastError();

            var httpException = exception as HttpException;
            if (httpException != null)
            {
                // http exception or IIS exceptions, including 404

                int statusCode = httpException.GetHttpCode();
                if (statusCode >= (int)HttpStatusCode.InternalServerError)
                {
                    // logging all server exceptions
                    Trace.TraceError("Request processing has failed: {0}", exception);
                }
            }
            else
            {
                // other exceptions

                // logging
                Trace.TraceError("Request processing has failed: {0}", exception);
            }

            Server.ClearError();
        }
    }
}