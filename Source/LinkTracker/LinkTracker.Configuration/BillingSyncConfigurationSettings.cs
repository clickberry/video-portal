// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace LinkTracker.Configuration
{
    /// <summary>
    ///     BillingSync configuration setting names.
    /// </summary>
    public class BillingSyncConfigurationSettings
    {
        public static readonly string MongoConnectionString = "MongoConnectionString";

        public static readonly string StripeApiKey = "StripeApiKey";

        public static readonly string SyncInterval = "SyncInterval";

        public static readonly string SubscriptionPlans = "SubscriptionPlans";

        public static readonly string BillingInvoiceItemDescriptionTemplate = "BillingInvoiceItemDescriptionTemplate";
    }
}