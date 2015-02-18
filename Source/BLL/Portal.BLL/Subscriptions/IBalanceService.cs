// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Domain.SubscriptionContext;

namespace Portal.BLL.Subscriptions
{
    public interface IBalanceService
    {
        Task<DomainBalanceHistory> AddAsync(DomainBalanceHistory record);

        Task<IEnumerable<DomainBalanceHistory>> QueryHistoryAsync(string companyId);

        Task<IEnumerable<DomainBalanceHistory>> QueryPaymentsHistoryAsync(string companyId);

        Task<IEnumerable<DomainBalanceHistory>> QueryChargesHistoryAsync(string companyId);

        Task<decimal> GetBalanceAsync(string companyId);

        Task DeleteAsync(string id);
    }
}