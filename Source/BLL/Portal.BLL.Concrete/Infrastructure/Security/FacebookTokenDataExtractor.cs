// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Facebook;
using Portal.BLL.Infrastructure;
using Portal.Domain.ProfileContext;
using Portal.DTO.User;
using Portal.Exceptions.CRUD;

namespace Portal.BLL.Concrete.Infrastructure.Security
{
    /// <summary>
    ///     Facebook token data extractor.
    /// </summary>
    public sealed class FacebookTokenDataExtractor : ITokenDataExtractor
    {
        public TokenData Get(IpData data)
        {
            var client = new FacebookClient(data.Token);
            var result = new TokenData();

            dynamic userData;

            try
            {
                userData = client.Get("me");
            }
            catch (FacebookOAuthException e)
            {
                throw new BadRequestException(e);
            }
            catch (WebExceptionWrapper e)
            {
                throw new BadGatewayException(e);
            }

            result.IdentityProvider = ProviderType.Facebook;
            result.Name = userData.name;
            result.UserIdentifier = userData.id;
            result.Token = data.Token;
            result.TokenSecret = data.TokenSecret;

            // If email was not requested
            try
            {
                result.Email = userData.email;
            }
            catch
            {
                result.Email = null;
            }

            return result;
        }
    }
}