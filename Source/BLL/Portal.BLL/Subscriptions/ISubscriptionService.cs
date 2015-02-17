// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Portal.Domain.SubscriptionContext;

namespace Portal.BLL.Subscriptions
{
    public interface ISubscriptionService
    {
        Task<CompanySubscription> AddAsync(string userId, CompanySubscriptionCreateOptions options);

        Task<CompanySubscription> GetAsync(string subscriptionId);

        Task<CompanySubscription> UpdateAsync(string userId, string subscriptionId, CompanySubscriptionUpdateOptions options);

        Task<CompanySubscription> UpdateLastSyncDateAsync(string subscriptionId, DateTime date);

        Task<CompanySubscription> UpdateLastCycleDateAsync(string subscriptionId, DateTime date);

        Task<CompanySubscription> UpdateHasTrialClicksAsync(string subscriptionId, bool value);

        Task DeleteAsync(string userId, string subscriptionId);
    }
}