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
    public class TimeSeriesRollupsMinuteRepository : ITimeSeriesRollupsMinuteRepository
    {
        private readonly Lazy<PreparedStatement> _getStatement;
        private readonly Lazy<PreparedStatement> _incStatement;

        private readonly IMapper _mapper;
        private readonly ICassandraSession _session;

        public TimeSeriesRollupsMinuteRepository(ICassandraSession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;

            // Preparing statements
            TableAttribute tableAttribute = typeof (TimeSeriesRollupsMinuteEntity).GetCustomAttributes(typeof (TableAttribute), true).Select(a => (TableAttribute)a).First();
            string entityName = tableAttribute.Name;

            string rowKeyPropertyName = NameOfHelper.PropertyName<TimeSeriesRollupsMinuteEntity>(x => x.EventIdHh);
            string minutePropertyName = NameOfHelper.PropertyName<TimeSeriesRollupsMinuteEntity>(x => x.Minute);
            string countPropertyName = NameOfHelper.PropertyName<TimeSeriesRollupsMinuteEntity>(x => x.Count);

            _getStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("SELECT * FROM \"{0}\" WHERE \"{1}\" = ? AND \"{2}\" >= ? AND \"{2}\" <= ?", entityName, rowKeyPropertyName, minutePropertyName)));
            _incStatement =
                new Lazy<PreparedStatement>(
                    () =>
                        _session.Get().Prepare(string.Format("UPDATE  \"{0}\" SET \"{1}\" = \"{1}\" + 1 WHERE \"{2}\" = ? AND \"{3}\" = ?", entityName, countPropertyName, rowKeyPropertyName,
                            minutePropertyName)));
        }

        public async Task<IEnumerable<TimeSeriesRollupsMinuteEntity>> GetSequenceAsync(string eventId, int hour, DateTime fromTime, DateTime toTime, int pageSize)
        {
            string rowKey = BuildRowKey(eventId, hour);
            BoundStatement boundStatement = _getStatement.Value.Bind(rowKey, fromTime, toTime);

            // setting properties
            boundStatement.SetPageSize(pageSize);

            RowSet rowset = await _session.Get().ExecuteAsync(boundStatement);

            return rowset.Select(r => _mapper.Map<Row, TimeSeriesRollupsMinuteEntity>(r));
        }

        public Task IncAsync(TimeSeriesRollupsInsertOptions options)
        {
            string rowKey = BuildRowKey(options.EventId, options.DateTime.Hour);
            var minute = new DateTime(options.DateTime.Year, options.DateTime.Month, options.DateTime.Day, options.DateTime.Hour, options.DateTime.Minute, 0);

            BoundStatement boundStatement = _incStatement.Value.Bind(rowKey, minute);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        #region helpers

        private static string BuildRowKey(string eventId, int hour)
        {
            return eventId + "|" + hour;
        }

        #endregion
    }
}