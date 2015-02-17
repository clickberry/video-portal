// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoRepository;
using Portal.DAL.Comment;
using Portal.DAL.Entities.AggregateObject;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Azure.Comment
{
    public class CommentRepository : MongoRepository<CommentEntity>, ICommentRepository
    {
        public CommentRepository(MongoUrl url) : base(url)
        {
        }

        public Task<List<CommentEntity>> GetCommentsAsync(string projectId)
        {
            return Task.Run(() => Context.Where(c => c.ProjectId == projectId).ToList());
        }

        public Task<Dictionary<string, List<CommentEntity>>> GetRecentCommentsByProjectsAsync(string[] projectIds, int? commentsLimit = null)
        {
            return Task.Run(() =>
            {
                ILookup<string, CommentEntity> projectComments = Context.Where(p => projectIds.Contains(p.ProjectId)).ToLookup(c => c.ProjectId);

                var recentComments = new Dictionary<string, List<CommentEntity>>();
                foreach (var projectComment in projectComments)
                {
                    IEnumerable<CommentEntity> comments = projectComment.OrderByDescending(c => c.DateTime);
                    if (commentsLimit.HasValue)
                    {
                        comments = comments.Take(commentsLimit.Value);
                    }

                    recentComments.Add(projectComment.Key, comments.ToList());
                }


                return recentComments;
            });
        }

        public Task<int> GetCommentsCountByProjectAsync(string projectId)
        {
            return Task.Run(() => Context.Count(p => p.ProjectId == projectId));
        }

        public Task<List<CommentsAggregation>> GetCommentsCountByProjectsAsync(string[] projectIds)
        {
            return Task.Run(() =>
            {
                var inExpression = new BsonDocument("$in", new BsonArray(projectIds));
                var sumExpression = new BsonDocument("$sum", 1);

                var matchExpression = new BsonDocument("ProjectId", inExpression);
                var groupExpression = new BsonDocument { { "_id", "$ProjectId" }, { "count", sumExpression } };
                var projectExpression = new BsonDocument { { "_id", 0 }, { "CommentsCount", "$count" }, { "ProjectId", "$_id" } };

                List<CommentsAggregation> result = Collection
                    .Aggregate(new AggregateArgs
                    {
                        Pipeline = new List<BsonDocument>
                        {
                            new BsonDocument("$match", matchExpression),
                            new BsonDocument("$group", groupExpression),
                            new BsonDocument("$project", projectExpression)
                        }
                    })
                    .Select(BsonSerializer.Deserialize<CommentsAggregation>)
                    .ToList();

                return result;
            });
        }

        public Task UpdateUserIdFromAsync(string userId, string toUserId)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query<CommentEntity>.EQ(entity => entity.UserId, userId);
                UpdateBuilder<CommentEntity> update = Update<CommentEntity>.Set(entity => entity.UserId, toUserId);

                Collection.Update(query, update, UpdateFlags.Multi);
            });
        }
    }
}