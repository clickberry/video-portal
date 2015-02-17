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
    public class UserCountsRepository : IUserCountsRepository
    {
        private readonly Lazy<PreparedStatement> _decDislikesStatement;
        private readonly Lazy<PreparedStatement> _decLikesStatement;
        private readonly Lazy<PreparedStatement> _getStatement;
        private readonly Lazy<PreparedStatement> _incAbuseStatement;
        private readonly Lazy<PreparedStatement> _incDislikesStatement;
        private readonly Lazy<PreparedStatement> _incLikesStatement;
        private readonly Lazy<PreparedStatement> _incViewsStatement;

        private readonly IMapper _mapper;
        private readonly ICassandraSession _session;


        public UserCountsRepository(ICassandraSession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;

            // Preparing statements
            TableAttribute tableAttribute = typeof (UserCountsEntity).GetCustomAttributes(typeof (TableAttribute), true).Select(a => (TableAttribute)a).First();
            string entityName = tableAttribute.Name;

            string userIdPropertyName = NameOfHelper.PropertyName<UserCountsEntity>(x => x.UserId);
            string likesPropertyName = NameOfHelper.PropertyName<UserCountsEntity>(x => x.Likes);
            string dislikesPropertyName = NameOfHelper.PropertyName<UserCountsEntity>(x => x.Dislikes);
            string viewsPropertyName = NameOfHelper.PropertyName<UserCountsEntity>(x => x.Views);
            string abusesPropertyName = NameOfHelper.PropertyName<UserCountsEntity>(x => x.Abuses);

            _getStatement = new Lazy<PreparedStatement>(() => _session.Get().Prepare(string.Format("SELECT * FROM \"{0}\" WHERE \"{1}\" = ?", entityName, userIdPropertyName)));
            _incViewsStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("UPDATE  \"{0}\" SET \"{2}\" = \"{2}\" + 1 WHERE \"{1}\" = ?", entityName, userIdPropertyName, viewsPropertyName)));
            _incLikesStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("UPDATE  \"{0}\" SET \"{2}\" = \"{2}\" + 1 WHERE \"{1}\" = ?", entityName, userIdPropertyName, likesPropertyName)));
            _incDislikesStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("UPDATE  \"{0}\" SET \"{2}\" = \"{2}\" + 1 WHERE \"{1}\" = ?", entityName, userIdPropertyName, dislikesPropertyName)));
            _decLikesStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("UPDATE  \"{0}\" SET \"{2}\" = \"{2}\" - 1 WHERE \"{1}\" = ?", entityName, userIdPropertyName, likesPropertyName)));
            _decDislikesStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("UPDATE  \"{0}\" SET \"{2}\" = \"{2}\" - 1 WHERE \"{1}\" = ?", entityName, userIdPropertyName, dislikesPropertyName)));
            _incAbuseStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("UPDATE  \"{0}\" SET \"{2}\" = \"{2}\" + 1 WHERE \"{1}\" = ?", entityName, userIdPropertyName, abusesPropertyName)));
        }

        public async Task<UserCountsEntity> GetAsync(string userId)
        {
            BoundStatement boundStatement = _getStatement.Value.Bind(userId);
            RowSet rowset = await _session.Get().ExecuteAsync(boundStatement);

            List<Row> rows = rowset.ToList();
            if (rows.Count == 0)
            {
                return new UserCountsEntity
                {
                    UserId = userId
                };
            }

            Row row = rows.First();
            return _mapper.Map<Row, UserCountsEntity>(row);
        }

        public Task IncViewsAsync(string userId)
        {
            BoundStatement boundStatement = _incViewsStatement.Value.Bind(userId);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        public Task IncLikesAsync(string userId)
        {
            BoundStatement boundStatement = _incLikesStatement.Value.Bind(userId);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        public Task IncDislikesAsync(string userId)
        {
            BoundStatement boundStatement = _incDislikesStatement.Value.Bind(userId);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        public Task DecLikesAsync(string userId)
        {
            BoundStatement boundStatement = _decLikesStatement.Value.Bind(userId);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        public Task DecDislikesAsync(string userId)
        {
            BoundStatement boundStatement = _decDislikesStatement.Value.Bind(userId);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        public Task IncAbuseAsync(string userId)
        {
            BoundStatement boundStatement = _incAbuseStatement.Value.Bind(userId);
            return _session.Get().ExecuteAsync(boundStatement);
        }
    }
}