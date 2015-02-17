// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Azure.Queries
{
    public class PasswordRecoveryQuery : IQuery<PasswordRecoveryEntity>
    {
        public IMongoQuery Create(PasswordRecoveryEntity entity)
        {
            IMongoQuery query1 = Query<PasswordRecoveryEntity>.EQ(e => e.UserId, entity.UserId);
            IMongoQuery query2 = Query<PasswordRecoveryEntity>.EQ(e => e.LinkData, entity.LinkData);
            IMongoQuery query = Query.And(query1, query2);

            return query;
        }
    }
}