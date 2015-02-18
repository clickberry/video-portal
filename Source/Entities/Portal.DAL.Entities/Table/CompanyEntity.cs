// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    [CollectionName("Company")]
    public class CompanyEntity : Entity
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string NameSort { get; set; }

        public string Country { get; set; }

        public string Ein { get; set; }

        public string Address { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public List<string> Users { get; set; }

        public List<SubscriptionEntity> Subscriptions { get; set; }

        /// <summary>
        ///     Customer identifier in billing system.
        /// </summary>
        public string BillingCustomerId { get; set; }

        public DateTime Created { get; set; }

        public int State { get; set; }
    }
}