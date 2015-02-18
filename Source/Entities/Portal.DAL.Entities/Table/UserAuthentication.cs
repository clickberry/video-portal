// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.DAL.Entities.Table
{
    public class UserAuthentication
    {
        public string ETag { get; set; }

        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public string IdentityProvider { get; set; }
    }
}