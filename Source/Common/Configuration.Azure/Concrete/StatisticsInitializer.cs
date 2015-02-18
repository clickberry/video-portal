// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using Cassandra;
using Cassandra.Data.Linq;
using Portal.DAL;
using Portal.DAL.Entities.Statistics;

namespace Configuration.Azure.Concrete
{
    public sealed class StatisticsInitializer : IInitializable
    {
        private readonly ICassandraClient _cassandraClient;
        private readonly string _keyspace;

        public StatisticsInitializer(ICassandraClient cassandraClient, IPortalFrontendSettings settings)
        {
            _cassandraClient = cassandraClient;
            _keyspace = settings.CassandraKeyspace;
        }

        public void Initialize()
        {
            // Create keyspace if not exists
            var replicationOptions = new Dictionary<string, string>
            {
                { "class", "SimpleStrategy" },
                { "replication_factor", "3" }
            };

            ISession session = _cassandraClient.GetSession();
            session.CreateKeyspaceIfNotExists(_keyspace, replicationOptions);
            session.ChangeKeyspace(_keyspace);


            // create table if not exists
            session.GetTable<ItemCountsEntity>().CreateIfNotExists();
            session.GetTable<UserCountsEntity>().CreateIfNotExists();
            session.GetTable<ItemSignalsEntity>().CreateIfNotExists();
            session.GetTable<UserSignalsEntity>().CreateIfNotExists();
            session.GetTable<UserSignalsUnorderedEntity>().CreateIfNotExists();

            session.GetTable<AffinityGroupMostSignaledEntity>().CreateIfNotExists();
            session.GetTable<AffinityGroupMostSignaledVersionEntity>().CreateIfNotExists();
            session.GetTable<AffinityGroupItemCountsEntity>().CreateIfNotExists();
            session.GetTable<AffinityGroupCountsEntity>().CreateIfNotExists();

            session.GetTable<TimeSeriesRawEntity>().CreateIfNotExists();
            session.GetTable<TimeSeriesRollupsDayEntity>().CreateIfNotExists();
            session.GetTable<TimeSeriesRollupsHourEntity>().CreateIfNotExists();
            session.GetTable<TimeSeriesRollupsMinuteEntity>().CreateIfNotExists();
        }
    }
}