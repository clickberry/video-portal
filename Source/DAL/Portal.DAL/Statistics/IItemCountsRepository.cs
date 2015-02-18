// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.DAL.Entities.Statistics;

namespace Portal.DAL.Statistics
{
    public interface IItemCountsRepository
    {
        Task<ItemCountsEntity> GetAsync(string itemId);

        Task IncViewsAsync(string itemId);

        Task IncLikesAsync(string itemId);

        Task IncDislikesAsync(string itemId);

        Task DecLikesAsync(string itemId);

        Task DecDislikesAsync(string itemId);

        Task IncAbuseAsync(string itemId);
    }
}