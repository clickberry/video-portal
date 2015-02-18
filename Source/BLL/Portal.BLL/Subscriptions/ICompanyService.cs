// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Domain.SubscriptionContext;

namespace Portal.BLL.Subscriptions
{
    public interface ICompanyService
    {
        Task<DomainCompany> AddAsync(DomainCompany company);

        Task<DomainCompany> GetAsync(string id);

        Task<IEnumerable<DomainCompany>> ListAsync();

        Task<DomainCompany> FindBySubscriptionAsync(string subscriptionId);

        Task<DomainCompany> FindByUserAsync(string userId);

        Task<DomainCompany> FindByCustomerAsync(string customerId);

        Task<DomainCompany> UpdateByUserAsync(string userId, CompanyUpdateOptions update);

        Task DeleteByUserAsync(string userId);

        Task ChargeAsync(CompanyChargeOptions charge);
    }
}