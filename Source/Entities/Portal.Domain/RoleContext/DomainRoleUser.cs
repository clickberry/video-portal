// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.Domain.RoleContext
{
    public sealed class DomainRoleUser
    {
        public string UserName { get; set; }

        public string UserId { get; set; }

        public string RoleName { get; set; }
    }
}