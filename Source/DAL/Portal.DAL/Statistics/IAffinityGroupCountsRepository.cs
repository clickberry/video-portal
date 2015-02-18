// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.DAL.Entities.Statistics;

namespace Portal.DAL.Statistics
{
    public interface IAffinityGroupCountsRepository
    {
        Task<AffinityGroupCountsEntity> GetAsync(string groupId, SignalType signal);

        Task IncAsync(string groupId, SignalType signal);

        Task DecAsync(string groupId, SignalType signal);
    }
}