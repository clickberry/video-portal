// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.DAL.Entities.Statistics;

namespace Portal.DAL.Statistics
{
    public interface IAffinityGroupMostSignaledVersionRepository
    {
        Task<AffinityGroupMostSignaledVersionEntity> GetAsync(string groupId, SignalType signal);

        Task UpdateAsync(string groupId, SignalType signal, long version);
    }
}