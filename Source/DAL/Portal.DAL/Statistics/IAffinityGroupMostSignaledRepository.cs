// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.DAL.Entities.Statistics;

namespace Portal.DAL.Statistics
{
    public interface IAffinityGroupMostSignaledRepository
    {
        Task AddAsync(string groupId, SignalType signal, long version, List<AffinityGroupMostSignaledInsertOptions> options);

        Task<IEnumerable<AffinityGroupMostSignaledEntity>> GetSequenceAsync(string groupId, SignalType signal, long version, int pageSize);

        Task DeleteVersionAsync(string groupId, SignalType signal, long version);
    }
}