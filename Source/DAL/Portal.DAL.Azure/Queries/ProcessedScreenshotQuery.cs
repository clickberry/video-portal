// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Azure.Queries
{
    public class ProcessedScreenshotQuery : IQuery<ProcessedScreenshotEntity>
    {
        public IMongoQuery Create(ProcessedScreenshotEntity entity)
        {
            IMongoQuery query1 = Query<ProcessedScreenshotEntity>.EQ(e => e.SourceFileId, entity.SourceFileId);
            IMongoQuery query2 = Query<ProcessedScreenshotEntity>.EQ(e => e.TimeOffset, entity.TimeOffset);
            IMongoQuery query = Query.And(query1, query2);

            return query;
        }
    }
}