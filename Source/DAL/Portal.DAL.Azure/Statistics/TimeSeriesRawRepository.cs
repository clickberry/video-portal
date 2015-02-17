// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Data.Linq;
using Portal.Common.Helpers;
using Portal.DAL.Entities.Statistics;
using Portal.DAL.Statistics;
using Portal.Mappers;

namespace Portal.DAL.Azure.Statistics
{
    public class TimeSeriesRawRepository : ITimeSeriesRawRepository
    {
        private readonly Lazy<PreparedStatement> _getStatement;
        private readonly Lazy<PreparedStatement> _insertStatement;

        private readonly IMapper _mapper;
        private readonly ICassandraSession _session;

        public TimeSeriesRawRepository(ICassandraSession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;

            // Preparing statements
            TableAttribute tableAttribute = typeof (TimeSeriesRawEntity).GetCustomAttributes(typeof (TableAttribute), true).Select(a => (TableAttribute)a).First();
            string entityName = tableAttribute.Name;

            string rowKeyPropertyName = NameOfHelper.PropertyName<TimeSeriesRawEntity>(x => x.EventIdYymmddhh);
            string dateTimePropertyName = NameOfHelper.PropertyName<TimeSeriesRawEntity>(x => x.DateTime);
            string payloadPropertyName = NameOfHelper.PropertyName<TimeSeriesRawEntity>(x => x.Payload);

            _insertStatement =
                new Lazy<PreparedStatement>(
                    () =>
                        _session.Get().Prepare(string.Format("INSERT INTO \"{0}\" (\"{1}\",\"{2}\",\"{3}\") VALUES(?,?,?)", entityName, rowKeyPropertyName, dateTimePropertyName, payloadPropertyName)));
            _getStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("SELECT * FROM \"{0}\" WHERE \"{1}\" = ? AND \"{2}\" >= ? AND \"{2}\" <= ?", entityName, rowKeyPropertyName, dateTimePropertyName)));
        }

        public Task AddHourlyAsync(TimeSeriesRawInsertOptions options)
        {
            string rowKey = BuildRowKey(options.EventId, options.DateTime);
            BoundStatement boundStatement = _insertStatement.Value.Bind(rowKey, options.DateTime, options.Payload);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        public async Task<IEnumerable<TimeSeriesRawEntity>> GetHourlySequenceAsync(string eventId, DateTime fromTime, DateTime toTime, int pageSize)
        {
            if ((toTime - fromTime).Hours >= 1)
            {
                throw new NotSupportedException("Date range should be fully included in 1-hour bucket");
            }

            string rowKey = BuildRowKey(eventId, fromTime);
            BoundStatement boundStatement = _getStatement.Value.Bind(rowKey, fromTime, toTime);

            // setting properties
            boundStatement.SetPageSize(pageSize);

            RowSet rowset = await _session.Get().ExecuteAsync(boundStatement);

            return rowset.Select(r => _mapper.Map<Row, TimeSeriesRawEntity>(r));
        }

        #region helpers

        private static string BuildRowKey(string eventId, DateTimeOffset dateTime)
        {
            return eventId + "|" + dateTime.ToString("yyMMddhh");
        }

        #endregion
    }
}