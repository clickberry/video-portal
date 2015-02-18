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
    public class UserSignalsRepository : IUserSignalsRepository
    {
        private readonly Lazy<PreparedStatement> _deleteStatement;
        private readonly Lazy<PreparedStatement> _getStatement;
        private readonly Lazy<PreparedStatement> _insertStatement;

        private readonly IMapper _mapper;
        private readonly ICassandraSession _session;

        public UserSignalsRepository(ICassandraSession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;

            // Preparing statements
            TableAttribute tableAttribute = typeof (UserSignalsEntity).GetCustomAttributes(typeof (TableAttribute), true).Select(a => (TableAttribute)a).First();
            string entityName = tableAttribute.Name;

            string rowKeyPropertyName = NameOfHelper.PropertyName<UserSignalsEntity>(x => x.UserIdSignalType);
            string isAnticolumnPropertyName = NameOfHelper.PropertyName<UserSignalsEntity>(x => x.IsAnticolumn);
            string dateTimePropertyName = NameOfHelper.PropertyName<UserSignalsEntity>(x => x.DateTime);
            string itemIdPropertyName = NameOfHelper.PropertyName<UserSignalsEntity>(x => x.ItemId);

            _insertStatement =
                new Lazy<PreparedStatement>(
                    () =>
                        _session.Get()
                            .Prepare(string.Format("INSERT INTO \"{0}\" (\"{1}\",\"{2}\",\"{3}\",\"{4}\") VALUES(?,false,?,?)", entityName, rowKeyPropertyName, isAnticolumnPropertyName,
                                dateTimePropertyName, itemIdPropertyName)));
            _getStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("SELECT * FROM \"{0}\" WHERE \"{1}\" = ? AND \"{2}\" = ?", entityName, rowKeyPropertyName, isAnticolumnPropertyName)));
            _deleteStatement =
                new Lazy<PreparedStatement>(
                    () =>
                        _session.Get()
                            .Prepare(string.Format("INSERT INTO \"{0}\" (\"{1}\",\"{2}\",\"{3}\",\"{4}\") VALUES(?,true,?,?)", entityName, rowKeyPropertyName, isAnticolumnPropertyName,
                                dateTimePropertyName, itemIdPropertyName)));
        }


        public Task AddAsync(UserSignalsInsertDeleteOptions insertOptions)
        {
            string rowKey = BuildRowKey(insertOptions.UserId, insertOptions.Signal);
            BoundStatement boundStatement = _insertStatement.Value.Bind(rowKey, insertOptions.DateTime, insertOptions.ItemId);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        public Task<IEnumerable<UserSignalsEntity>> GetAntiSequenceAsync(string userId, SignalType signal, int pageSize)
        {
            return GetSequenceInternalAsync(userId, signal, pageSize, true);
        }

        public Task<IEnumerable<UserSignalsEntity>> GetSequenceAsync(string userId, SignalType signal, int pageSize)
        {
            return GetSequenceInternalAsync(userId, signal, pageSize, false);
        }

        public Task DeleteAsync(UserSignalsInsertDeleteOptions deleteOptions)
        {
            string rowKey = BuildRowKey(deleteOptions.UserId, deleteOptions.Signal);
            BoundStatement boundStatement = _deleteStatement.Value.Bind(rowKey, deleteOptions.DateTime, deleteOptions.ItemId);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        #region helpers

        private static string BuildRowKey(string userId, SignalType signal)
        {
            return userId + "|" + signal;
        }

        private async Task<IEnumerable<UserSignalsEntity>> GetSequenceInternalAsync(string userId, SignalType signal, int pageSize, bool isAntiSequence)
        {
            string rowKey = BuildRowKey(userId, signal);
            BoundStatement boundStatement = _getStatement.Value.Bind(rowKey, isAntiSequence);

            // setting properties
            boundStatement.SetPageSize(pageSize);

            RowSet rowset = await _session.Get().ExecuteAsync(boundStatement);
            return rowset.Select(r => _mapper.Map<Row, UserSignalsEntity>(r));
        }

        #endregion
    }
}