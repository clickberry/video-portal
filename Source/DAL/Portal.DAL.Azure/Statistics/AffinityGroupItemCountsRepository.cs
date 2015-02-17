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
    public class AffinityGroupItemCountsRepository : IAffinityGroupItemCountsRepository
    {
        private readonly Lazy<PreparedStatement> _decStatement;
        private readonly Lazy<PreparedStatement> _getByIdStatement;
        private readonly Lazy<PreparedStatement> _getStatement;
        private readonly Lazy<PreparedStatement> _incStatement;

        private readonly IMapper _mapper;
        private readonly ICassandraSession _session;

        public AffinityGroupItemCountsRepository(ICassandraSession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;

            // Preparing statements
            TableAttribute tableAttribute = typeof (AffinityGroupItemCountsEntity).GetCustomAttributes(typeof (TableAttribute), true).Select(a => (TableAttribute)a).First();
            string entityName = tableAttribute.Name;

            string rowKeyPropertyName = NameOfHelper.PropertyName<AffinityGroupItemCountsEntity>(x => x.AffinityGroupSignalType);
            string itemIdPropertyName = NameOfHelper.PropertyName<AffinityGroupItemCountsEntity>(x => x.ItemId);
            string countPropertyName = NameOfHelper.PropertyName<AffinityGroupItemCountsEntity>(x => x.Count);

            _getByIdStatement =
                new Lazy<PreparedStatement>(() => _session.Get().Prepare(string.Format("SELECT * FROM \"{0}\" WHERE \"{1}\" = ? AND \"{2}\" = ?", entityName, rowKeyPropertyName, itemIdPropertyName)));
            _getStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("SELECT * FROM \"{0}\" WHERE \"{1}\" = ?", entityName, rowKeyPropertyName)));
            _incStatement =
                new Lazy<PreparedStatement>(
                    () =>
                        _session.Get()
                            .Prepare(string.Format("UPDATE  \"{0}\" SET \"{1}\" = \"{1}\" + 1 WHERE \"{2}\" = ? AND \"{3}\" = ?", entityName, countPropertyName, rowKeyPropertyName, itemIdPropertyName)));
            _decStatement =
                new Lazy<PreparedStatement>(
                    () =>
                        _session.Get()
                            .Prepare(string.Format("UPDATE  \"{0}\" SET \"{1}\" = \"{1}\" - 1 WHERE \"{2}\" = ? AND \"{3}\" = ?", entityName, countPropertyName, rowKeyPropertyName, itemIdPropertyName)));
        }

        public async Task<AffinityGroupItemCountsEntity> GetAsync(string groupId, SignalType signal, string itemId)
        {
            string rowId = BuildRowKey(groupId, signal);
            BoundStatement boundStatement = _getByIdStatement.Value.Bind(rowId, itemId);

            RowSet rowset = await _session.Get().ExecuteAsync(boundStatement);

            List<Row> rows = rowset.ToList();
            if (rows.Count == 0)
            {
                return new AffinityGroupItemCountsEntity
                {
                    AffinityGroupSignalType = rowId,
                    ItemId = itemId
                };
            }

            Row row = rows.First();
            return _mapper.Map<Row, AffinityGroupItemCountsEntity>(row);
        }

        public async Task<IEnumerable<AffinityGroupItemCountsEntity>> GetSequenceAsync(string groupId, SignalType signal, int pageSize)
        {
            string rowKey = BuildRowKey(groupId, signal);
            BoundStatement boundStatement = _getStatement.Value.Bind(rowKey);

            // setting properties
            boundStatement.SetPageSize(pageSize);

            RowSet rowset = await _session.Get().ExecuteAsync(boundStatement);
            return rowset.Select(r => _mapper.Map<Row, AffinityGroupItemCountsEntity>(r));
        }

        public Task IncAsync(string groupId, SignalType signal, string itemId)
        {
            string rowId = BuildRowKey(groupId, signal);
            BoundStatement boundStatement = _incStatement.Value.Bind(rowId, itemId);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        public Task DecAsync(string groupId, SignalType signal, string itemId)
        {
            string rowId = BuildRowKey(groupId, signal);
            BoundStatement boundStatement = _decStatement.Value.Bind(rowId, itemId);
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