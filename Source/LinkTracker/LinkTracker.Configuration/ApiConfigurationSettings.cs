// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace LinkTracker.Configuration
{
    /// <summary>
    ///     Api configuration setting names.
    /// </summary>
    public class ApiConfigurationSettings
    {
        public static string ProjectBaseUri = "ProjectBaseUri";

        public static readonly string MongoConnectionString = "MongoConnectionString";

        public static readonly string StripeApiKey = "StripeApiKey";

        public static string StripePlanIds = "StripePlanIds";
    }
}