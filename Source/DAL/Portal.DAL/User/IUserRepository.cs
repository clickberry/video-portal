// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using MongoRepository;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.User
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<UserEntity> FindByIdentityAsync(string identityProvider, string userIdentity);

        Task<List<UserEntity>> GetByIdsAsync(string[] userIds);

        Task<List<UserEntity>> GetUsersInRoleAsync(string role);

        Task<List<UserEntity>> FindByNameAsync(string name);

        Task<UserEntity> FindByEmailAsync(string email);

        Task<UserEntity> FindByEmailAsync(string appName, string email);

        Task<List<UserEntity>> GetUsersByIdsAsync(string[] userIds);

        Task AddMembershipAsync(string userId, UserMembershipEntity membership);

        Task UpdateMembershipAsync(string userId, UserMembershipEntity membership);

        Task DeleteMembershipAsync(string userId, string identityProvider);

        Task ChangeEmailAsync(string userId, string email);

        Task AddFollowerAsync(string userId, FollowerEntity follower);

        Task DeleteFollowerAsync(string userId, FollowerEntity follower);

        Task<List<FollowerEntity>> FindFollowingUsersAsync(string userId);
    }
}