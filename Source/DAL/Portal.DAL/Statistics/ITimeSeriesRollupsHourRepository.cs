// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.DAL.Entities.Statistics;

namespace Portal.DAL.Statistics
{
    public interface ITimeSeriesRollupsHourRepository
    {
        Task<IEnumerable<TimeSeriesRollupsHourEntity>> GetSequenceAsync(string eventId, int day, DateTime fromTime, DateTime toTime, int pageSize);

        Task IncAsync(TimeSeriesRollupsInsertOptions options);
    }
}