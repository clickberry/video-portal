// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.DAL.Entities.Statistics;

namespace Portal.DAL.Statistics
{
    public interface IUserSignalsRepository
    {
        Task AddAsync(UserSignalsInsertDeleteOptions insertOptions);

        Task<IEnumerable<UserSignalsEntity>> GetAntiSequenceAsync(string userId, SignalType signal, int pageSize);

        Task<IEnumerable<UserSignalsEntity>> GetSequenceAsync(string userId, SignalType signal, int pageSize);

        Task DeleteAsync(UserSignalsInsertDeleteOptions deleteOptions);
    }
}