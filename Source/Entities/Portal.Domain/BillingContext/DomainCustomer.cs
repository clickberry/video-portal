// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.Domain.BillingContext
{
    public class DomainCustomer
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public DateTime Created { get; set; }

        public bool IsDeleted { get; set; }

        public int AccountBalanceInCents { get; set; }
    }
}