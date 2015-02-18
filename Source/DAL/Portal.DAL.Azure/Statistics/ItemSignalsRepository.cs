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
    public class ItemSignalsRepository : IItemSignalsRepository
    {
        private readonly Lazy<PreparedStatement> _deleteStatement;
        private readonly Lazy<PreparedStatement> _getStatement;
        private readonly Lazy<PreparedStatement> _insertStatement;

        private readonly IMapper _mapper;
        private readonly ICassandraSession _session;

        public ItemSignalsRepository(ICassandraSession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;

            // Preparing statements
            TableAttribute tableAttribute = typeof (ItemSignalsEntity).GetCustomAttributes(typeof (TableAttribute), true).Select(a => (TableAttribute)a).First();
            string entityName = tableAttribute.Name;

            string rowKeyPropertyName = NameOfHelper.PropertyName<ItemSignalsEntity>(x => x.ItemIdSignalType);
            string isAnticolumnPropertyName = NameOfHelper.PropertyName<ItemSignalsEntity>(x => x.IsAnticolumn);
            string userIdPropertyName = NameOfHelper.PropertyName<ItemSignalsEntity>(x => x.UserId);
            string dateTimePropertyName = NameOfHelper.PropertyName<ItemSignalsEntity>(x => x.DateTime);

            _insertStatement =
                new Lazy<PreparedStatement>(
                    () =>
                        _session.Get()
                            .Prepare(string.Format("INSERT INTO \"{0}\" (\"{1}\",\"{2}\",\"{3}\",\"{4}\") VALUES(?,false,?,?)", entityName, rowKeyPropertyName, isAnticolumnPropertyName,
                                userIdPropertyName,
                                dateTimePropertyName)));
            _getStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("SELECT * FROM \"{0}\" WHERE \"{1}\" = ? AND \"{2}\" = ?", entityName, rowKeyPropertyName, isAnticolumnPropertyName)));
            _deleteStatement =
                new Lazy<PreparedStatement>(
                    () =>
                        _session.Get()
                            .Prepare(string.Format("INSERT INTO \"{0}\" (\"{1}\",\"{2}\",\"{3}\",\"{4}\") VALUES(?,true,?,?)", entityName, rowKeyPropertyName, isAnticolumnPropertyName,
                                userIdPropertyName,
                                dateTimePropertyName)));
        }


        public Task AddAsync(ItemSignalsInsertDeleteOptions options)
        {
            string rowKey = BuildRowKey(options.ItemId, options.Signal);
            BoundStatement boundStatement = _insertStatement.Value.Bind(rowKey, options.UserId, options.DateTime);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        public Task<IEnumerable<ItemSignalsEntity>> GetAntiSequenceAsync(string itemId, SignalType signal, int pageSize)
        {
            return GetSequenceInternalAsync(itemId, signal, pageSize, true);
        }

        public Task<IEnumerable<ItemSignalsEntity>> GetSequenceAsync(string itemId, SignalType signal, int pageSize)
        {
            return GetSequenceInternalAsync(itemId, signal, pageSize, false);
        }

        public Task DeleteAsync(ItemSignalsInsertDeleteOptions options)
        {
            string rowKey = BuildRowKey(options.ItemId, options.Signal);
            BoundStatement boundStatement = _deleteStatement.Value.Bind(rowKey, options.UserId, options.DateTime);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        #region helpers

        private static string BuildRowKey(string itemId, SignalType signal)
        {
            return itemId + "|" + signal;
        }

        private async Task<IEnumerable<ItemSignalsEntity>> GetSequenceInternalAsync(string itemId, SignalType signal, int pageSize, bool isAntiSequence)
        {
            string rowKey = BuildRowKey(itemId, signal);
            BoundStatement boundStatement = _getStatement.Value.Bind(rowKey, isAntiSequence);

            // setting properties
            boundStatement.SetPageSize(pageSize);

            RowSet rowset = await _session.Get().ExecuteAsync(boundStatement);
            return rowset.Select(r => _mapper.Map<Row, ItemSignalsEntity>(r));
        }

        #endregion
    }
}