// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using MongoRepository;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Project
{
    public interface IProjectRepository : IRepository<ProjectEntity>
    {
        Task IncrementHitsCounterAsync(string projectId);

        Task UpdateLikesCounterAsync(string projectId, long count);

        Task UpdateDislikesCounterAsync(string projectId, long count);

        Task IncrementAbuseCounterAsync(string projectId);

        Task<List<ProjectEntity>> GetUserProjectsAsync(string userId);

        Task SetAvsxFileIdAsync(string projectId, string fileId);

        Task SetScreenshotFileIdAsync(string projectId, string fileId);

        Task SetVideoAsync(string projectId, int videoType, string source, string productName = null);

        Task<List<ProjectEntity>> GetByIdsAsync(string[] projectIds);

        Task<List<ProjectEntity>> GetByUserIdsAsync(string[] userIds);

        Task UpdateUserIdFromAsync(string userId, string toUserId);
    }
}