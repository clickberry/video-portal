// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Cassandra.Data.Linq;

namespace Portal.DAL.Entities.Statistics
{
    /// <summary>
    ///     Time series rollups for each day.
    ///     Wide row.
    /// </summary>
    [Table("TimeSeriesRollupsDay")]
    public class TimeSeriesRollupsDayEntity
    {
        [PartitionKey]
        public string EventId { get; set; }

        [ClusteringKey(1)]
        public DateTimeOffset Day { get; set; }

        [Counter]
        public long Count { get; set; }
    }
}