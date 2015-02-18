// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.Domain.Admin
{
    public sealed class DomainUserMembershipForAdmin
    {
        public string Provider { get; set; }

        public string Identity { get; set; }
    }
}