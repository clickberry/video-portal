// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Portal.BLL.Services;
using Portal.Domain.PortalContext;
using Portal.Domain.ProfileContext;
using Portal.DTO.Portal;

namespace Portal.Api.Controllers.Roles
{
    [AuthorizeHttp(Roles = DomainRoles.SuperAdministrator)]
    [ValidationHttp]
    [Route("roles/{id}/users")]
    public class RoleUsersController : ApiController
    {
        private readonly IAdminUserService _adminUserService;

        public RoleUsersController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        // GET api/roles/5/users
        public async Task<HttpResponseMessage> Get(string id)
        {
            List<DomainUser> users = await _adminUserService.GetUsersInRoleAsync(id);

            IEnumerable<RoleUser> result = users.Select(p => new RoleUser { UserId = p.Id, UserName = p.Name });

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}