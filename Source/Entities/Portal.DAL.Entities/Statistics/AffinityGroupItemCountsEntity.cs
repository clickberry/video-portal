// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Cassandra.Data.Linq;

namespace Portal.DAL.Entities.Statistics
{
    /// <summary>
    ///     Signal counts for each item in affinity group.
    ///     Wide row.
    /// </summary>
    [Table("AffinityGroupItemCounts")]
    public class AffinityGroupItemCountsEntity
    {
        /// <summary>
        ///     Composite row key as group|signalType
        /// </summary>
        [PartitionKey]
        public string AffinityGroupSignalType { get; set; }

        [ClusteringKey(1)]
        public string ItemId { get; set; }

        [Counter]
        public long Count { get; set; }
    }
}