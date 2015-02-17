// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoRepository;
using Portal.Common.Helpers;
using Portal.DAL.Entities.Table;
using Portal.DAL.Subscriptions;

namespace Portal.DAL.Azure.Subscriptions
{
    public class CompanyRepository : MongoRepository<CompanyEntity>, ICompanyRepository
    {
        public CompanyRepository(MongoUrl url) : base(url)
        {
        }

        public Task<CompanyEntity> FindByUserAsync(string userId)
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new ArgumentNullException("userId");
                }

                return Context.SingleOrDefault(s => s.Users.Contains(userId));
            });
        }

        public Task<CompanyEntity> FindByCustomerAsync(string customerId)
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrEmpty(customerId))
                {
                    throw new ArgumentNullException("customerId");
                }

                return Context.SingleOrDefault(c => c.BillingCustomerId == customerId);
            });
        }

        public Task SetSubscriptionLastSyncDateAsync(string subscriptionId, DateTime date)
        {
            return Task.Run(() =>
            {
                string subscriptionsCollectionName = NameOfHelper.PropertyName<CompanyEntity>(x => x.Subscriptions);
                string lastSyncDatePropertyName = NameOfHelper.PropertyName<SubscriptionEntity>(x => x.LastSyncDate);

                IMongoQuery query = Query.EQ(string.Format("{0}._id", subscriptionsCollectionName), subscriptionId);
                UpdateBuilder update = Update.Set(string.Format("{0}.$.{1}", subscriptionsCollectionName, lastSyncDatePropertyName), date);

                Collection.Update(query, update);
            });
        }

        public Task SetSubscriptionLastCycleDateAsync(string subscriptionId, DateTime date)
        {
            return Task.Run(() =>
            {
                string subscriptionsCollectionName = NameOfHelper.PropertyName<CompanyEntity>(x => x.Subscriptions);
                string lastCycleDatePropertyName = NameOfHelper.PropertyName<SubscriptionEntity>(x => x.LastCycleDate);

                IMongoQuery query = Query.EQ(string.Format("{0}._id", subscriptionsCollectionName), subscriptionId);
                UpdateBuilder update = Update.Set(string.Format("{0}.$.{1}", subscriptionsCollectionName, lastCycleDatePropertyName), date);

                Collection.Update(query, update);
            });
        }

        public Task SetSubscriptionHasTrialClicksAsync(string subscriptionId, bool value)
        {
            return Task.Run(() =>
            {
                string subscriptionsCollectionName = NameOfHelper.PropertyName<CompanyEntity>(x => x.Subscriptions);
                string hasTrialClicksPropertyName = NameOfHelper.PropertyName<SubscriptionEntity>(x => x.HasTrialClicks);

                IMongoQuery query = Query.EQ(string.Format("{0}._id", subscriptionsCollectionName), subscriptionId);
                UpdateBuilder update = Update.Set(string.Format("{0}.$.{1}", subscriptionsCollectionName, hasTrialClicksPropertyName), value);

                Collection.Update(query, update);
            });
        }

        public Task UpdateUserIdFromAsync(string userId, string toUserId)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query<CompanyEntity>.EQ(c => c.Users, userId);

                string usersCollectionName = NameOfHelper.PropertyName<CompanyEntity>(x => x.Users);
                UpdateBuilder update = Update.Set(string.Format("{0}.$", usersCollectionName), BsonDocumentWrapper.Create(toUserId));

                Collection.Update(query, update, UpdateFlags.Multi);
            });
        }

        public Task<CompanyEntity> FindBySubscriptionAsync(string subscriptionId)
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrEmpty(subscriptionId))
                {
                    throw new ArgumentNullException("subscriptionId");
                }

                return Context.SingleOrDefault(c => c.Subscriptions.Any(s => s.Id == subscriptionId));
            });
        }
    }
}