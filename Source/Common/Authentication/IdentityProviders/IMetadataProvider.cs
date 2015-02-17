// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Authentication.IdentityProviders
{
    public interface IMetadataProvider
    {
        string GetFederationMetadata(Uri endpoint);
    }
}