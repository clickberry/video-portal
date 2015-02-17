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
    public class UserSignalsUnorderedRepository : IUserSignalsUnorderedRepository
    {
        private readonly Lazy<PreparedStatement> _deleteStatement;
        private readonly Lazy<PreparedStatement> _getStatement;
        private readonly Lazy<PreparedStatement> _insertStatement;

        private readonly IMapper _mapper;
        private readonly ICassandraSession _session;

        public UserSignalsUnorderedRepository(ICassandraSession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;

            // Preparing statements
            TableAttribute tableAttribute = typeof (UserSignalsUnorderedEntity).GetCustomAttributes(typeof (TableAttribute), true).Select(a => (TableAttribute)a).First();
            string entityName = tableAttribute.Name;

            string rowKeyPropertyName = NameOfHelper.PropertyName<UserSignalsUnorderedEntity>(x => x.UserIdSignalType);
            string isAnticolumnPropertyName = NameOfHelper.PropertyName<UserSignalsUnorderedEntity>(x => x.IsAnticolumn);
            string itemIdPropertyName = NameOfHelper.PropertyName<UserSignalsUnorderedEntity>(x => x.ItemId);
            string dateTimePropertyName = NameOfHelper.PropertyName<UserSignalsUnorderedEntity>(x => x.DateTime);

            _insertStatement =
                new Lazy<PreparedStatement>(
                    () =>
                        _session.Get()
                            .Prepare(string.Format("INSERT INTO \"{0}\" (\"{1}\",\"{2}\",\"{3}\",\"{4}\") VALUES(?,false,?,?)", entityName, rowKeyPropertyName, isAnticolumnPropertyName,
                                itemIdPropertyName, dateTimePropertyName)));
            _getStatement =
                new Lazy<PreparedStatement>(
                    () =>
                        _session.Get()
                            .Prepare(string.Format("SELECT * FROM \"{0}\" WHERE \"{1}\" = ? AND \"{2}\" = ? AND \"{3}\" = ?", entityName, rowKeyPropertyName, isAnticolumnPropertyName,
                                itemIdPropertyName)));
            _deleteStatement =
                new Lazy<PreparedStatement>(
                    () =>
                        _session.Get()
                            .Prepare(string.Format("INSERT INTO \"{0}\" (\"{1}\",\"{2}\",\"{3}\",\"{4}\") VALUES(?,true,?,?)", entityName, rowKeyPropertyName, isAnticolumnPropertyName,
                                itemIdPropertyName, dateTimePropertyName)));
        }


        public Task AddAsync(UserSignalsInsertDeleteOptions insertOptions)
        {
            string rowKey = BuildRowKey(insertOptions.UserId, insertOptions.Signal);
            BoundStatement boundStatement = _insertStatement.Value.Bind(rowKey, insertOptions.ItemId, insertOptions.DateTime);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        public async Task<UserSignalsUnorderedEntity> GetAsync(string userId, SignalType signal, string itemId)
        {
            string rowKey = BuildRowKey(userId, signal);
            BoundStatement antiBoundStatement = _getStatement.Value.Bind(rowKey, true, itemId);
            BoundStatement boundStatement = _getStatement.Value.Bind(rowKey, false, itemId);

            RowSet antiRowset = await _session.Get().ExecuteAsync(antiBoundStatement);
            List<UserSignalsUnorderedEntity> antiResults = antiRowset.Select(r => _mapper.Map<Row, UserSignalsUnorderedEntity>(r)).OrderBy(r => r.DateTime).ToList();
            UserSignalsUnorderedEntity lastAntiResult = antiResults.LastOrDefault();

            RowSet rowset = await _session.Get().ExecuteAsync(boundStatement);
            List<UserSignalsUnorderedEntity> results = rowset.Select(r => _mapper.Map<Row, UserSignalsUnorderedEntity>(r)).OrderBy(r => r.DateTime).ToList();
            UserSignalsUnorderedEntity lastResult = results.LastOrDefault();

            if (lastResult == null)
            {
                return null;
            }

            if (lastAntiResult == null)
            {
                return lastResult;
            }

            // Compare dates
            return lastResult.DateTime > lastAntiResult.DateTime ? lastResult : null;
        }

        public Task DeleteAsync(UserSignalsInsertDeleteOptions deleteOptions)
        {
            string rowKey = BuildRowKey(deleteOptions.UserId, deleteOptions.Signal);
            BoundStatement boundStatement = _deleteStatement.Value.Bind(rowKey, deleteOptions.ItemId, deleteOptions.DateTime);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        #region helpers

        private static string BuildRowKey(string userId, SignalType signal)
        {
            return userId + "|" + signal;
        }

        #endregion
    }
}