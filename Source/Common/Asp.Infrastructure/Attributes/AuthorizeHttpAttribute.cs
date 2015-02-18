// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;
using Portal.Resources.Api;

namespace Asp.Infrastructure.Attributes
{
    /// <summary>
    ///     Handles authentication and authorization in Web API.
    /// </summary>
    public sealed class AuthorizeHttpAttribute : AuthorizeAttribute
    {
        public bool IsAuthenticationRequired { get; set; }

        public AuthorizeHttpAttribute()
        {
            IsAuthenticationRequired = true;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            IPrincipal user = actionContext.ControllerContext.RequestContext.Principal;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                if (IsAuthenticationRequired)
                {
                    actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, ResponseMessages.UnathorizedRequest);
                }
            }
            else
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, ResponseMessages.Forbidden);
            }
        }
    }
}