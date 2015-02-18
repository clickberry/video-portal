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
    public class ItemCountsRepository : IItemCountsRepository
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


        public ItemCountsRepository(ICassandraSession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;

            // Preparing statements
            TableAttribute tableAttribute = typeof (ItemCountsEntity).GetCustomAttributes(typeof (TableAttribute), true).Select(a => (TableAttribute)a).First();
            string entityName = tableAttribute.Name;

            string itemIdPropertyName = NameOfHelper.PropertyName<ItemCountsEntity>(x => x.ItemId);
            string likesPropertyName = NameOfHelper.PropertyName<ItemCountsEntity>(x => x.Likes);
            string dislikesPropertyName = NameOfHelper.PropertyName<ItemCountsEntity>(x => x.Dislikes);
            string viewsPropertyName = NameOfHelper.PropertyName<ItemCountsEntity>(x => x.Views);
            string abusesPropertyName = NameOfHelper.PropertyName<ItemCountsEntity>(x => x.Abuses);

            _getStatement = new Lazy<PreparedStatement>(() => _session.Get().Prepare(string.Format("SELECT * FROM \"{0}\" WHERE \"{1}\" = ?", entityName, itemIdPropertyName)));
            _incViewsStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("UPDATE  \"{0}\" SET \"{2}\" = \"{2}\" + 1 WHERE \"{1}\" = ?", entityName, itemIdPropertyName, viewsPropertyName)));
            _incLikesStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("UPDATE  \"{0}\" SET \"{2}\" = \"{2}\" + 1 WHERE \"{1}\" = ?", entityName, itemIdPropertyName, likesPropertyName)));
            _incDislikesStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("UPDATE  \"{0}\" SET \"{2}\" = \"{2}\" + 1 WHERE \"{1}\" = ?", entityName, itemIdPropertyName, dislikesPropertyName)));
            _decLikesStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("UPDATE  \"{0}\" SET \"{2}\" = \"{2}\" - 1 WHERE \"{1}\" = ?", entityName, itemIdPropertyName, likesPropertyName)));
            _decDislikesStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("UPDATE  \"{0}\" SET \"{2}\" = \"{2}\" - 1 WHERE \"{1}\" = ?", entityName, itemIdPropertyName, dislikesPropertyName)));
            _incAbuseStatement =
                new Lazy<PreparedStatement>(
                    () => _session.Get().Prepare(string.Format("UPDATE  \"{0}\" SET \"{2}\" = \"{2}\" + 1 WHERE \"{1}\" = ?", entityName, itemIdPropertyName, abusesPropertyName)));
        }

        public async Task<ItemCountsEntity> GetAsync(string itemId)
        {
            BoundStatement boundStatement = _getStatement.Value.Bind(itemId);
            RowSet rowset = await _session.Get().ExecuteAsync(boundStatement);

            List<Row> rows = rowset.ToList();
            if (rows.Count == 0)
            {
                return new ItemCountsEntity
                {
                    ItemId = itemId
                };
            }

            Row row = rows.First();
            return _mapper.Map<Row, ItemCountsEntity>(row);
        }

        public Task IncViewsAsync(string itemId)
        {
            BoundStatement boundStatement = _incViewsStatement.Value.Bind(itemId);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        public Task IncLikesAsync(string itemId)
        {
            BoundStatement boundStatement = _incLikesStatement.Value.Bind(itemId);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        public Task IncDislikesAsync(string itemId)
        {
            BoundStatement boundStatement = _incDislikesStatement.Value.Bind(itemId);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        public Task DecLikesAsync(string itemId)
        {
            BoundStatement boundStatement = _decLikesStatement.Value.Bind(itemId);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        public Task DecDislikesAsync(string itemId)
        {
            BoundStatement boundStatement = _decDislikesStatement.Value.Bind(itemId);
            return _session.Get().ExecuteAsync(boundStatement);
        }

        public Task IncAbuseAsync(string itemId)
        {
            BoundStatement boundStatement = _incAbuseStatement.Value.Bind(itemId);
            return _session.Get().ExecuteAsync(boundStatement);
        }
    }
}