// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using MongoRepository;
using Portal.DAL.Entities.AggregateObject;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Comment
{
    public interface ICommentRepository : IRepository<CommentEntity>
    {
        Task<List<CommentEntity>> GetCommentsAsync(string projectId);

        Task<Dictionary<string, List<CommentEntity>>> GetRecentCommentsByProjectsAsync(string[] projectIds, int? commentsLimit);

        Task<int> GetCommentsCountByProjectAsync(string projectId);

        Task<List<CommentsAggregation>> GetCommentsCountByProjectsAsync(string[] projectIds);

        Task UpdateUserIdFromAsync(string userId, string toUserId);
    }
}