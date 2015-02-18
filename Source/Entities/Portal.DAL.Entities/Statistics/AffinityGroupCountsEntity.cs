// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Cassandra.Data.Linq;

namespace Portal.DAL.Entities.Statistics
{
    /// <summary>
    ///     Affinity group cumulative counters.
    /// </summary>
    [Table("AffinityGroupCounts")]
    public class AffinityGroupCountsEntity
    {
        /// <summary>
        ///     Composite row key as group|signalType
        /// </summary>
        [PartitionKey]
        public string AffinityGroupSignalType { get; set; }

        [Counter]
        public long Count { get; set; }
    }
}