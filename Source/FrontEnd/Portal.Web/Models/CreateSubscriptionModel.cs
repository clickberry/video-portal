// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.Web.Models
{
    public class CreateSubscriptionModel
    {
        public string StripePublicKey { get; set; }

        public string Email { get; set; }
    }
}