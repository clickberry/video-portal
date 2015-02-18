// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Net;
using Cassandra;

namespace Portal.DAL.Azure
{
    public sealed class CassandraAddressTranslator : IAddressTranslator
    {
        private readonly Dictionary<IPEndPoint, IPEndPoint> _addressMap;

        public CassandraAddressTranslator()
        {
            _addressMap = new Dictionary<IPEndPoint, IPEndPoint>();
        }

        public CassandraAddressTranslator(Dictionary<IPEndPoint, IPEndPoint> addressMap)
        {
            _addressMap = addressMap;
        }

        public Dictionary<IPEndPoint, IPEndPoint> AddressMap
        {
            get { return _addressMap; }
        }

        public IPEndPoint Translate(IPEndPoint address)
        {
            IPEndPoint result;
            return _addressMap.TryGetValue(address, out result) ? result : address;
        }
    }
}