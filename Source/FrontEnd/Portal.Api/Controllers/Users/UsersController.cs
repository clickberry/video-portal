// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.Domain.PortalContext;
using Portal.Domain.ProfileContext;
using Portal.DTO.Portal;
using Portal.DTO.User;

namespace Portal.Api.Controllers.Users
{
    [ValidationHttp]
    [RoutePrefix("users")]
    public class UsersController : ApiControllerBase
    {
        private readonly IAdminUserService _adminUserService;
        private readonly IUserAvatarProvider _avatarProvider;
        private readonly IUserService _userService;

        public UsersController(IUserService userService, IAdminUserService adminUserService, IUserAvatarProvider avatarProvider)
        {
            _userService = userService;
            _adminUserService = adminUserService;
            _avatarProvider = avatarProvider;
        }

        // GET api/users/$filter=...
        [Route]
        [HttpGet]
        [AuthorizeHttp(Roles = DomainRoles.SuperAdministrator)]
        public async Task<IEnumerable<RoleUser>> FindUsersByName(string userName)
        {
            // Checks whether user exists
            return (await _adminUserService.FindByNameAsync(userName))
                .Select(p => new RoleUser
                {
                    UserId = p.Id,
                    UserName = p.Name
                });
        }

        [Route("{id}")]
        public async Task<HttpResponseMessage> Get(string id)
        {
            DomainUser user = await _userService.GetAsync(id);

            var userInfo = new UserInfo
            {
                Id = user.Id,
                UserName = user.Name,
                AvatarUrl = _avatarProvider.GetAvatar(user)
            };

            return Request.CreateResponse(userInfo);
        }
    }
}