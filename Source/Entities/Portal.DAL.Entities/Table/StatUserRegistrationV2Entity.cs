// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    [CollectionName("StatUserRegistrationV2")]
    public class StatUserRegistrationV2Entity : StatEntity
    {
        public string EventId { get; set; }

        public string UserId { get; set; }

        public string ProductName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string IdentityProvider { get; set; }

        public DateTime DateTime { get; set; }
    }
}