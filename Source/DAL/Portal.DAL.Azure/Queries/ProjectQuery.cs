// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Azure.Queries
{
    public class ProjectQuery : IQuery<ProjectEntity>
    {
        public IMongoQuery Create(ProjectEntity entity)
        {
            IMongoQuery query = Query<ProjectEntity>.EQ(e => e.Id, entity.Id);

            return query;
        }
    }
}