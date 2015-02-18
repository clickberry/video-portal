// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Web;
using Authentication.IdentityProviders.Twitter;
using Authentication.IdentityProviders.VK;
using Portal.BLL.Concrete.Infrastructure.FederatedIdentity;
using Portal.BLL.Infrastructure;
using Portal.Domain.ProfileContext;
using Portal.DTO.User;
using Portal.Exceptions.CRUD;

namespace Portal.BLL.Concrete.Infrastructure.Security
{
    /// <summary>
    ///     ACS SWT token data extractor.
    /// </summary>
    public sealed class SwtTokenDataExtractor : ITokenDataExtractor
    {
        public TokenData Get(IpData data)
        {
            // Validate security token
            var validator = new SimpleWebTokenValidator
            {
                SharedKeyBase64 = "ZY+lm6+MQeWVSeMdV+apXAaaIGNTZT7ztfBskldOMn0="
            };

            Dictionary<string, string> claims;

            try
            {
                claims = validator.ValidateToken(data.Token).Claims;
            }
            catch (HttpException e)
            {
                throw new BadRequestException(e);
            }
            catch (InvalidSecurityTokenException e)
            {
                throw new BadRequestException(e);
            }

            // Get values
            string identityProvider = claims.Single(p => p.Key == CommonClaims.AcsIdentityProvider).Value;

            return new TokenData
            {
                UserIdentifier = claims.Single(p => p.Key == CommonClaims.NameIdentifier).Value,
                Name = claims.SingleOrDefault(p => p.Key == CommonClaims.Name).Value,
                Email = claims.SingleOrDefault(p => p.Key == CommonClaims.Email).Value,
                IdentityProvider = IdentityTypeParser.ParseProviderType(identityProvider),
                Token = GetToken(claims),
                TokenSecret = GetTokenSecret(claims)
            };
        }

        private static string GetToken(IReadOnlyDictionary<string, string> claims)
        {
            // Try facebook token
            if (claims.ContainsKey(CommonClaims.FacebookAccessToken))
            {
                return claims[CommonClaims.FacebookAccessToken];
            }

            // Try twitter token
            if (claims.ContainsKey(TwitterClaims.TwitterToken))
            {
                return claims[TwitterClaims.TwitterToken];
            }

            // Try vk token
            if (claims.ContainsKey(VkClaims.VkToken))
            {
                return claims[VkClaims.VkToken];
            }

            return null;
        }

        private static string GetTokenSecret(IReadOnlyDictionary<string, string> claims)
        {
            // Try twitter token secret
            if (claims.ContainsKey(TwitterClaims.TwitterTokenSecret))
            {
                return claims[TwitterClaims.TwitterTokenSecret];
            }

            return null;
        }
    }
}