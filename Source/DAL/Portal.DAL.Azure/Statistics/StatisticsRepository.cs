// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoRepository;
using Portal.DAL.Azure.Context;
using Portal.DAL.Entities.QueryObject;
using Portal.DAL.Entities.Table;
using Portal.DAL.Statistics;
using Portal.Exceptions.CRUD;

namespace Portal.DAL.Azure.Statistics
{
    public class StatisticsRepository<T> : MongoTableRepository<T>, IStatisticsRepository<T> where T : class, IEntity
    {
        public StatisticsRepository(MongoUrl url) : base(url)
        {
        }

        public IEnumerable<T> GetStatEntities(StatQueryObject queryObject)
        {
            try
            {
                IMongoQuery query = StatIntervalQuery(queryObject);

                return Collection.Find(query).AsEnumerable();
            }
            catch (InvalidOperationException exception)
            {
                throw new NotFoundException(exception);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Mongo driver failure.", e);
            }
        }

        protected IMongoQuery StatIntervalQuery(StatQueryObject queryObject)
        {
            IMongoQuery query1 = queryObject.IsStartInclude
                ? Query<StatEntity>.LTE(e => e.Tick, queryObject.StartInterval)
                : Query<StatEntity>.LT(e => e.Tick, queryObject.StartInterval);

            IMongoQuery query2 = queryObject.IsEndInclude
                ? Query<StatEntity>.GTE(e => e.Tick, queryObject.EndInterval)
                : Query<StatEntity>.GT(e => e.Tick, queryObject.EndInterval);

            IMongoQuery query = Query.And(query1, query2);
            return query;
        }
    }
}