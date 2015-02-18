// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.DAL.Entities.Statistics;

namespace Portal.DAL.Statistics
{
    public interface ITimeSeriesRawRepository
    {
        Task AddHourlyAsync(TimeSeriesRawInsertOptions options);

        Task<IEnumerable<TimeSeriesRawEntity>> GetHourlySequenceAsync(string eventId, DateTime fromTime, DateTime toTime, int pageSize);
    }
}