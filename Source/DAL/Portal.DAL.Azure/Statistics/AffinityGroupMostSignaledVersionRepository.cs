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
    public class AffinityGroupMostSignaledVersionRepository : IAffinityGroupMostSignaledVersionRepository
    {
        private readonly Lazy<PreparedStatement> _getStatement;

        private readonly IMapper _mapper;
        private readonly ICassandraSession _session;
        private readonly Lazy<PreparedStatement> _updateStatement;

        public AffinityGroupMostSignaledVersionRepository(ICassandraSession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;

            // Preparing statements
            TableAttribute tableAttribute = typeof (AffinityGroupMostSignaledVersionEntity).GetCustomAttributes(typeof (TableAttribute), true).Select(a => (TableAttribute)a).First();
            string entityName = tableAttribute.Name;

            string rowKeyPropertyName = NameOfHelper.PropertyName<AffinityGroupMostSignaledVersionEntity>(x => x.AffinityGroupSignalType);
            string versionPropertyName = NameOfHelper.PropertyName<AffinityGroupMostSignaledVersionEntity>(x => x.Version);

            _updateStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("UPDATE \"{0}\" SET \"{1}\" = ? WHERE \"{2}\" = ?", entityName, versionPropertyName, rowKeyPropertyName)));
            _getStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("SELECT * FROM \"{0}\" WHERE \"{1}\" = ?", entityName, rowKeyPropertyName)));
        }


        public async Task<AffinityGroupMostSignaledVersionEntity> GetAsync(string groupId, SignalType signal)
        {
            string rowKey = BuildRowKey(groupId, signal);
            BoundStatement boundStatement = _getStatement.Value.Bind(rowKey);
            RowSet rowset = await _session.Get().ExecuteAsync(boundStatement);

            List<Row> rows = rowset.ToList();
            if (rows.Count == 0)
            {
                return new AffinityGroupMostSignaledVersionEntity
                {
                    AffinityGroupSignalType = rowKey,
                    Version = 0
                };
            }

            Row row = rows.First();
            return _mapper.Map<Row, AffinityGroupMostSignaledVersionEntity>(row);
        }

        public Task UpdateAsync(string groupId, SignalType signal, long version)
        {
            string rowKey = BuildRowKey(groupId, signal);
            BoundStatement boundStatement = _updateStatement.Value.Bind(version, rowKey);
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