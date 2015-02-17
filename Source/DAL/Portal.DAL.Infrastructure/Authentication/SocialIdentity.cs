// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Infrastructure.Authentication
{
    public sealed class SocialIdentity : GenericIdentity
    {
        public SocialIdentity(string userId)
            : base(userId)
        {
            Memberships = new List<UserMembershipEntity>();
            SocialClaims = new List<Claim>();
        }

        public List<UserMembershipEntity> Memberships { get; set; }

        /// <summary>
        ///     Claims retrieved from social provider.
        /// </summary>
        public List<Claim> SocialClaims { get; set; }
    }
}