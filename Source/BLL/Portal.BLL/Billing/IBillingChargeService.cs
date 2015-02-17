// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Domain.BillingContext;

namespace Portal.BLL.Billing
{
    public interface IBillingChargeService
    {
        Task<DomainCharge> GetAsync(DomainCharge charge);

        Task<DomainCharge> AddAsync(DomainChargeCreateOptions options);

        Task<IEnumerable<DomainCharge>> ListAsync(DomainCustomer customer, int count);
    }
}