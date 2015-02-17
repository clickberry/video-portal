// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace LinkTracker.Configuration.Azure
{
    public class AzureApiConfigurationProvider : AzureConfigurationProviderBase, IApiConfigurationProvider
    {
        public string ProjectBaseUri
        {
            get { return Get(ApiConfigurationSettings.ProjectBaseUri); }
        }

        public string MongoConnectionString
        {
            get { return Get(ApiConfigurationSettings.MongoConnectionString); }
        }

        public string StripeApiKey
        {
            get { return Get(ApiConfigurationSettings.StripeApiKey); }
        }

        public string StripePlanIds
        {
            get { return Get(ApiConfigurationSettings.StripePlanIds); }
        }
    }
}