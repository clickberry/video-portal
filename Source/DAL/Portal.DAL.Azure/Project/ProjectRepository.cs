// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using MongoRepository;
using Portal.DAL.Entities.Table;
using Portal.DAL.Project;

namespace Portal.DAL.Azure.Project
{
    public class ProjectRepository : MongoRepository<ProjectEntity>, IProjectRepository
    {
        public ProjectRepository(MongoUrl url) : base(url)
        {
        }

        public Task IncrementHitsCounterAsync(string projectId)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query<ProjectEntity>.EQ(entity => entity.Id, projectId);
                UpdateBuilder<ProjectEntity> update = Update<ProjectEntity>.Inc(entity => entity.HitsCount, 1);

                Collection.Update(query, update);
            });
        }

        public Task UpdateLikesCounterAsync(string projectId, long count)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query<ProjectEntity>.EQ(entity => entity.Id, projectId);
                UpdateBuilder<ProjectEntity> update = Update<ProjectEntity>.Set(entity => entity.LikesCount, count);

                Collection.Update(query, update);
            });
        }

        public Task UpdateDislikesCounterAsync(string projectId, long count)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query<ProjectEntity>.EQ(entity => entity.Id, projectId);
                UpdateBuilder<ProjectEntity> update = Update<ProjectEntity>.Set(entity => entity.DislikesCount, count);

                Collection.Update(query, update);
            });
        }

        public Task IncrementAbuseCounterAsync(string projectId)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query<ProjectEntity>.EQ(entity => entity.Id, projectId);
                UpdateBuilder<ProjectEntity> update = Update<ProjectEntity>.Inc(entity => entity.AbuseCount, 1);

                Collection.Update(query, update);
            });
        }

        public Task<List<ProjectEntity>> GetUserProjectsAsync(string userId)
        {
            return Task.Run(() => Context.Where(p => p.UserId == userId).ToList());
        }

        public Task SetAvsxFileIdAsync(string projectId, string fileId)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query<ProjectEntity>.EQ(entity => entity.Id, projectId);
                UpdateBuilder<ProjectEntity> update = Update<ProjectEntity>.Set(entity => entity.AvsxFileId, fileId);

                Collection.Update(query, update);
            });
        }

        public Task SetScreenshotFileIdAsync(string projectId, string fileId)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query<ProjectEntity>.EQ(entity => entity.Id, projectId);
                UpdateBuilder<ProjectEntity> update = Update<ProjectEntity>.Set(entity => entity.ScreenshotFileId, fileId);

                Collection.Update(query, update);
            });
        }

        public Task SetVideoAsync(string projectId, int videoType, string source, string productName = null)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query<ProjectEntity>.EQ(entity => entity.Id, projectId);
                UpdateBuilder<ProjectEntity> update = Update<ProjectEntity>.Set(entity => entity.VideoType, videoType);

                // Set video source
                if (videoType == 0)
                {
                    update.Set(p => p.OriginalVideoFileId, source)
                        .Set(p => p.VideoSource, null)
                        .Set(p => p.VideoSourceProductName, null);
                }
                else
                {
                    update.Set(p => p.VideoSource, source)
                        .Set(p => p.VideoSourceProductName, productName)
                        .Set(p => p.OriginalVideoFileId, null);
                }

                Collection.Update(query, update);
            });
        }

        public Task<List<ProjectEntity>> GetByIdsAsync(string[] projectIds)
        {
            return Task.Run(() => Context.Where(u => u.Id.In(projectIds)).ToList());
        }

        public Task<List<ProjectEntity>> GetByUserIdsAsync(string[] userIds)
        {
            return Task.Run(() => Context.Where(u => u.UserId.In(userIds)).ToList());
        }

        public Task UpdateUserIdFromAsync(string userId, string toUserId)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query<ProjectEntity>.EQ(entity => entity.UserId, userId);
                UpdateBuilder<ProjectEntity> update = Update<ProjectEntity>.Set(entity => entity.UserId, toUserId);

                Collection.Update(query, update, UpdateFlags.Multi);
            });
        }
    }
}