// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.Domain.SubscriptionContext
{
    public class CompanySubscription
    {
        public string Id { get; set; }

        public string SiteHostname { get; set; }

        public string SiteName { get; set; }

        public SubscriptionType Type { get; set; }

        public DateTime Created { get; set; }

        public ResourceState State { get; set; }

        public string GoogleAnalyticsId { get; set; }

        public bool IsManuallyEnabled { get; set; }

        public DateTime? LastSyncDate { get; set; }

        public DateTime? LastCycleDate { get; set; }

        public bool HasTrialClicks { get; set; }
    }
}