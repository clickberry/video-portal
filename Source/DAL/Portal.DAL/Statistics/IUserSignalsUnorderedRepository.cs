// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.DAL.Entities.Statistics;

namespace Portal.DAL.Statistics
{
    /// <summary>
    ///     Separate index repository for quick search whether a user has a signal for specified item.
    /// </summary>
    public interface IUserSignalsUnorderedRepository
    {
        Task AddAsync(UserSignalsInsertDeleteOptions insertOptions);

        Task<UserSignalsUnorderedEntity> GetAsync(string userId, SignalType signal, string itemId);

        Task DeleteAsync(UserSignalsInsertDeleteOptions deleteOptions);
    }
}