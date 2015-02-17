// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Portal.Domain.SubscriptionContext;

namespace Portal.DTO.Subscriptions
{
    public class Subscription
    {
        public virtual string Id { get; set; }

        public virtual string SiteHostname { get; set; }

        public virtual string SiteName { get; set; }

        public virtual SubscriptionType Type { get; set; }

        public virtual DateTime Created { get; set; }

        public virtual string GoogleAnalyticsId { get; set; }

        public virtual int State { get; set; }
    }
}