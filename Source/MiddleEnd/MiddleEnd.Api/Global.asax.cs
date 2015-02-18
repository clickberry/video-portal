// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using MiddleEnd.Worker.Abstract;

namespace MiddleEnd.Api
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class WebApiApplication : HttpApplication
    {
        private ITaskStateManager _taskStateManager;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Configure);

            // Setup outbound connections settings
            // http://blogs.msdn.com/b/windowsazurestorage/archive/2010/11/06/how-to-get-most-out-of-windows-azure-tables.aspx#_Toc276237444
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;

            // Start scheduled tasks updater
            _taskStateManager = GetService<ITaskStateManager>();
            _taskStateManager.Start();
        }

        protected void Application_End(Object sender, EventArgs e)
        {
            // Stop scheduled tasks updater
            _taskStateManager.Stop();
        }

        /// <summary>
        ///     Get type instance by using dependency resolver.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>Instance.</returns>
        private T GetService<T>()
        {
            IDependencyResolver dependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;
            return (T)dependencyResolver.GetService(typeof (T));
        }
    }
}