// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.Web.Models
{
    public class CreateSubscriptionModel
    {
        public string StripePublicKey { get; set; }

        public string Email { get; set; }
    }
}