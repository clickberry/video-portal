// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Cassandra;
using Configuration;

namespace Portal.DAL.Azure
{
    public class CassandraSession : ICassandraSession
    {
        private readonly Lazy<ISession> _session;

        public CassandraSession(ICassandraClient client, IPortalFrontendSettings settings)
        {
            _session = new Lazy<ISession>(() => client.GetSession(settings.CassandraKeyspace));
        }

        public ISession Get()
        {
            return _session.Value;
        }
    }
}