// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Portal.BLL.Infrastructure;
using Portal.Domain.ProfileContext;
using Portal.DTO.User;
using Portal.Exceptions.CRUD;
using VkSdk;

namespace Portal.BLL.Concrete.Infrastructure.Security
{
    /// <summary>
    ///     VK token data extractor.
    /// </summary>
    public sealed class VkTokenDataExtractor : ITokenDataExtractor
    {
        public TokenData Get(IpData data)
        {
            // Try to validate token
            var service = new VkClient();
            service.AuthenticateWith(data.Token);

            dynamic result;

            try
            {
                result = service.Get("users.get", new
                {
                    fields = "screen_name"
                });
            }
            catch (Exception e)
            {
                throw new BadGatewayException(e);
            }

            if (result.error != null)
            {
                dynamic message = result.error.error_msg ?? string.Empty;
                throw new BadRequestException(message.ToString());
            }

            try
            {
                dynamic user = result.response[0];
                return new TokenData
                {
                    IdentityProvider = ProviderType.Vk,
                    Name = string.Format("{0} {1}", user.first_name, user.last_name),
                    UserIdentifier = user.uid.ToString(),
                    Token = data.Token,
                    TokenSecret = data.TokenSecret
                };
            }
            catch (Exception e)
            {
                string message = string.Format("Unable to receive VK profile: {0}", e);
                throw new InternalServerErrorException(message);
            }
        }
    }
}