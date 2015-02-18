// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Portal.Domain.ProjectContext;

namespace Portal.Domain.ProfileContext
{
    /// <summary>
    ///     User profile.
    /// </summary>
    public sealed class DomainUser
    {
        public DomainUser()
        {
            NotifyOnVideoComments = true;
        }

        public string Id { get; set; }

        public string ApplicationName { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public long MaximumStorageSpace { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Timezone { get; set; }

        public ProductType UserAgent { get; set; }

        public ResourceState State { get; set; }


        // Extra properties

        public long UsedStorageSpace { get; set; }

        public List<UserMembership> Memberships { get; set; }

        /// <summary>
        ///     Roles should be managed via service.
        /// </summary>
        public IReadOnlyList<string> Roles { get; set; }

        /// <summary>
        ///     Gets a value indicating whether user should be notified
        ///     for video comments.
        /// </summary>
        public bool NotifyOnVideoComments { get; set; }

        public List<Follower> Followers { get; set; }
    }
}