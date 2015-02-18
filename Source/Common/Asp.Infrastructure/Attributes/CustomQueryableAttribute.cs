// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using Portal.Resources.Api;

namespace Asp.Infrastructure.Attributes
{
    public class CustomQueryableAttribute : QueryableAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            if (actionExecutedContext.Response.StatusCode == HttpStatusCode.BadRequest)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ResponseMessages.BadRequest)
                };
            }
        }
    }
}