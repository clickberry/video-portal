// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using Asp.Infrastructure.Attributes.WebApi;
using Portal.DAL.Entities.Table;
using Portal.DAL.Infrastructure.Authentication;

namespace Portal.Api.Controllers
{
    [ExceptionHandlingWebApi]
    public abstract class ApiControllerBase : ApiController
    {
        /// <summary>
        ///     Gets a current user identity.
        /// </summary>
        protected string UserId
        {
            get { return User.Identity.Name; }
        }

        /// <summary>
        ///     Gets a current user identity.
        /// </summary>
        protected List<UserMembershipEntity> Memberships
        {
            get
            {
                var socialIdentity = User.Identity as SocialIdentity;
                return socialIdentity != null ? socialIdentity.Memberships : new List<UserMembershipEntity>();
            }
        }

        /// <summary>
        ///     Gets a current application name.
        /// </summary>
        protected string AppName
        {
            get { return Request.RequestUri.Host; }
        }

        /// <summary>
        ///     Extracts User agent. (KO)
        /// </summary>
        protected string UserAgent
        {
            get { return HttpContext.Current.Request.Headers["User-Agent"]; }
        }
    }
}