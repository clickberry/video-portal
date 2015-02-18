// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Portal.Domain.SubscriptionContext
{
    public class DomainCompany
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string Address { get; set; }

        public string ZipCode { get; set; }

        public string Ein { get; set; }

        public string Phone { get; set; }

        public List<string> Users { get; set; }

        public List<CompanySubscription> Subscriptions { get; set; }

        public string BillingCustomerId { get; set; }

        public DateTime Created { get; set; }

        public ResourceState State { get; set; }
    }
}