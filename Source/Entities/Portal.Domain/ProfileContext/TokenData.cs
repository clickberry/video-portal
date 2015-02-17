// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.Domain.ProfileContext
{
    public class TokenData
    {
        public virtual string Name { get; set; }

        public virtual string Email { get; set; }

        public string UserIdentifier { get; set; }

        public ProviderType IdentityProvider { get; set; }

        public virtual string Token { get; set; }

        public virtual string TokenSecret { get; set; }
    }
}