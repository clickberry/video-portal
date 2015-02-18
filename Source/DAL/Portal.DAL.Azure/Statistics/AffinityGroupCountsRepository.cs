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
    public class AffinityGroupCountsRepository : IAffinityGroupCountsRepository
    {
        private readonly Lazy<PreparedStatement> _decStatement;
        private readonly Lazy<PreparedStatement> _getStatement;
        private readonly Lazy<PreparedStatement> _incStatement;

        private readonly IMapper _mapper;
        private readonly ICassandraSession _session;

        public AffinityGroupCountsRepository(ICassandraSession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;

            // Preparing statements
            TableAttribute tableAttribute = typeof (AffinityGroupCountsEntity).GetCustomAttributes(typeof (TableAttribute), true).Select(a => (TableAttribute)a).First();
            string entityName = tableAttribute.Name;

            string rowId = NameOfHelper.PropertyName<AffinityGroupCountsEntity>(x => x.AffinityGroupSignalType);
            string countPropertyName = NameOfHelper.PropertyName<AffinityGroupCountsEntity>(x => x.Count);

            _getStatement = new Lazy<PreparedStatement>(() => _session.Get().Prepare(string.Format("SELECT * FROM \"{0}\" WHERE \"{1}\" = ?", entityName, rowId)));
            _incStatement =
                new Lazy<PreparedStatement>(() => _session.Get().Prepare(string.Format("UPDATE  \"{0}\" SET \"{2}\" = \"{2}\" + 1 WHERE \"{1}\" = ?", entityName, rowId, countPropertyName)));
            _decStatement =
                new Lazy<PreparedStatement>(() => _session.Get().Prepare(string.Format("UPDATE  \"{0}\" SET \"{2}\" = \"{2}\" - 1 WHERE \"{1}\" = ?", entityName, rowId, countPropertyName)));
        }

        public async Task<AffinityGroupCountsEntity> GetAsync(string groupId, SignalType signal)
        {
            string rowId = BuildRowKey(groupId, signal);
            BoundStatement boundStatement = _getStatement.Value.Bind(rowId);
            RowSet rowset = await _session.Get().ExecuteAsync(boundStatement);

            List<Row> rows = rowset.ToList();
            if (rows.Count == 0)
            {
                return new AffinityGroupCountsEntity
                {
                    AffinityGroupSignalType = rowId
                };
            }

            Row row = rows.First();
            return _mapper.Map<Row, AffinityGroupCountsEntity>(row);
        }

        public Task IncAsync(string groupId, SignalType signal)
        {
            string rowId = BuildRowKey(groupId, signal);
            BoundStatement boundStatement = _incStatement.Value.Bind(rowId);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        public Task DecAsync(string groupId, SignalType signal)
        {
            string rowId = BuildRowKey(groupId, signal);
            BoundStatement boundStatement = _decStatement.Value.Bind(rowId);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        #region helpers

        private static string BuildRowKey(string groupId, SignalType signal)
        {
            return groupId + "|" + signal;
        }

        #endregion
    }
}