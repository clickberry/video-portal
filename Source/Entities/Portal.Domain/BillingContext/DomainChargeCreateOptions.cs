// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.Domain.BillingContext
{
    public class DomainChargeCreateOptions
    {
        public int AmountInCents { get; set; }

        public string Currency { get; set; }

        public string CustomerId { get; set; }

        public string Description { get; set; }

        public string Card { get; set; }
    }
}