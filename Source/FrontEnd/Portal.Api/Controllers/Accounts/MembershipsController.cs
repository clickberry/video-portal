// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Portal.BLL.Services;
using Portal.Domain.ProfileContext;

namespace Portal.Api.Controllers.Accounts
{
    /// <summary>
    ///     Manages account memberships.
    /// </summary>
    [Authorize]
    [ValidationHttp]
    [RoutePrefix("accounts/profile/memberships")]
    public sealed class MembershipsController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public MembershipsController(IUserService userService)
        {
            _userService = userService;
        }

        //
        // DELETE api/accounts/profile/memberships/{provider}

        /// <summary>
        ///     Deletes user memberhip.
        /// </summary>
        [AuthorizeHttp]
        [Route("{provider}")]
        public async Task<HttpResponseMessage> Delete(int provider)
        {
            await _userService.DeleteMembersipAsync(UserId, (ProviderType)provider);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}