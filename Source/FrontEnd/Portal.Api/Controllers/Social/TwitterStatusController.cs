// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Portal.Api.Models;
using Portal.BLL.Services;
using Portal.DAL.Entities.Table;
using Portal.Domain.ProfileContext;

namespace Portal.Api.Controllers.Social
{
    [ValidationHttp]
    [Route("social/twitter/status")]
    public class TwitterStatusController : ApiControllerBase
    {
        private readonly ITwitterServiceService _twitterServiceService;

        public TwitterStatusController(ITwitterServiceService twitterServiceService)
        {
            _twitterServiceService = twitterServiceService;
        }

        public async Task<HttpResponseMessage> Post(TwitterStatusModel model)
        {
            // If token and token secret was not overridden use last one saved
            if (string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.TokenSecret))
            {
                UserMembershipEntity twitterMembership = Memberships.FirstOrDefault(p => p.IdentityProvider == ProviderType.Twitter.ToString());
                if (twitterMembership != null)
                {
                    model.Token = twitterMembership.Token;
                    model.TokenSecret = twitterMembership.TokenSecret;
                }
            }

            await _twitterServiceService.SetStatusAsync(model);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}