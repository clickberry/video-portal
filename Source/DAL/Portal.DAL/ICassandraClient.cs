// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Cassandra;

namespace Portal.DAL
{
    public interface ICassandraClient : IDisposable
    {
        ISession GetSession();

        ISession GetSession(string keyspace);
    }
}