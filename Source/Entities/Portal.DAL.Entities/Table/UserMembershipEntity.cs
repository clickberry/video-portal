// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.DAL.Entities.Table
{
    public class UserMembershipEntity
    {
        public string IdentityProvider { get; set; }

        public string UserIdentifier { get; set; }

        public string Token { get; set; }

        public string TokenSecret { get; set; }
    }
}