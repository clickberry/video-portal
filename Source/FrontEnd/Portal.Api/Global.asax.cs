// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Net;
using System.Web;
using System.Web.Http;

namespace Portal.Api
{
    /// <summary>
    ///     Web Api application.
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        /// <summary>
        ///     Application start.
        /// </summary>
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Configure);
            TransferModeConfig.Configure(GlobalConfiguration.Configuration);
            EventAggregatorConfig.Configure(GlobalConfiguration.Configuration.DependencyResolver);

            // Setup outbound connections settings
            // http://blogs.msdn.com/b/windowsazurestorage/archive/2010/11/06/how-to-get-most-out-of-windows-azure-tables.aspx#_Toc276237444
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;
        }
    }
}