// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

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

namespace Portal.BLL.Concrete.Services
{
    public class ProjectAbuseService : IProjectAbuseService
    {
        private const int MemoryPageSize = 1000;
        private const int MaxScanPages = 10;

        private readonly ICassandraStatisticsService _cassandraStatisticsService;
        private readonly IUserAvatarProvider _userAvatarProvider;
        private readonly IUserService _userService;

        public ProjectAbuseService(ICassandraStatisticsService cassandraStatisticsService,
            IUserService userService,
            IUserAvatarProvider userAvatarProvider)
        {
            _cassandraStatisticsService = cassandraStatisticsService;
            _userService = userService;
            _userAvatarProvider = userAvatarProvider;
        }

        public Task<bool> IsReportedAsync(string projectId, string userId)
        {
            return _cassandraStatisticsService.IsAbuseReportedAsync(StatisticsSpaces.Projects, projectId, userId);
        }

        public async Task<IEnumerable<UserInfo>> GetProjectReportersSequenceAsync(string projectId, DataQueryOptions filter)
        {
            var result = new List<UserInfo>();
            IEnumerable<DomainItemSignal> data = await _cassandraStatisticsService.GetItemAbuseSequenceAsync(StatisticsSpaces.Projects, projectId, MemoryPageSize);

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

        public async Task<long> GetProjectReportsCountAsync(string projectId)
        {
            IEnumerable<DomainItemSignal> data = await _cassandraStatisticsService.GetItemAbuseSequenceAsync(StatisticsSpaces.Projects, projectId, MemoryPageSize);
            return data.LongCount();
        }

        #region helpers

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