// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Authentication.IdentityProviders.Twitter;
using Authentication.IdentityProviders.VK;
using Newtonsoft.Json.Linq;
using Portal.BLL.Infrastructure;
using Portal.Domain.ProfileContext;
using Portal.DTO.User;

namespace Portal.BLL.Concrete.Infrastructure.Security
{
    /// <summary>
    ///     Claims token data extractor.
    /// </summary>
    public sealed class ClaimsTokenDataExtractor : ITokenDataExtractor
    {
        public TokenData Get(IpData data)
        {
            JObject claims = JObject.Parse(data.Token);

            var provider = (string)claims[CommonClaims.AcsIdentityProvider];
            var identifier = (string)claims[CommonClaims.NameIdentifier];
            var name = (string)claims[CommonClaims.Name];
            var email = (string)claims[CommonClaims.Email];

            return new TokenData
            {
                Email = email,
                Name = name,
                IdentityProvider = IdentityTypeParser.ParseProviderType(provider),
                UserIdentifier = identifier,
                Token = GetToken(claims),
                TokenSecret = GetTokenSecret(claims)
            };
        }

        private static string GetToken(IDictionary<string, JToken> claims)
        {
            // Try facebook token
            var token = (string)claims[CommonClaims.FacebookAccessToken];

            // Try twitter token
            if (string.IsNullOrEmpty(token))
            {
                token = (string)claims[TwitterClaims.TwitterToken];
            }

            // Try vk token
            if (string.IsNullOrEmpty(token))
            {
                token = (string)claims[VkClaims.VkToken];
            }

            return token;
        }

        private static string GetTokenSecret(IDictionary<string, JToken> claims)
        {
            // Try twitter token secret
            return (string)claims[TwitterClaims.TwitterTokenSecret];
        }
    }
}