// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.Domain.StatisticContext
{
    public class DomainStatUser
    {
        protected DomainStatUser()
        {
        }

        public DomainStatUser(string userId, string email, string userName, string identityProvider, string productName, string productVersion)
        {
            UserId = userId;
            Email = email;
            UserName = userName;
            IdentityProvider = identityProvider;
            ProductName = productName;
            ProductVersion = productVersion;
        }

        public string UserId { get; protected set; }

        public string Email { get; protected set; }

        public string UserName { get; protected set; }

        public string IdentityProvider { get; protected set; }

        public string ProductName { get; protected set; }

        public string ProductVersion { get; protected set; }
    }
}