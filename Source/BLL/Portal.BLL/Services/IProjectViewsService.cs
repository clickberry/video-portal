// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Domain;
using Portal.DTO.Trends;

namespace Portal.BLL.Services
{
    /// <summary>
    ///     Trends statistics for Watch objects on top of the Cassandra Statistics.
    /// </summary>
    public interface IProjectViewsService
    {
        Task<IEnumerable<TrendingWatch>> GetWeeklyTrendsSequenceAsync(DataQueryOptions filter);
    }
}