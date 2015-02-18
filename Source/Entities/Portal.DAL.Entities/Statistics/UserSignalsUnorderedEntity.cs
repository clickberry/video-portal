// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Cassandra.Data.Linq;

namespace Portal.DAL.Entities.Statistics
{
    /// <summary>
    ///     All unordered user signals for specified signal type for quick searching if item has been already signaled.
    ///     Wide row.
    /// </summary>
    [Table("UserSignalsUnordered2")]
    public class UserSignalsUnorderedEntity
    {
        /// <summary>
        ///     Compostie row key as userId|signalType
        /// </summary>
        [PartitionKey]
        public string UserIdSignalType { get; set; }

        /// <summary>
        ///     Negative columns which cancels positive columns.
        /// </summary>
        [ClusteringKey(1, "DESC")]
        public bool IsAnticolumn { get; set; }

        [ClusteringKey(2)]
        public string ItemId { get; set; }

        [ClusteringKey(3)]
        public DateTimeOffset DateTime { get; set; }
    }
}