// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using MongoRepository;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Subscriptions
{
    public interface ICompanyRepository : IRepository<CompanyEntity>
    {
        Task<CompanyEntity> FindByUserAsync(string userId);

        Task<CompanyEntity> FindBySubscriptionAsync(string subscriptionId);

        Task<CompanyEntity> FindByCustomerAsync(string customerId);

        Task SetSubscriptionLastSyncDateAsync(string subscriptionId, DateTime date);

        Task SetSubscriptionLastCycleDateAsync(string subscriptionId, DateTime date);

        Task SetSubscriptionHasTrialClicksAsync(string subscriptionId, bool value);

        Task UpdateUserIdFromAsync(string userId, string toUserId);
    }
}