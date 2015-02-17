// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Portal.Domain.ProfileContext;

namespace Portal.DTO.User
{
    /// <summary>
    ///     User profile.
    /// </summary>
    public class Profile
    {
        public Profile()
        {
            NotifyOnVideoComments = true;
        }

        public virtual string Id { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Email { get; set; }

        public virtual string City { get; set; }

        public virtual string Country { get; set; }

        public virtual string Timezone { get; set; }

        public long MaximumDiskSpace { get; set; }

        public long UsedDiskSpace { get; set; }

        public string AvatarUrl { get; set; }

        public List<ProviderType> Memberships { get; set; }

        public bool NotifyOnVideoComments { get; set; }
    }
}