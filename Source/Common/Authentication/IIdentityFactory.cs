// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Authentication.IdentityProviders;
using Portal.Domain.ProfileContext;

namespace Authentication
{
    public interface IIdentityFactory
    {
        IMetadataProvider CreateMetadataProvider(ProviderType type);

        IIdentityProvider CreateIdentityProvider(ProviderType type);
    }
}