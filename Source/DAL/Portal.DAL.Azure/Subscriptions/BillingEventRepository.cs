// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MongoDB.Driver;
using MongoRepository;
using Portal.DAL.Entities.Table;
using Portal.DAL.Subscriptions;

namespace Portal.DAL.Azure.Subscriptions
{
    public class BillingEventRepository : MongoRepository<BillingEventEntity>, IBillingEventRepository
    {
        public BillingEventRepository(MongoUrl url) : base(url)
        {
        }
    }
}