// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Cassandra.Data.Linq;

namespace Portal.DAL.Entities.Statistics
{
    /// <summary>
    ///     All user signals in chronological order for specified signal type.
    ///     Wide row.
    /// </summary>
    [Table("UserSignals2")]
    public class UserSignalsEntity
    {
        /// <summary>
        ///     Compostie row key as userId|signalType
        /// </summary>
        [PartitionKey]
        public string UserIdSignalType { get; set; }

        /// <summary>
        ///     Negative columns which cancel positive columns.
        /// </summary>
        [ClusteringKey(1, "DESC")]
        public bool IsAnticolumn { get; set; }

        [ClusteringKey(2, "DESC")]
        public DateTimeOffset DateTime { get; set; }

        [ClusteringKey(3)]
        public string ItemId { get; set; }
    }
}