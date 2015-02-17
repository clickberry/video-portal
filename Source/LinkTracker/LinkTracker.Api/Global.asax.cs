// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace LinkTracker.Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            JsonConfig.Configure(GlobalConfiguration.Configuration);

            // Setup outbound connections settings
            // http://blogs.msdn.com/b/windowsazurestorage/archive/2010/11/06/how-to-get-most-out-of-windows-azure-tables.aspx#_Toc276237444
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            //
            // TPL ThreadPool Configuration
            //
            ThreadPool.SetMinThreads(1000, 1000);
            ThreadPool.SetMaxThreads(5000, 5000);
        }
    }
}