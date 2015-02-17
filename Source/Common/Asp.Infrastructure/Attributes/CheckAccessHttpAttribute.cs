// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Configuration;
using Portal.Resources.Api;

namespace Asp.Infrastructure.Attributes
{
    /// <summary>
    ///     Checks whether persistance should be read only.
    /// </summary>
    public sealed class CheckAccessHttpAttribute : ActionFilterAttribute
    {
        [Import]
        public IPortalFrontendSettings Settings { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (Settings.StorageReadOnly)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ResponseMessages.ServiceUnavailable);
            }
        }
    }
}