// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Domain.ProfileContext;

namespace Portal.BLL.Services
{
    public interface IUserService
    {
        Task<DomainUser> GetAsync(string id);

        Task<List<DomainUser>> GetByIdsAsync(string[] ids);

        Task<DomainUser> FindByEmailAsync(string email);

        Task<DomainUser> AddAsync(DomainUser entity);

        Task AddRoleAsync(string userId, string role);

        Task DeleteRoleAsync(string userId, string role);

        Task<DomainUser> UpdateAsync(string userId, UserUpdateOptions update);

        Task<DomainUser> FindByIdentityAsync(ProviderType provider, string userIdentifier);

        Task<DomainUser> CheckCredentialsAsync(string email, string password);

        Task ChangeEmailAsync(string userId, string email);

        Task AddMembersipAsync(string userId, TokenData tokenData);

        Task DeleteMembersipAsync(string userId, ProviderType provider);

        Task DeleteAsync(string userId);

        Task MergeFromAsync(string userId, string toUserId);

        Task AddFollowerAsync(string userId, Follower follower);

        Task DeleteFollowerAsync(string userId, Follower follower);

        Task<List<Follower>> FindFollowingUsersAsync(string userId);
    }
}