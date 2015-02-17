// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;
using Configuration;
using Portal.BLL.Infrastructure;
using Portal.Domain.ProfileContext;
using Portal.DTO.User;
using Portal.Exceptions.CRUD;
using TweetSharp;

namespace Portal.BLL.Concrete.Infrastructure.Security
{
    /// <summary>
    ///     Twitter token data extractor.
    /// </summary>
    public sealed class TwitterTokenDataExtractor : ITokenDataExtractor
    {
        private const string TwitterAccountPage = "https://twitter.com/account/redirect_by_id?id={0}";
        private readonly IPortalFrontendSettings _settings;

        public TwitterTokenDataExtractor(IPortalFrontendSettings settings)
        {
            _settings = settings;
        }

        public TokenData Get(IpData data)
        {
            // Pass your credentials to the service
            string consumerKey = _settings.TwitterConsumerKey;
            string consumerSecret = _settings.TwitterConsumerSecret;

            // Try to validate token
            var service = new TwitterService(consumerKey, consumerSecret);
            service.AuthenticateWith(data.Token, data.TokenSecret);

            TwitterUser user = service.GetUserProfile(new GetUserProfileOptions());
            if (user != null)
            {
                return new TokenData
                {
                    IdentityProvider = ProviderType.Twitter,
                    Name = user.Name,
                    UserIdentifier = string.Format(TwitterAccountPage, user.Id),
                    Token = data.Token,
                    TokenSecret = data.TokenSecret
                };
            }

            // Check response status code
            switch (service.Response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.BadRequest:
                    throw new BadRequestException();

                case (HttpStatusCode)429:
                    throw new TooManyRequestsException();
            }

            // Twitter internal errors
            if ((int)service.Response.StatusCode >= 500)
            {
                throw new BadGatewayException(service.Response.Response);
            }

            string message = string.Format("Unable to receive twitter profile. Status code {0}: {1}", service.Response.StatusCode, service.Response.InnerException);
            throw new InternalServerErrorException(message);
        }
    }
}