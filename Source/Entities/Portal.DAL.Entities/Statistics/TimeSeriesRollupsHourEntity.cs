// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Cassandra.Data.Linq;

namespace Portal.DAL.Entities.Statistics
{
    /// <summary>
    ///     Time series rollups for each hour.
    ///     Wide row.
    /// </summary>
    [Table("TimeSeriesRollupsHour")]
    public class TimeSeriesRollupsHourEntity
    {
        /// <summary>
        ///     Composite row key as eventId|dd
        /// </summary>
        [PartitionKey]
        public string EventIdDd { get; set; }

        [ClusteringKey(1)]
        public DateTimeOffset Hour { get; set; }

        [Counter]
        public long Count { get; set; }
    }
}