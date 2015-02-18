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
    public class AffinityGroupMostSignaledRepository : IAffinityGroupMostSignaledRepository
    {
        private readonly Lazy<PreparedStatement> _deleteStatement;
        private readonly Lazy<PreparedStatement> _getStatement;
        private readonly Lazy<PreparedStatement> _insertStatement;

        private readonly IMapper _mapper;
        private readonly ICassandraSession _session;

        public AffinityGroupMostSignaledRepository(ICassandraSession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;

            // Preparing statements
            TableAttribute tableAttribute = typeof (AffinityGroupMostSignaledEntity).GetCustomAttributes(typeof (TableAttribute), true).Select(a => (TableAttribute)a).First();
            string entityName = tableAttribute.Name;

            string rowKeyPropertyName = NameOfHelper.PropertyName<AffinityGroupMostSignaledEntity>(x => x.AffinityGroupSignalType);
            string countPropertyName = NameOfHelper.PropertyName<AffinityGroupMostSignaledEntity>(x => x.Count);
            string itemIdPropertyName = NameOfHelper.PropertyName<AffinityGroupMostSignaledEntity>(x => x.ItemId);

            _insertStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("INSERT INTO \"{0}\" (\"{1}\",\"{2}\",\"{3}\") VALUES(?,?,?)", entityName, rowKeyPropertyName, countPropertyName, itemIdPropertyName)));
            _getStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("SELECT * FROM \"{0}\" WHERE \"{1}\" = ? ORDER BY \"{2}\" DESC", entityName, rowKeyPropertyName, countPropertyName)));
            _deleteStatement =
                new Lazy<PreparedStatement>(
                    () =>
                        _session.Get()
                            .Prepare(string.Format("DELETE FROM \"{0}\" WHERE \"{1}\" = ?", entityName, rowKeyPropertyName)));
        }

        public Task AddAsync(string groupId, SignalType signal, long version, List<AffinityGroupMostSignaledInsertOptions> options)
        {
            string rowKey = BuildRowKey(groupId, signal, version);

            var batch = new BatchStatement();
            foreach (var option in options)
            {
                BoundStatement boundStatement = _insertStatement.Value.Bind(rowKey, option.Count, option.ItemId);
                batch.Add(boundStatement);
            }

            return _session.Get().ExecuteAsync(batch.SetConsistencyLevel(ConsistencyLevel.One));
        }

        public async Task<IEnumerable<AffinityGroupMostSignaledEntity>> GetSequenceAsync(string groupId, SignalType signal, long version, int pageSize)
        {
            string rowKey = BuildRowKey(groupId, signal, version);
            BoundStatement boundStatement = _getStatement.Value.Bind(rowKey);

            // setting properties
            boundStatement.SetPageSize(pageSize);

            RowSet rowset = await _session.Get().ExecuteAsync(boundStatement.SetConsistencyLevel(ConsistencyLevel.One));
            return rowset.Select(r => _mapper.Map<Row, AffinityGroupMostSignaledEntity>(r));
        }

        public Task DeleteVersionAsync(string groupId, SignalType signal, long version)
        {
            string rowKey = BuildRowKey(groupId, signal, version);
            BoundStatement boundStatement = _deleteStatement.Value.Bind(rowKey);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        #region helpers

        private static string BuildRowKey(string groupId, SignalType signal, long version)
        {
            return groupId + "|" + signal + "|" + version;
        }

        #endregion
    }
}