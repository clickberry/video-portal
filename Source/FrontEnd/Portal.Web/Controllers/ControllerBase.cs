// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using Configuration;

namespace Portal.Web.Controllers
{
    /// <summary>
    ///     Base class for a controllers.
    /// </summary>
    public abstract class ControllerBase : Controller
    {
        protected ControllerBase(IPortalFrontendSettings settings)
        {
            ViewBag.GoogleAnalyticsId = settings.GoogleAnalyticsId;
        }

        /// <summary>
        ///     Gets a current user identity.
        /// </summary>
        protected string UserId
        {
            get { return User.Identity.Name; }
        }

        /// <summary>
        ///     Gets a current application name.
        /// </summary>
        protected string AppName
        {
            get
            {
                if (Request.Url == null)
                {
                    throw new NullReferenceException("Request url is null.");
                }

                return Request.Url.Host.Replace("localhost", "127.0.0.1");
            }
        }
    }
}