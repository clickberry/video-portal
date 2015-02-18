// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Cassandra.Data.Linq;

namespace Portal.DAL.Entities.Statistics
{
    /// <summary>
    ///     Holds current version value for affinity group most signaled CF.
    /// </summary>
    [Table("AffinityGroupMostSignaledVersion")]
    public class AffinityGroupMostSignaledVersionEntity
    {
        /// <summary>
        ///     Composite row key as group|signalType
        /// </summary>
        [PartitionKey]
        public string AffinityGroupSignalType { get; set; }

        public long Version { get; set; }
    }
}