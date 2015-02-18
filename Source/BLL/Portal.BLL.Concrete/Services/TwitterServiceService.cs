// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Configuration;
using Portal.BLL.Services;
using Portal.DTO.Twitter;
using Portal.Exceptions.CRUD;
using TweetSharp;
using TwitterStatus = Portal.DTO.Twitter.TwitterStatus;

namespace Portal.BLL.Concrete.Services
{
    /// <summary>
    ///     Twitter social integration service.
    /// </summary>
    public class TwitterServiceService : ITwitterServiceService
    {
        private readonly IPortalFrontendSettings _settings;

        public TwitterServiceService(IPortalFrontendSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        ///     Sets an user status.
        /// </summary>
        /// <param name="status">Status entity.</param>
        public async Task SetStatusAsync(TwitterStatus status)
        {
            // Pass your credentials to the service
            string consumerKey = _settings.TwitterConsumerKey;
            string consumerSecret = _settings.TwitterConsumerSecret;

            // Authorize
            var service = new TwitterService(consumerKey, consumerSecret);
            service.AuthenticateWith(status.Token, status.TokenSecret);

            // Send message
            TweetSharp.TwitterStatus result;
            if (string.IsNullOrEmpty(status.ScreenshotUrl))
            {
                var tweet = new SendTweetOptions { Status = status.Message };
                result = service.SendTweet(tweet);
            }
            else
            {
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(status.ScreenshotUrl);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new BadRequestException(response.ReasonPhrase);
                    }

                    Stream stream = await response.Content.ReadAsStreamAsync();
                    var tweet = new SendTweetWithMediaOptions
                    {
                        Status = status.Message,
                        Images = new Dictionary<string, Stream>
                        {
                            { "media", stream }
                        }
                    };

                    result = service.SendTweetWithMedia(tweet);
                }
            }

            // Check result status
            if (result != null)
            {
                return;
            }

            // Check response status code
            switch (service.Response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.BadRequest:
                    // Invalid credentials or request data
                    throw new BadRequestException(service.Response.Response);

                case HttpStatusCode.Forbidden:
                    throw new ForbiddenException(service.Response.Response);

                case (HttpStatusCode)429:
                    throw new TooManyRequestsException(service.Response.Response);
            }

            // Twitter internal errors
            if ((int)service.Response.StatusCode >= 500)
            {
                throw new BadGatewayException(service.Response.Response);
            }

            string message = string.Format("Unable to send tweet. Status code {0}: {1}", service.Response.StatusCode, service.Response);
            throw new InternalServerErrorException(message);
        }

        /// <summary>
        ///     Gets an user profile information.
        /// </summary>
        /// <param name="request">Request message.</param>
        /// <returns>User profile.</returns>
        public TwitterProfile GetProfile(TwitterRequest request)
        {
            // Pass your credentials to the service
            string consumerKey = _settings.TwitterConsumerKey;
            string consumerSecret = _settings.TwitterConsumerSecret;

            // Authorize
            var service = new TwitterService(consumerKey, consumerSecret);
            service.AuthenticateWith(request.Token, request.TokenSecret);

            // Get user info
            TwitterUser user = service.GetUserProfile(new GetUserProfileOptions());
            if (user != null)
            {
                return new TwitterProfile
                {
                    Id = user.Id,
                    ImageUrl = user.ProfileImageUrl,
                    Name = user.Name,
                    ScreenName = user.ScreenName
                };
            }

            // Check response status code
            switch (service.Response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.BadRequest:
                    // Invalid credentials or request data
                    throw new BadRequestException(service.Response.Response);

                case (HttpStatusCode)429:
                    throw new TooManyRequestsException(service.Response.Response);
            }

            // Twitter internal errors
            if ((int)service.Response.StatusCode >= 500)
            {
                throw new BadGatewayException(service.Response.Response);
            }

            string message = string.Format("Unable to receive twitter profile. Status code {0}: {1}", service.Response.StatusCode, service.Response);
            throw new InternalServerErrorException(message);
        }
    }
}