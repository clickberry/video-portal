// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Domain;
using Portal.Domain.SubscriptionContext;

namespace Portal.BLL.Subscriptions
{
    public interface IUrlTrackingStatService
    {
        Task CountAsync(DomainTrackingStat trackingUrl);

        /// <summary>
        /// Calculates number of cliks for period.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="from">Inclusive from date & time</param>
        /// <param name="to">Non-inclusive to date & time</param>
        /// <returns></returns>
        Task<long> GetTotalAsync(string subscriptionId, DateTime? from, DateTime? to);

        Task<DataResult<DomainTrackingStatPerUrl>> GetStatsPerUrlAsync(string subscriptionId, DataQueryOptions filter);

        Task<DataResult<DomainTrackingStat>> GetUrlStatsAsync(string subscriptionId, string url, DataQueryOptions filter);

        Task<IEnumerable<DomainTrackingStatPerDate>> GetStatsPerDateAsync(string subscriptionId, string url, DataQueryOptions filter);
    }
}