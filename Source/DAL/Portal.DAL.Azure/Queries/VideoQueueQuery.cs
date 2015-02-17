// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Azure.Queries
{
    public class VideoQueueQuery : IQuery<VideoQueueEntity>
    {
        public IMongoQuery Create(VideoQueueEntity entity)
        {
            IMongoQuery query = Query<VideoQueueEntity>.EQ(e => e.FileId, entity.FileId);
            return query;
        }
    }
}