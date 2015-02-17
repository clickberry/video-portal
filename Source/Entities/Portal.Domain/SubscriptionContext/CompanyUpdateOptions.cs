// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.Domain.SubscriptionContext
{
    public class CompanyUpdateOptions
    {
        public string Name { get; set; }

        public string Country { get; set; }

        public string Ein { get; set; }

        public string Address { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}