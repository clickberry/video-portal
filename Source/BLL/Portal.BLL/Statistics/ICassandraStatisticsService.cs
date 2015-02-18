// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Statistics
{
    public interface ICassandraStatisticsService
    {
        Task<DomainItemCounts> GetItemCountsAsync(string space, string itemId);

        Task<DomainUserCounts> GetUserCountsAsync(string space, string userId);

        // VIEWS

        Task AddViewAsync(string space, string itemId, string userId);

        Task<IEnumerable<DomainItemSignal>> GetItemViewsSequenceAsync(string space, string itemId, int pageSize);

        Task<IEnumerable<DomainUserSignal>> GetUserViewsSequenceAsync(string space, string userId, int pageSize);

        Task<IEnumerable<DomainMostSignaledItem>> GetMostViewedForLastWeekAsync(string space, int pageSize, long? version);

        // LIKES

        Task AddLikeAsync(string space, string itemId, string userId);

        Task DeleteLikeAsync(string space, string itemId, string userId);

        Task<IEnumerable<DomainItemSignal>> GetItemLikesSequenceAsync(string space, string itemId, int pageSize);

        /// <summary>
        ///     Returns user likes.
        ///     Since sequence is ordered in anti-chronological order it can contains duplicates which should be filtered in client
        ///     code.
        /// </summary>
        /// <param name="space"></param>
        /// <param name="userId"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IEnumerable<DomainUserSignal>> GetUserLikesSequenceAsync(string space, string userId, int pageSize);

        Task<bool> IsLikedAsync(string space, string itemId, string userId);

        // DISLIKES

        Task AddDislikeAsync(string space, string itemId, string userId);

        Task DeleteDislikeAsync(string space, string itemId, string userId);

        Task<IEnumerable<DomainItemSignal>> GetItemDislikesSequenceAsync(string space, string itemId, int pageSize);

        /// <summary>
        ///     Returns user dislikes.
        ///     Since sequence is ordered in anti-chronological order it can contains duplicates which should be filtered in client
        ///     code.
        /// </summary>
        /// <param name="space"></param>
        /// <param name="userId"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IEnumerable<DomainUserSignal>> GetUserDislikesSequenceAsync(string space, string userId, int pageSize);

        Task<bool> IsDislikedAsync(string space, string itemId, string userId);

        // ABUSE

        Task AddAbuseAsync(string space, string itemId, string userId);

        Task<IEnumerable<DomainItemSignal>> GetItemAbuseSequenceAsync(string space, string itemId, int pageSize);

        Task<bool> IsAbuseReportedAsync(string space, string itemId, string userId);
    }
}