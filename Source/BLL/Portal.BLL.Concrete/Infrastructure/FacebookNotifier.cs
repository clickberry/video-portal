// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Configuration;
using Facebook;
using Newtonsoft.Json.Linq;
using Portal.BLL.Infrastructure;
using Portal.Domain.ProfileContext;
using Portal.Exceptions.CRUD;

namespace Portal.BLL.Concrete.Infrastructure
{
    public sealed class FacebookNotifier : ISocialNetworkNotifier
    {
        private readonly IPortalFrontendSettings _settings;

        public FacebookNotifier(IPortalFrontendSettings settings)
        {
            _settings = settings;
        }

        public async Task SendWelcomeMessageAsync(DomainUser user, TokenData data)
        {
            string facebookMessage = _settings.FacebookRegistrationMessage;
            if (String.IsNullOrEmpty(facebookMessage))
            {
                return;
            }

            if (data == null || string.IsNullOrEmpty(data.Token))
            {
                throw new ArgumentNullException("data");
            }

            JObject message = JObject.Parse(facebookMessage);

            var fb = new FacebookClient(data.Token);
            var post = new
            {
                caption = (string)message["caption"],
                message = (string)message["message"],
                name = (string)message["name"],
                description = (string)message["description"],
                picture = (string)message["picture"],
                link = (string)message["link"]
            };

            try
            {
                await fb.PostTaskAsync("me/feed", post);
            }
            catch (FacebookOAuthException e)
            {
                //Permission error
                if (e.ErrorCode != 200)
                {
                    throw new BadRequestException(e);
                }
            }
            catch (WebExceptionWrapper e)
            {
                throw new BadGatewayException(e);
            }
        }
    }
}