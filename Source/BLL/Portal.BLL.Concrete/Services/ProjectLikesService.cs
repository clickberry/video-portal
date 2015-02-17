// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Concrete.Statistics;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.BLL.Statistics;
using Portal.Domain;
using Portal.Domain.ProfileContext;
using Portal.Domain.StatisticContext;
using Portal.DTO.User;
using Portal.DTO.Watch;

namespace Portal.BLL.Concrete.Services
{
    public class ProjectLikesService : IProjectLikesService
    {
        private const int MemoryPageSize = 1000;
        private const int MaxScanPages = 10;

        private readonly ICassandraStatisticsService _cassandraStatisticsService;
        private readonly IUserAvatarProvider _userAvatarProvider;
        private readonly IUserService _userService;
        private readonly IWatchProjectService _watchProjectService;

        public ProjectLikesService(ICassandraStatisticsService cassandraStatisticsService,
            IWatchProjectService watchProjectService,
            IUserService userService,
            IUserAvatarProvider userAvatarProvider)
        {
            _cassandraStatisticsService = cassandraStatisticsService;
            _watchProjectService = watchProjectService;
            _userService = userService;
            _userAvatarProvider = userAvatarProvider;
        }

        public async Task<IEnumerable<Watch>> GetUserLikesSequenceAsync(string userId, DataQueryOptions filter)
        {
            var result = new List<Watch>();
            IEnumerable<DomainUserSignal> data = await _cassandraStatisticsService.GetUserLikesSequenceAsync(StatisticsSpaces.Projects, userId, MemoryPageSize);

            var pageData = new List<DomainUserSignal>();
            int count = 0;
            int skip = filter != null && filter.Skip.HasValue ? filter.Skip.Value : 0;
            var uniqueIds = new HashSet<string>();
            foreach (DomainUserSignal item in data)
            {
                if (result.Count >= MemoryPageSize || count > MemoryPageSize*MaxScanPages)
                {
                    // memory limit reached
                    break;
                }

                if (filter != null && filter.Take.HasValue && result.Count >= filter.Take.Value + skip)
                {
                    // all items loaded
                    break;
                }

                // checking uniqueness
                if (!uniqueIds.Contains(item.ItemId))
                {
                    // there can be duplicated ItemIds with different DateTime values due to race conditions
                    uniqueIds.Add(item.ItemId);
                }
                else
                {
                    // skipping duplicate
                    count++;
                    continue;
                }

                // adding to memory page
                pageData.Add(item);

                // checking limits
                if ((count > 0 && count%MemoryPageSize == 0) ||
                    (filter != null && filter.Take.HasValue && pageData.Count >= filter.Take.Value))
                {
                    // loading and filtering items
                    List<Watch> page = await LoadProjectPageDataAsync(pageData, userId);
                    result.AddRange(page);
                    pageData.Clear();
                }

                count++;
            }

            if (pageData.Count > 0)
            {
                // loading last page
                List<Watch> page = await LoadProjectPageDataAsync(pageData, userId);
                result.AddRange(page);
                pageData.Clear();
            }

            // Skip
            result = result.Skip(skip).ToList();

            // Take
            if (filter != null && filter.Take.HasValue)
            {
                result = result.Take(filter.Take.Value).ToList();
            }

            return result;
        }

        public Task<bool> IsLikedAsync(string projectId, string userId)
        {
            return _cassandraStatisticsService.IsLikedAsync(StatisticsSpaces.Projects, projectId, userId);
        }

        public async Task<IEnumerable<UserInfo>> GetProjectLikersSequenceAsync(string projectId, DataQueryOptions filter)
        {
            var result = new List<UserInfo>();
            IEnumerable<DomainItemSignal> data = await _cassandraStatisticsService.GetItemLikesSequenceAsync(StatisticsSpaces.Projects, projectId, MemoryPageSize);

            var pageData = new List<DomainItemSignal>();
            int count = 0;
            int skip = filter != null && filter.Skip.HasValue ? filter.Skip.Value : 0;
            foreach (DomainItemSignal user in data)
            {
                if (result.Count >= MemoryPageSize || count > MemoryPageSize*MaxScanPages)
                {
                    // memory limit reached
                    break;
                }

                if (filter != null && filter.Take.HasValue && result.Count >= filter.Take.Value + skip)
                {
                    // all items loaded
                    break;
                }

                // adding to page
                pageData.Add(user);

                // checking limits
                if ((count > 0 && count%MemoryPageSize == 0) ||
                    (filter != null && filter.Take.HasValue && pageData.Count >= filter.Take.Value))
                {
                    // loading and filtering items
                    List<UserInfo> page = await LoadUserPageDataAsync(pageData);
                    result.AddRange(page);
                    pageData.Clear();
                }

                count++;
            }

            if (pageData.Count > 0)
            {
                // loading last page
                List<UserInfo> page = await LoadUserPageDataAsync(pageData);
                result.AddRange(page);
                pageData.Clear();
            }

            // Skip
            result = result.Skip(skip).ToList();

            // Take
            if (filter != null && filter.Take.HasValue)
            {
                result = result.Take(filter.Take.Value).ToList();
            }

            return result;
        }

        public async Task<long> GetProjectLikesCountAsync(string projectId)
        {
            IEnumerable<DomainItemSignal> data = await _cassandraStatisticsService.GetItemLikesSequenceAsync(StatisticsSpaces.Projects, projectId, MemoryPageSize);
            return data.LongCount();
        }

        public async Task<IEnumerable<Watch>> GetUserDislikesSequenceAsync(string userId, DataQueryOptions filter)
        {
            var result = new List<Watch>();
            IEnumerable<DomainUserSignal> data = await _cassandraStatisticsService.GetUserDislikesSequenceAsync(StatisticsSpaces.Projects, userId, MemoryPageSize);

            var pageData = new List<DomainUserSignal>();
            int count = 0;
            int skip = filter != null && filter.Skip.HasValue ? filter.Skip.Value : 0;
            var uniqueIds = new HashSet<string>();
            foreach (DomainUserSignal item in data)
            {
                if (result.Count >= MemoryPageSize || count > MemoryPageSize*MaxScanPages)
                {
                    // memory limit reached
                    break;
                }

                if (filter != null && filter.Take.HasValue && result.Count >= filter.Take.Value + skip)
                {
                    // all items loaded
                    break;
                }

                // checking uniqueness
                if (!uniqueIds.Contains(item.ItemId))
                {
                    // there can be duplicated ItemIds with different DateTime values due to race conditions
                    uniqueIds.Add(item.ItemId);
                }
                else
                {
                    // skipping duplicate
                    count++;
                    continue;
                }

                // adding to memory page
                pageData.Add(item);

                // checking limits
                if ((count > 0 && count%MemoryPageSize == 0) ||
                    (filter != null && filter.Take.HasValue && pageData.Count >= filter.Take.Value))
                {
                    // loading and filtering items
                    List<Watch> page = await LoadProjectPageDataAsync(pageData, userId);
                    result.AddRange(page);
                    pageData.Clear();
                }

                count++;
            }

            if (pageData.Count > 0)
            {
                // loading last page
                List<Watch> page = await LoadProjectPageDataAsync(pageData, userId);
                result.AddRange(page);
                pageData.Clear();
            }

            // Skip
            result = result.Skip(skip).ToList();

            // Take
            if (filter != null && filter.Take.HasValue)
            {
                result = result.Take(filter.Take.Value).ToList();
            }

            return result;
        }

        public Task<bool> IsDislikedAsync(string projectId, string userId)
        {
            return _cassandraStatisticsService.IsDislikedAsync(StatisticsSpaces.Projects, projectId, userId);
        }

        public async Task<IEnumerable<UserInfo>> GetProjectDislikersSequenceAsync(string projectId, DataQueryOptions filter)
        {
            var result = new List<UserInfo>();
            IEnumerable<DomainItemSignal> data = await _cassandraStatisticsService.GetItemDislikesSequenceAsync(StatisticsSpaces.Projects, projectId, MemoryPageSize);

            var pageData = new List<DomainItemSignal>();
            int count = 0;
            int skip = filter != null && filter.Skip.HasValue ? filter.Skip.Value : 0;
            foreach (DomainItemSignal user in data)
            {
                if (result.Count >= MemoryPageSize || count > MemoryPageSize*MaxScanPages)
                {
                    // memory limit reached
                    break;
                }

                if (filter != null && filter.Take.HasValue && result.Count >= filter.Take.Value + skip)
                {
                    // all items loaded
                    break;
                }

                // adding to page
                pageData.Add(user);

                // checking limits
                if ((count > 0 && count%MemoryPageSize == 0) ||
                    (filter != null && filter.Take.HasValue && pageData.Count >= filter.Take.Value))
                {
                    // loading and filtering items
                    List<UserInfo> page = await LoadUserPageDataAsync(pageData);
                    result.AddRange(page);
                    pageData.Clear();
                }

                count++;
            }

            if (pageData.Count > 0)
            {
                // loading last page
                List<UserInfo> page = await LoadUserPageDataAsync(pageData);
                result.AddRange(page);
                pageData.Clear();
            }

            // Skip
            result = result.Skip(skip).ToList();

            // Take
            if (filter != null && filter.Take.HasValue)
            {
                result = result.Take(filter.Take.Value).ToList();
            }

            return result;
        }

        public async Task<long> GetProjectDislikesCountAsync(string projectId)
        {
            IEnumerable<DomainItemSignal> data = await _cassandraStatisticsService.GetItemDislikesSequenceAsync(StatisticsSpaces.Projects, projectId, MemoryPageSize);
            return data.LongCount();
        }

        #region helpers

        private async Task<List<Watch>> LoadProjectPageDataAsync(IEnumerable<DomainUserSignal> pageData, string userId)
        {
            List<DomainUserSignal> items = pageData.ToList();
            List<Watch> watches = await _watchProjectService.GetByIdsAsync(items.Select(i => i.ItemId).ToArray(), userId);

            return items.Select(item => watches.FirstOrDefault(w => w.Id == item.ItemId)).Where(watch => watch != null).ToList();
        }

        private async Task<List<UserInfo>> LoadUserPageDataAsync(IEnumerable<DomainItemSignal> pageData)
        {
            List<DomainItemSignal> items = pageData.ToList();

            // Aggregating user data
            List<DomainUser> users = await _userService.GetByIdsAsync(items.Select(i => i.UserId).ToArray());

            // Projecting
            return users.Select(u => new UserInfo { Id = u.Id, UserName = u.Name, AvatarUrl = _userAvatarProvider.GetAvatar(new DomainUser { Email = u.Email }) }).ToList();
        }

        #endregion
    }
}