// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Cassandra.Data.Linq;

namespace Portal.DAL.Entities.Statistics
{
    /// <summary>
    ///     Holds all cumulative counters for item.
    /// </summary>
    [Table("ItemCounts")]
    public class ItemCountsEntity
    {
        [PartitionKey]
        public string ItemId { get; set; }

        [Counter]
        public long Likes { get; set; }

        [Counter]
        public long Dislikes { get; set; }

        [Counter]
        public long Views { get; set; }

        [Counter]
        public long Abuses { get; set; }
    }
}