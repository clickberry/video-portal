// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Portal.DAL.Entities.QueryObject;
using Portal.DAL.Entities.Table;
using Portal.DAL.Statistics;
using Portal.Exceptions.CRUD;

namespace Portal.DAL.Azure.Statistics
{
    public sealed class StandardReportRepository : StatisticsRepository<StandardReportV3Entity>, IStandardReportRepository
    {
        public StandardReportRepository(MongoUrl url) : base(url)
        {
        }

        public IEnumerable<StandardReportV3Entity> GetReportEntities(ReportQueryObject queryObject)
        {
            try
            {
                IMongoQuery query1 = StatIntervalQuery(queryObject);
                IMongoQuery query2 = Query<ReportEntity>.EQ(e => e.Interval, queryObject.Interval);
                IMongoQuery query = Query.And(query1, query2);

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

        public Task<StandardReportV3Entity> GetLastReport(ReportQueryObject queryObject)
        {
            return Task.Run(() =>
            {
                try
                {
                    return Context
                        .OfType<StandardReportV3Entity>()
                        .Where(p => p.Interval == queryObject.Interval)
                        .OrderBy(p => p.Tick).FirstOrDefault();
                }
                catch (InvalidOperationException exception)
                {
                    throw new NotFoundException(exception);
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }
            });
        }
    }
}