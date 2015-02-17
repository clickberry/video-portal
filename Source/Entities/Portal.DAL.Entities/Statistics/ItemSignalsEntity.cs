// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Cassandra.Data.Linq;

namespace Portal.DAL.Entities.Statistics
{
    /// <summary>
    ///     All signaled users for item and event type.
    ///     Wide row.
    /// </summary>
    [Table("ItemSignals2")]
    public class ItemSignalsEntity
    {
        /// <summary>
        ///     Composite row key as itemId|signalType
        /// </summary>
        [PartitionKey]
        public string ItemIdSignalType { get; set; }

        /// <summary>
        ///     Negative columns which cancels positive columns.
        /// </summary>
        [ClusteringKey(1, "DESC")]
        public bool IsAnticolumn { get; set; }

        [ClusteringKey(2)]
        public string UserId { get; set; }

        public DateTimeOffset DateTime { get; set; }
    }
}