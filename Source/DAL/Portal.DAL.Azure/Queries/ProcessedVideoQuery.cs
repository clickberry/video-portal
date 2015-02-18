// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Azure.Queries
{
    public class ProcessedVideoQuery : IQuery<ProcessedVideoEntity>
    {
        public IMongoQuery Create(ProcessedVideoEntity entity)
        {
            IMongoQuery query1 = Query<ProcessedVideoEntity>.EQ(e => e.SourceFileId, entity.SourceFileId);
            IMongoQuery query2 = Query<ProcessedVideoEntity>.EQ(e => e.OutputFormat, entity.OutputFormat);
            IMongoQuery query = Query.And(query1, query2);

            return query;
        }
    }
}