// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.Domain.SubscriptionContext
{
    public class CompanyChargeOptions
    {
        public string Id { get; set; }

        public int AmountInCents { get; set; }

        public string Currency { get; set; }

        public string Description { get; set; }

        public string TokenId { get; set; }
    }
}