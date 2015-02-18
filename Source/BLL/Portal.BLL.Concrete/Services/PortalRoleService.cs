// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Portal.BLL.Services;
using Portal.Domain.PortalContext;
using Portal.Domain.RoleContext;

namespace Portal.BLL.Concrete.Services
{
    public sealed class PortalRoleService : IPortalRoleService
    {
        public List<DomainRole> GetAdminRoles()
        {
            return DomainRoles.AllAdminRoles.Select(r => new DomainRole { RoleName = r }).ToList();
        }
    }
}