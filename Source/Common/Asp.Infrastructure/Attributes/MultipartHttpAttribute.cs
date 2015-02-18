// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Asp.Infrastructure.Attributes
{
    /// <summary>
    ///     Request must contains multipart/form-data body.
    /// </summary>
    public sealed class MultipartHttpAttribute : ActionFilterAttribute
    {
        public MultipartHttpAttribute()
        {
            InvalidMultipartBodyText = "Invalid request body. Required multipart/form-data.";
        }

        public string InvalidMultipartBodyText { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.Request.Content.IsMimeMultipartContent())
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(InvalidMultipartBodyText));
            }
        }
    }
}