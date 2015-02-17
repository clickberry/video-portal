// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoRepository;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Subscriptions
{
    public interface ITrackingStatRepository : IRepository<TrackingStatEntity>
    {
        Task DeleteBySubscriptionIdAsync(string subscriptionId);

        Task<DataResultEntity<TrackingStatPerUrlEntity>> AggregatePerUrlAsync(string subscriptionId, string redirectUrl,
            DateTime? dateFrom, DateTime? dateTo, string orderBy, bool? orderDesc, long? skip, long? take, bool? count);

        Task<IEnumerable<TrackingStatPerDateEntity>> AggregatePerDateAsync(string subscriptionId, string redirectUrl, DateTime? dateFrom, DateTime? dateTo);
    }
}