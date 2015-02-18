// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.Domain.BillingContext
{
    public class DomainCharge
    {
        public string Id { get; set; }

        public int AmountInCents { get; set; }

        public DateTime Created { get; set; }

        public string Currency { get; set; }

        public string CustomerId { get; set; }

        public string Description { get; set; }

        public string InvoiceId { get; set; }

        public bool IsPaid { get; set; }
    }
}