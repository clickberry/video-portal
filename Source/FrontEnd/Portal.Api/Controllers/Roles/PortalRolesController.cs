// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Portal.BLL.Services;
using Portal.Domain.PortalContext;
using Portal.Domain.RoleContext;
using Portal.DTO.Portal;

namespace Portal.Api.Controllers.Roles
{
    [AuthorizeHttp(Roles = DomainRoles.SuperAdministrator)]
    [ValidationHttp]
    [Route("roles")]
    public class PortalRolesController : ApiControllerBase
    {
        private readonly IPortalRoleService _portalRoleService;

        public PortalRolesController(IPortalRoleService portalRoleService)
        {
            _portalRoleService = portalRoleService;
        }

        // GET api/roles
        public HttpResponseMessage Get()
        {
            List<DomainRole> roles = _portalRoleService.GetAdminRoles();

            IEnumerable<PortalRole> result = roles.Select(p => new PortalRole { Name = p.RoleName });

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}