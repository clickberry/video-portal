// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.Domain.BillingContext;

namespace Portal.BLL.Billing
{
    public interface IBillingEventLogService
    {
        Task<bool> ExistsAsync(DomainEvent e);

        Task<DomainEvent> AddAsync(DomainEvent e);

        Task DeleteAsync(DomainEvent e);
    }
}