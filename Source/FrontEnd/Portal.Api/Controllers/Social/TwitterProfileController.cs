// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Portal.Api.Models;
using Portal.BLL.Services;

namespace Portal.Api.Controllers.Social
{
    [ValidationHttp]
    [Route("social/twitter/profile")]
    public class TwitterProfileController : ApiControllerBase
    {
        private readonly ITwitterServiceService _twitterServiceService;

        public TwitterProfileController(ITwitterServiceService twitterServiceService)
        {
            _twitterServiceService = twitterServiceService;
        }

        public HttpResponseMessage Get([FromUri] TwitterRequestModel model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _twitterServiceService.GetProfile(model));
        }
    }
}