// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Configuration;

namespace Portal.Api.Controllers.Accounts
{
    /// <summary>
    ///     Handles account emails & password.
    /// </summary>
    [Route("accounts/networks")]
    public class NetworksController : ApiControllerBase
    {
        private readonly IPortalFrontendSettings _settings;

        public NetworksController(IPortalFrontendSettings settings)
        {
            _settings = settings;
        }

        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_settings.SocialNetworks, Encoding.UTF8, "application/json")
            };
        }
    }
}