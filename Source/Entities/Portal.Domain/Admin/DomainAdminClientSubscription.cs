// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Portal.Domain.SubscriptionContext;

namespace Portal.Domain.Admin
{
    public sealed class DomainAdminClientSubscription
    {
        public string Id { get; set; }

        public string SiteName { get; set; }

        public string SiteHostname { get; set; }

        public bool IsManuallyEnabled { get; set; }

        public ResourceState State { get; set; }

        public SubscriptionType Type { get; set; }

        public DateTime Created { get; set; }
    }
}