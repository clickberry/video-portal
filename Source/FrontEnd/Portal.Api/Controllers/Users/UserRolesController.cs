// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Portal.Api.Models;
using Portal.BLL.Services;
using Portal.Domain.PortalContext;
using Portal.Domain.ProfileContext;
using Portal.DTO.Portal;
using Portal.Resources.Api;

namespace Portal.Api.Controllers.Users
{
    [AuthorizeHttp(Roles = DomainRoles.SuperAdministrator)]
    [ValidationHttp]
    [Route("users/{id}/roles")]
    public class UserRolesController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public UserRolesController(IUserService userService)
        {
            _userService = userService;
        }

        // POST api/users/5/roles
        public async Task<HttpResponseMessage> Post(string id, PortalRoleModel role)
        {
            // Add user role
            await _userService.AddRoleAsync(id, role.Name);

            return Request.CreateResponse(HttpStatusCode.Created, (PortalRole)role);
        }

        // GET api/users/5/roles
        public async Task<HttpResponseMessage> Get(string id)
        {
            // Get user
            DomainUser user = await _userService.GetAsync(id);

            IEnumerable<PortalRole> result = user.Roles.Select(p => new PortalRole { Name = p });

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // PUT api/users/5/roles
        public HttpResponseMessage Put()
        {
            return Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, ResponseMessages.MethodNotAllowed);
        }

        // DELETE api/users/5/roles
        public async Task<HttpResponseMessage> Delete(string id, PortalRoleModel role)
        {
            await _userService.DeleteRoleAsync(id, role.Name);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}