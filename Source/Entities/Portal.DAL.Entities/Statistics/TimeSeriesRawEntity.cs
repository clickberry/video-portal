// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Cassandra.Data.Linq;

namespace Portal.DAL.Entities.Statistics
{
    /// <summary>
    ///     Time serises raw data with 1 hour bucket.
    ///     Wide row.
    /// </summary>
    [Table("TimeSeriesRaw")]
    public class TimeSeriesRawEntity
    {
        /// <summary>
        ///     Composite row key as eventId|yymmddhh
        /// </summary>
        [PartitionKey]
        public string EventIdYymmddhh { get; set; }

        [ClusteringKey(1)]
        public DateTimeOffset DateTime { get; set; }

        /// <summary>
        ///     Arbitrary data associated with event.
        /// </summary>
        public string Payload { get; set; }
    }
}