// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    [CollectionName("TrackingStat")]
    public class TrackingStatEntity : Entity
    {
        public DateTime Date { get; set; }

        public string ProjectId { get; set; }

        public string RedirectUrl { get; set; }

        public string SubscriptionId { get; set; }
    }
}