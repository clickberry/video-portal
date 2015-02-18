// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Domain.Admin;

namespace Portal.BLL.Services
{
    public interface IAdminClientSubscriptionService
    {
        Task<IEnumerable<DomainAdminClientSubscription>> GetSubscriptionsAsync(string clientId);

        Task<DomainAdminClientSubscription> EditSubscriptionAsync(string clientId, DomainAdminClientSubscription domainSubscription);
    }
}