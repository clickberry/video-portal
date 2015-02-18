// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Cassandra;
using Configuration;

namespace Portal.DAL.Azure
{
    public class CassandraClient : ICassandraClient
    {
        private readonly Cluster _cluster;
        private readonly int _heartBeatInterval = (int)TimeSpan.FromMinutes(3).TotalMilliseconds;

        public CassandraClient(IPortalFrontendSettings settings)
        {
            string[] nodeAddresses = settings.CassandraNodes;
            var privateAddresses = settings.CassandraPrivateAddresses;

            if (nodeAddresses.Length == 0)
            {
                throw new ArgumentException("settings");
            }

            // Credetials
            Builder clusterBuilder = Cluster.Builder();
            if (!string.IsNullOrEmpty(settings.CassandraUsername) && !string.IsNullOrEmpty(settings.CassandraPassword))
            {
                clusterBuilder = clusterBuilder.WithCredentials(settings.CassandraUsername, settings.CassandraPassword);
            }

            // Hosts
            clusterBuilder.AddContactPoints(nodeAddresses);

            if (privateAddresses.Length > 0)
            {
                if (privateAddresses.Length != nodeAddresses.Length)
                {
                    throw new ArgumentException("settings");
                }

                const int cqlPort = 9042;
                var addressMap = new Dictionary<IPEndPoint, IPEndPoint>(nodeAddresses.Length);
                for (int i = 0; i < nodeAddresses.Length; i++)
                {
                    // Create address map for cassandra nodes behind azure firewall
                    IPHostEntry host = Dns.GetHostEntry(nodeAddresses[i]);
                    var privateAddress = IPAddress.Parse(privateAddresses[i]);
                    var publicAddress = host.AddressList.First();

                    addressMap.Add(new IPEndPoint(privateAddress, cqlPort), new IPEndPoint(publicAddress, cqlPort));
                }

                clusterBuilder.WithAddressTranslator(new CassandraAddressTranslator(addressMap));
            }

            // Set heartbeat interval
            clusterBuilder.SocketOptions.SetIdleTimeoutMillis((int)TimeSpan.FromMinutes(3).TotalMilliseconds);

            _cluster = clusterBuilder.Build();
        }

        public void Dispose()
        {
            _cluster.Shutdown();
        }

        public ISession GetSession()
        {
            return _cluster.Connect();
        }

        public ISession GetSession(string keyspace)
        {
            return _cluster.Connect(keyspace);
        }
    }
}