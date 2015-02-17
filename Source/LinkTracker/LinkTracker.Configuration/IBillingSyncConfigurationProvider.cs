// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json.Linq;

namespace LinkTracker.Configuration
{
    public interface IBillingSyncConfigurationProvider
    {
        string MongoConnectionString { get; }

        string StripeApiKey { get; }

        int SyncInterval { get; }

        JObject SubscriptionPlans { get; }

        string BillingInvoiceItemDescriptionTemplate { get; }
    }
}