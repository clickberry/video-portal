// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoRepository;
using Portal.DAL.Entities.Table;
using Portal.DAL.Subscriptions;

namespace Portal.DAL.Azure.Subscriptions
{
    public class BalanceHistoryRepository : MongoRepository<BalanceHistoryEntity>, IBalanceHistoryRepository
    {
        public BalanceHistoryRepository(MongoUrl url) : base(url)
        {
        }

        public Task<IEnumerable<BalanceHistoryEntity>> FindByCompanyAsync(string companyId)
        {
            return Task.Run(() => Context.Where(b => b.CompanyId == companyId).AsEnumerable());
        }
    }
}