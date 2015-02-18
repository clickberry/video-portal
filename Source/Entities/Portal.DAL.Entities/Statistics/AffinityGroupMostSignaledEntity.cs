// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Cassandra.Data.Linq;

namespace Portal.DAL.Entities.Statistics
{
    /// <summary>
    ///     Trends for specific group and signal type, e.g. most liked in category.
    ///     Wide row.
    /// </summary>
    [Table("AffinityGroupMostSignaled")]
    public class AffinityGroupMostSignaledEntity
    {
        /// <summary>
        ///     Composite row key as group|signalType
        /// </summary>
        [PartitionKey]
        public string AffinityGroupSignalType { get; set; }

        [ClusteringKey(1, "DESC")]
        public long Count { get; set; }

        [ClusteringKey(2)]
        public string ItemId { get; set; }
    }
}