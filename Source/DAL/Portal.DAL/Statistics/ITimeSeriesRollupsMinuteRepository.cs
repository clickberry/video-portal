// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.DAL.Entities.Statistics;

namespace Portal.DAL.Statistics
{
    public interface ITimeSeriesRollupsMinuteRepository
    {
        Task<IEnumerable<TimeSeriesRollupsMinuteEntity>> GetSequenceAsync(string eventId, int hour, DateTime fromTime, DateTime toTime, int pageSize);

        Task IncAsync(TimeSeriesRollupsInsertOptions options);
    }
}