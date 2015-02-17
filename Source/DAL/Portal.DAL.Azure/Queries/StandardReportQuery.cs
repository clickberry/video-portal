// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Azure.Queries
{
    public class StandardReportQuery : IQuery<StandardReportV3Entity>
    {
        public IMongoQuery Create(StandardReportV3Entity entity)
        {
            IMongoQuery query1 = Query<StandardReportV3Entity>.EQ(e => e.Tick, entity.Tick);
            IMongoQuery query2 = Query<StandardReportV3Entity>.EQ(e => e.Interval, entity.Interval);
            IMongoQuery query = Query.And(query1, query2);

            return query;
        }
    }
}