// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace LinkTracker.Configuration
{
    public interface IApiConfigurationProvider
    {
        string ProjectBaseUri { get; }

        string MongoConnectionString { get; }

        string StripeApiKey { get; }

        string StripePlanIds { get; }
    }
}