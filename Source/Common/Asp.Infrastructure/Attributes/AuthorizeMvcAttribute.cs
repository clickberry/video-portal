// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asp.Infrastructure.Attributes
{
    public sealed class AuthorizeMvcAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            else
            {
                filterContext.Result = new RedirectResult(string.Format("~/?returnUrl={0}", HttpUtility.UrlEncode(request.Url.PathAndQuery)));
            }
        }
    }
}