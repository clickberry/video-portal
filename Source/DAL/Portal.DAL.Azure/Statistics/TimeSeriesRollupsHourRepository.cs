// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

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
    public class TimeSeriesRollupsHourRepository : ITimeSeriesRollupsHourRepository
    {
        private readonly Lazy<PreparedStatement> _getStatement;
        private readonly Lazy<PreparedStatement> _incStatement;

        private readonly IMapper _mapper;
        private readonly ICassandraSession _session;

        public TimeSeriesRollupsHourRepository(ICassandraSession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;

            // Preparing statements
            TableAttribute tableAttribute = typeof (TimeSeriesRollupsHourEntity).GetCustomAttributes(typeof (TableAttribute), true).Select(a => (TableAttribute)a).First();
            string entityName = tableAttribute.Name;

            string rowKeyPropertyName = NameOfHelper.PropertyName<TimeSeriesRollupsHourEntity>(x => x.EventIdDd);
            string hourPropertyName = NameOfHelper.PropertyName<TimeSeriesRollupsHourEntity>(x => x.Hour);
            string countPropertyName = NameOfHelper.PropertyName<TimeSeriesRollupsHourEntity>(x => x.Count);

            _getStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("SELECT * FROM \"{0}\" WHERE \"{1}\" = ? AND \"{2}\" >= ? AND \"{2}\" <= ?", entityName, rowKeyPropertyName, hourPropertyName)));
            _incStatement =
                new Lazy<PreparedStatement>(
                    () =>
                        _session.Get().Prepare(string.Format("UPDATE  \"{0}\" SET \"{1}\" = \"{1}\" + 1 WHERE \"{2}\" = ? AND \"{3}\" = ?", entityName, countPropertyName, rowKeyPropertyName,
                            hourPropertyName)));
        }

        public async Task<IEnumerable<TimeSeriesRollupsHourEntity>> GetSequenceAsync(string eventId, int day, DateTime fromTime, DateTime toTime, int pageSize)
        {
            string rowKey = BuildRowKey(eventId, day);
            BoundStatement boundStatement = _getStatement.Value.Bind(rowKey, fromTime, toTime);

            // setting properties
            boundStatement.SetPageSize(pageSize);

            RowSet rowset = await _session.Get().ExecuteAsync(boundStatement);

            return rowset.Select(r => _mapper.Map<Row, TimeSeriesRollupsHourEntity>(r));
        }

        public Task IncAsync(TimeSeriesRollupsInsertOptions options)
        {
            string rowKey = BuildRowKey(options.EventId, options.DateTime.Day);
            var hour = new DateTime(options.DateTime.Year, options.DateTime.Month, options.DateTime.Day, options.DateTime.Hour, 0, 0);

            BoundStatement boundStatement = _incStatement.Value.Bind(rowKey, hour);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        #region helpers

        private static string BuildRowKey(string eventId, int day)
        {
            return eventId + "|" + day;
        }

        #endregion
    }
}