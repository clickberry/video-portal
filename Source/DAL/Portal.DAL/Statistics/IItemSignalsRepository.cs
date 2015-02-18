// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.DAL.Entities.Statistics;

namespace Portal.DAL.Statistics
{
    public interface IItemSignalsRepository
    {
        Task AddAsync(ItemSignalsInsertDeleteOptions options);

        Task<IEnumerable<ItemSignalsEntity>> GetAntiSequenceAsync(string itemId, SignalType signal, int pageSize);

        Task<IEnumerable<ItemSignalsEntity>> GetSequenceAsync(string itemId, SignalType signal, int pageSize);

        Task DeleteAsync(ItemSignalsInsertDeleteOptions options);
    }
}