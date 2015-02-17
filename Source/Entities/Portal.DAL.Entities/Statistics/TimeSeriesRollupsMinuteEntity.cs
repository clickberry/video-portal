// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Cassandra.Data.Linq;

namespace Portal.DAL.Entities.Statistics
{
    /// <summary>
    ///     Time series rollups for each minute.
    ///     Wide row.
    /// </summary>
    [Table("TimeSeriesRollupsMinute")]
    public class TimeSeriesRollupsMinuteEntity
    {
        /// <summary>
        ///     Composite row key as eventId|hh
        /// </summary>
        [PartitionKey]
        public string EventIdHh { get; set; }

        [ClusteringKey(1)]
        public DateTimeOffset Minute { get; set; }

        [Counter]
        public long Count { get; set; }
    }
}