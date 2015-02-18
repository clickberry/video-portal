// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Domain;
using Portal.Domain.Admin;
using Portal.Domain.ProfileContext;

namespace Portal.BLL.Services
{
    public interface IAdminUserService
    {
        DataResult<Task<DomainUserForAdmin>> GetAsyncSequence(DataQueryOptions filter);

        Task<DomainUserForAdmin> GetAsync(DomainUserForAdmin user);

        Task SetUserPasswordAsync(string userId, string password);

        Task DeleteAsync(string userId);

        Task<List<DomainUser>> GetUsersInRoleAsync(string role);

        Task<List<DomainUser>> FindByNameAsync(string userName);
    }
}