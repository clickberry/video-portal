// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.DAL.Entities.Statistics;

namespace Portal.DAL.Statistics
{
    public interface IAffinityGroupItemCountsRepository
    {
        Task<AffinityGroupItemCountsEntity> GetAsync(string groupId, SignalType signal, string itemId);

        Task<IEnumerable<AffinityGroupItemCountsEntity>> GetSequenceAsync(string groupId, SignalType signal, int pageSize);

        Task IncAsync(string groupId, SignalType signal, string itemId);

        Task DecAsync(string groupId, SignalType signal, string itemId);
    }
}