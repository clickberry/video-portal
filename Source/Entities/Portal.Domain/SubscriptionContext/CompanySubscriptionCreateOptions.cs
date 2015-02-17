// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.Domain.SubscriptionContext
{
    public class CompanySubscriptionCreateOptions
    {
        public virtual SubscriptionType Type { get; set; }

        public virtual string SiteHostname { get; set; }

        public virtual string SiteName { get; set; }

        public virtual bool IsManuallyEnabled { get; set; }

        public virtual string GoogleAnalyticsId { get; set; }
    }
}