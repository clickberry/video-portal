// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Configuration;
using OkSdk;
using Portal.BLL.Infrastructure;
using Portal.Domain.ProfileContext;
using Portal.DTO.User;
using Portal.Exceptions.CRUD;

namespace Portal.BLL.Concrete.Infrastructure.Security
{
    /// <summary>
    ///     Odnoklassniki token data extractor.
    /// </summary>
    public sealed class OdnoklassnikiTokenDataExtractor : ITokenDataExtractor
    {
        private readonly OkClient _service;

        public OdnoklassnikiTokenDataExtractor(IPortalFrontendSettings settings)
        {
            // Pass your credentials to the service
            string applicationId = settings.OkApplicationId;
            string appSecret = settings.OkApplicationSecret;
            string appPublic = settings.OkApplicationPublic;

            _service = new OkClient(applicationId, appSecret, appPublic);
        }

        public TokenData Get(IpData data)
        {
            // Try to validate token
            _service.AuthenticateWith(data.Token);

            dynamic user;

            try
            {
                user = _service.Get("users.getCurrentUser");
            }
            catch (Exception e)
            {
                throw new BadGatewayException(e);
            }

            if (user.error_msg != null)
            {
                throw new BadRequestException(user.error_msg.ToString());
            }

            try
            {
                return new TokenData
                {
                    IdentityProvider = ProviderType.Odnoklassniki,
                    Name = user.name.ToString(),
                    UserIdentifier = user.uid.ToString(),
                    Token = data.Token,
                    TokenSecret = data.TokenSecret
                };
            }
            catch (Exception e)
            {
                string message = string.Format("Unable to receive Odnoklassniki profile: {0}", e);
                throw new InternalServerErrorException(message);
            }
        }
    }
}