// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Portal.Domain;
using Portal.Domain.SubscriptionContext;

namespace Portal.DTO.Admin
{
    public class AdminClientSubscription
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