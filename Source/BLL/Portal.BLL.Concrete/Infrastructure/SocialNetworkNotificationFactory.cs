// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Configuration;
using Portal.BLL.Infrastructure;
using Portal.Domain.ProfileContext;

namespace Portal.BLL.Concrete.Infrastructure
{
    public sealed class SocialNetworkNotificationFactory : ISocialNetworkNotificationFactory
    {
        private readonly IPortalFrontendSettings _settings;

        public SocialNetworkNotificationFactory(IPortalFrontendSettings settings)
        {
            _settings = settings;
        }

        public ISocialNetworkNotifier GetNotifier(ProviderType identityType)
        {
            switch (identityType)
            {
                case ProviderType.Facebook:
                    return new FacebookNotifier(_settings);
            }

            return null;
        }
    }
}