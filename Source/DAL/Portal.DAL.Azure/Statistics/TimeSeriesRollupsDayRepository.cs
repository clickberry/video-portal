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
    public class TimeSeriesRollupsDayRepository : ITimeSeriesRollupsDayRepository
    {
        private readonly Lazy<PreparedStatement> _getStatement;
        private readonly Lazy<PreparedStatement> _incStatement;

        private readonly IMapper _mapper;
        private readonly ICassandraSession _session;

        public TimeSeriesRollupsDayRepository(ICassandraSession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;

            // Preparing statements
            TableAttribute tableAttribute = typeof (TimeSeriesRollupsDayEntity).GetCustomAttributes(typeof (TableAttribute), true).Select(a => (TableAttribute)a).First();
            string entityName = tableAttribute.Name;

            string rowKeyPropertyName = NameOfHelper.PropertyName<TimeSeriesRollupsDayEntity>(x => x.EventId);
            string dayPropertyName = NameOfHelper.PropertyName<TimeSeriesRollupsDayEntity>(x => x.Day);
            string countPropertyName = NameOfHelper.PropertyName<TimeSeriesRollupsDayEntity>(x => x.Count);

            _getStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("SELECT * FROM \"{0}\" WHERE \"{1}\" = ? AND \"{2}\" >= ? AND \"{2}\" <= ?", entityName, rowKeyPropertyName, dayPropertyName)));
            _incStatement =
                new Lazy<PreparedStatement>(
                    () =>
                        _session.Get()
                            .Prepare(string.Format("UPDATE  \"{0}\" SET \"{1}\" = \"{1}\" + 1 WHERE \"{2}\" = ? AND \"{3}\" = ?", entityName, countPropertyName, rowKeyPropertyName, dayPropertyName)));
        }

        public async Task<IEnumerable<TimeSeriesRollupsDayEntity>> GetSequenceAsync(string eventId, DateTime fromTime, DateTime toTime, int pageSize)
        {
            BoundStatement boundStatement = _getStatement.Value.Bind(eventId, fromTime, toTime);

            // setting properties
            boundStatement.SetPageSize(pageSize);

            RowSet rowset = await _session.Get().ExecuteAsync(boundStatement);

            return rowset.Select(r => _mapper.Map<Row, TimeSeriesRollupsDayEntity>(r));
        }

        public Task IncAsync(TimeSeriesRollupsInsertOptions options)
        {
            var day = new DateTime(options.DateTime.Year, options.DateTime.Month, options.DateTime.Day, 0, 0, 0);

            BoundStatement boundStatement = _incStatement.Value.Bind(options.EventId, day);
            return _session.Get().ExecuteAsync(boundStatement);
        }
    }
}