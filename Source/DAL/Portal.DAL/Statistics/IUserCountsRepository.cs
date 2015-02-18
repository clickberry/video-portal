// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.DAL.Entities.Statistics;

namespace Portal.DAL.Statistics
{
    public interface IUserCountsRepository
    {
        Task<UserCountsEntity> GetAsync(string userId);

        Task IncViewsAsync(string userId);

        Task IncLikesAsync(string userId);

        Task IncDislikesAsync(string userId);

        Task DecLikesAsync(string userId);

        Task DecDislikesAsync(string userId);

        Task IncAbuseAsync(string userId);
    }
}