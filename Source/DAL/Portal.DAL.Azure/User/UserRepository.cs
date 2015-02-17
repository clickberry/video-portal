// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using MongoRepository;
using Portal.Common.Helpers;
using Portal.DAL.Entities.Table;
using Portal.DAL.User;
using Portal.Exceptions.CRUD;

namespace Portal.DAL.Azure.User
{
    public sealed class UserRepository : MongoRepository<UserEntity>, IUserRepository
    {
        public UserRepository(MongoUrl url) : base(url)
        {
        }

        public Task<UserEntity> FindByIdentityAsync(string identityProvider, string userIdentity)
        {
            return Task.Run(() => Context.SingleOrDefault(p => p.Memberships.Any(
                q => q.IdentityProvider == identityProvider && q.UserIdentifier == userIdentity)));
        }

        public Task<List<UserEntity>> GetByIdsAsync(string[] userIds)
        {
            return Task.Run(() => Context.Where(u => u.Id.In(userIds)).ToList());
        }

        public Task<List<UserEntity>> GetUsersInRoleAsync(string role)
        {
            return Task.Run(() => Context.Where(p => p.Roles.Contains(role)).ToList());
        }

        public Task<List<UserEntity>> FindByNameAsync(string name)
        {
            return Task.Run(() => Collection.Find(Query.Text(name.ToLowerInvariant())).ToList());
        }

        public Task<UserEntity> FindByEmailAsync(string email)
        {
            return Task.Run(() => Context.SingleOrDefault(p => p.Email == email));
        }

        public Task<UserEntity> FindByEmailAsync(string appName, string email)
        {
            return Task.Run(() => Context.SingleOrDefault(p => p.AppName == appName && p.Email == email));
        }

        public Task<List<UserEntity>> GetUsersByIdsAsync(string[] userIds)
        {
            return Task.Run(() => Context.Where(u => u.Id.In(userIds)).ToList());
        }

        public Task AddMembershipAsync(string userId, UserMembershipEntity membership)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query<UserEntity>.EQ(entity => entity.Id, userId);
                UpdateBuilder<UserEntity> update = Update<UserEntity>.AddToSet(entity => entity.Memberships, membership);

                Collection.Update(query, update);
            });
        }

        public Task UpdateMembershipAsync(string userId, UserMembershipEntity membership)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query.And(
                    Query<UserEntity>.EQ(e => e.Id, userId),
                    Query<UserEntity>.ElemMatch(e => e.Memberships, builder => builder.EQ(m => m.IdentityProvider, membership.IdentityProvider)),
                    Query<UserEntity>.ElemMatch(e => e.Memberships, builder => builder.EQ(m => m.UserIdentifier, membership.UserIdentifier)));

                string membershipsCollectionName = NameOfHelper.PropertyName<UserEntity>(x => x.Memberships);
                UpdateBuilder update = Update.Set(string.Format("{0}.$", membershipsCollectionName), BsonDocumentWrapper.Create(membership));

                Collection.Update(query, update);
            });
        }

        public Task DeleteMembershipAsync(string userId, string identityProvider)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query.And(
                    Query<UserEntity>.EQ(e => e.Id, userId),
                    Query<UserEntity>.ElemMatch(e => e.Memberships, builder => builder.EQ(m => m.IdentityProvider, identityProvider)));

                UpdateBuilder<UserEntity> pull = Update<UserEntity>.Pull(u => u.Memberships, builder => builder.EQ(m => m.IdentityProvider, identityProvider));

                WriteConcernResult result = Collection.Update(query, pull);
                if (result.DocumentsAffected == 0)
                {
                    throw new NotFoundException(string.Format("Could not find identityProvider '{0}', for userId {1}.", identityProvider, userId));
                }
            });
        }

        public Task ChangeEmailAsync(string userId, string email)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query<UserEntity>.EQ(u => u.Id, userId);
                UpdateBuilder<UserEntity> update = Update<UserEntity>.Set(u => u.Email, email);

                try
                {
                    WriteConcernResult result = Collection.Update(query, update);
                    CheckWriteResult(result, true);
                }
                catch (MongoWriteConcernException e)
                {
                    throw HandleWriteException(e);
                }
            });
        }

        public Task AddFollowerAsync(string userId, FollowerEntity follower)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query<UserEntity>.EQ(entity => entity.Id, userId);
                UpdateBuilder<UserEntity> update = Update<UserEntity>.AddToSet(entity => entity.Followers, follower);

                Collection.Update(query, update);
            });
        }

        public Task DeleteFollowerAsync(string userId, FollowerEntity follower)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query.And(
                    Query<UserEntity>.EQ(e => e.Id, userId),
                    Query<UserEntity>.ElemMatch(e => e.Followers, builder => builder.EQ(m => m.Id, follower.Id)));

                UpdateBuilder<UserEntity> pull = Update<UserEntity>.Pull(u => u.Followers, builder => builder.EQ(m => m.Id, follower.Id));

                WriteConcernResult result = Collection.Update(query, pull);
                if (result.DocumentsAffected == 0)
                {
                    throw new NotFoundException(string.Format("Could not find follower user '{0}', for userId {1}.", follower.Id, userId));
                }
            });
        }

        public Task<List<FollowerEntity>> FindFollowingUsersAsync(string userId)
        {
            return Task.Run(() => Context.Where(u => u.Followers.Any(
                f => f.Id == userId)).Select(u => new FollowerEntity { Id = u.Id, Name = u.Name }).ToList());
        }
    }
}