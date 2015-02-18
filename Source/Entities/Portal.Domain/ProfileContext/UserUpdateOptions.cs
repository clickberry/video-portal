// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.Domain.ProfileContext
{
    /// <summary>
    ///     User profile.
    /// </summary>
    public class UserUpdateOptions
    {
        public UserUpdateOptions()
        {
            NotifyOnVideoComments = true;
        }

        public virtual string UserName { get; set; }

        public virtual string City { get; set; }

        public virtual string Country { get; set; }

        public virtual string Timezone { get; set; }

        public bool NotifyOnVideoComments { get; set; }
    }
}