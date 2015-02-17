// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json.Linq;

namespace LinkTracker.Configuration.Azure
{
    public class AzureBillingSyncConfigurationProvider : AzureConfigurationProviderBase,
        IBillingSyncConfigurationProvider
    {
        public string MongoConnectionString
        {
            get { return Get(BillingSyncConfigurationSettings.MongoConnectionString); }
        }

        public string StripeApiKey
        {
            get { return Get(BillingSyncConfigurationSettings.StripeApiKey); }
        }

        public int SyncInterval
        {
            get { return Int32.Parse(Get(BillingSyncConfigurationSettings.SyncInterval)); }
        }

        public JObject SubscriptionPlans
        {
            get { return JObject.Parse((Get(BillingSyncConfigurationSettings.SubscriptionPlans))); }
        }

        public string BillingInvoiceItemDescriptionTemplate
        {
            get { return Get(BillingSyncConfigurationSettings.BillingInvoiceItemDescriptionTemplate); }
        }
    }
}