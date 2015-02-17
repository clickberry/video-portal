// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace LinkTracker.Domain
{
    public class DomainTrackingUrl
    {
        public string Id { get; set; }

        public string ProjectId { get; set; }

        public Uri RedirectUrl { get; set; }

        public string SubscriptionId { get; set; }

        public string Key { get; set; }

        public Uri RequestUri { get; set; }
    }
}