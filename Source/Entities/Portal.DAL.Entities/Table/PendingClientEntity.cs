// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    [CollectionName("PendingClient")]
    public class PendingClientEntity : Entity
    {
        public DateTime Created { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public string ContactPerson { get; set; }

        public string CompanyName { get; set; }

        public string Country { get; set; }

        public string Ein { get; set; }

        public string Address { get; set; }

        public string ZipCode { get; set; }

        public string PhoneNumber { get; set; }
    }
}