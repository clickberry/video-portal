// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Portal.Api.Models;
using Portal.BLL.Services;
using Portal.DTO.Facebook;
using Portal.Exceptions.CRUD;

namespace Portal.Api.Controllers.Social
{
    [ValidationHttp]
    [Route("social/facebook/extendtoken")]
    public class FacebookExtendController : ApiControllerBase
    {
        private readonly IFacebookService _facebookService;

        public FacebookExtendController(IFacebookService facebookService)
        {
            _facebookService = facebookService;
        }

        public HttpResponseMessage Post(FacebookTokenModel model)
        {
            string token;

            try
            {
                token = _facebookService.ExtendToken(model.Token);
            }
            catch (BadRequestException e)
            {
                ModelState.AddModelError("Token", e.Message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var response = new FacebookToken { Token = token };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}