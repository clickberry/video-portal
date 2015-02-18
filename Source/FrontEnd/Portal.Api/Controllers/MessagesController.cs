// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Web.Http;
using Portal.Resources.Api;

namespace Portal.Api.Controllers
{
    public sealed class MessagesController : ApiController
    {
        [AcceptVerbs("GET", "PUT", "POST", "DELETE", "HEAD")]
        public HttpResponseMessage NotFound()
        {
            return Request.CreateResponse(HttpStatusCode.NotFound, ResponseMessages.ResourceNotFound);
        }
    }
}