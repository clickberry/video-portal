// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Configuration;
using Facebook;
using Portal.BLL.Services;
using Portal.Exceptions.CRUD;

namespace Portal.BLL.Concrete.Services
{
    public sealed class FacebookService : IFacebookService
    {
        private readonly IPortalFrontendSettings _settings;

        public FacebookService(IPortalFrontendSettings settings)
        {
            _settings = settings;
        }

        public string ExtendToken(string token)
        {
            var fb = new FacebookClient();
            string appId = _settings.FacebookApplicationId;
            string appSecret = _settings.FacebookApplicationSecret;

            dynamic result;

            try
            {
                result = fb.Get("oauth/access_token", new
                {
                    grant_type = "fb_exchange_token",
                    client_id = appId,
                    client_secret = appSecret,
                    fb_exchange_token = token
                });
            }
            catch (FacebookOAuthException e)
            {
                throw new BadRequestException(e);
            }
            catch (WebExceptionWrapper e)
            {
                throw new BadGatewayException(e);
            }

            return result.access_token;
        }
    }
}