// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using MongoRepository;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Subscriptions
{
    public interface IBalanceHistoryRepository : IRepository<BalanceHistoryEntity>
    {
        Task<IEnumerable<BalanceHistoryEntity>> FindByCompanyAsync(string companyId);
    }
}