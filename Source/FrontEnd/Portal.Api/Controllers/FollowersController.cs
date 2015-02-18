// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Portal.BLL.Services;
using Portal.Domain.PortalContext;
using Portal.Domain.ProfileContext;
using Portal.Mappers;

namespace Portal.Api.Controllers
{
    [ValidationHttp]
    public class FollowersController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public FollowersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // GET: /api/followers

        /// <summary>
        ///     Gets user followers.
        /// </summary>
        /// <returns></returns>
        [AuthorizeHttp(Roles = DomainRoles.User)]
        [Route("followers")]
        public async Task<IEnumerable<Follower>> GetFollowers()
        {
            DomainUser user = await _userService.GetAsync(UserId);
            return user.Followers;
        }


        // GET: /api/following/{id}

        /// <summary>
        ///     Gets users following specified user.
        /// </summary>
        /// <returns></returns>
        [Route("following/{id}")]
        public async Task<IEnumerable<Follower>> GetFollowing(string id)
        {
            return await _userService.FindFollowingUsersAsync(id);
        }


        // POST: /api/followers/{id}

        /// <summary>
        ///     Folow user.
        /// </summary>
        /// <param name="id">Follower id</param>
        /// <returns></returns>
        [AuthorizeHttp(Roles = DomainRoles.User)]
        [Route("followers/{id}")]
        public async Task<HttpResponseMessage> Post(string id)
        {
            DomainUser follower = await _userService.GetAsync(id);
            await _userService.AddFollowerAsync(UserId, _mapper.Map<DomainUser, Follower>(follower));

            return Request.CreateResponse(HttpStatusCode.Created);
        }


        // DELETE: /api/followers/{id}

        /// <summary>
        ///     Unfollow user.
        /// </summary>
        /// <param name="id">Follower id</param>
        /// <returns></returns>
        [AuthorizeHttp(Roles = DomainRoles.User)]
        [Route("followers/{id}")]
        public async Task<HttpResponseMessage> Delete(string id)
        {
            DomainUser follower = await _userService.GetAsync(id);
            await _userService.DeleteFollowerAsync(UserId, _mapper.Map<DomainUser, Follower>(follower));

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}