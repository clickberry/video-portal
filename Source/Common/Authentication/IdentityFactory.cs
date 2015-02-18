// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Authentication.IdentityProviders;
using Authentication.IdentityProviders.Ok;
using Authentication.IdentityProviders.Twitter;
using Authentication.IdentityProviders.VK;
using Configuration;
using Portal.Domain.ProfileContext;

namespace Authentication
{
    public sealed class IdentityFactory : IIdentityFactory
    {
        private readonly IPortalFrontendSettings _settings;

        public IdentityFactory(IPortalFrontendSettings settings)
        {
            _settings = settings;
        }

        public IMetadataProvider CreateMetadataProvider(ProviderType type)
        {
            switch (type)
            {
                case ProviderType.Twitter:
                    return new TwitterSecurityTokenServiceConfiguration();

                case ProviderType.Vk:
                    return new VkSecurityTokenServiceConfiguration();

                case ProviderType.Odnoklassniki:
                    return new OkSecurityTokenServiceConfiguration();

                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        public IIdentityProvider CreateIdentityProvider(ProviderType type)
        {
            switch (type)
            {
                case ProviderType.Twitter:
                    return new TwitterSecurityTokenService(new TwitterSecurityTokenServiceConfiguration(), _settings);

                case ProviderType.Vk:
                    return new VkSecurityTokenService(new VkSecurityTokenServiceConfiguration(), _settings);

                case ProviderType.Odnoklassniki:
                    return new OkSecurityTokenService(new OkSecurityTokenServiceConfiguration(), _settings);

                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }
    }
}