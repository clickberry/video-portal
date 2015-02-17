// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Concrete.Statistics;
using Portal.BLL.Services;
using Portal.BLL.Statistics;
using Portal.Common.Helpers;
using Portal.Domain;
using Portal.Domain.Admin;
using Portal.Domain.ProjectContext;
using Portal.Domain.StatisticContext;
using Portal.DTO.Trends;
using Portal.DTO.Watch;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    public class ProjectViewsService : IProjectViewsService
    {
        private const int MemoryPageSize = 1000;
        private const int MaxScanPages = 10;

        private readonly ICassandraStatisticsService _cassandraStatisticsService;
        private readonly IMapper _mapper;
        private readonly IWatchProjectService _watchProjectService;

        public ProjectViewsService(ICassandraStatisticsService cassandraStatisticsService, IWatchProjectService watchProjectService, IMapper mapper)
        {
            _cassandraStatisticsService = cassandraStatisticsService;
            _watchProjectService = watchProjectService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TrendingWatch>> GetWeeklyTrendsSequenceAsync(DataQueryOptions filter)
        {
            List<DataFilterRule> filters = filter.Filters.Where(f => f.Value != null).ToList();
            long? version = null;
            foreach (DataFilterRule dataFilterRule in filters)
            {
                DataFilterRule f = dataFilterRule;
                if (string.Compare(f.Name, NameOfHelper.PropertyName<TrendingWatch>(x => x.Version),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by product id
                    version = Int64.Parse(f.Value.ToString());
                    break;
                }
            }

            IEnumerable<DomainMostSignaledItem> data = await _cassandraStatisticsService.GetMostViewedForLastWeekAsync(StatisticsSpaces.Projects, MemoryPageSize, version);
            var pageData = new List<DomainMostSignaledItem>();
            int count = 0;
            int skip = filter.Skip.HasValue ? filter.Skip.Value : 0;
            var uniqueIds = new HashSet<string>();
            var result = new List<TrendingWatch>();
            foreach (DomainMostSignaledItem mostSignaledItem in data)
            {
                if (result.Count >= MemoryPageSize || count > MemoryPageSize*MaxScanPages)
                {
                    // memory limit reached
                    break;
                }

                if (filter.Take.HasValue && result.Count >= filter.Take.Value + skip)
                {
                    // all items loaded
                    break;
                }

                // checking uniqueness
                if (!uniqueIds.Contains(mostSignaledItem.ItemId))
                {
                    // there can be duplicated ItemIds with different Count values due to race conditions
                    uniqueIds.Add(mostSignaledItem.ItemId);
                }
                else
                {
                    // skipping duplicate
                    count++;
                    continue;
                }

                // adding to memory page
                pageData.Add(mostSignaledItem);

                // checking limits
                if ((count > 0 && count%MemoryPageSize == 0) ||
                    (filter.Take.HasValue && pageData.Count >= filter.Take.Value))
                {
                    // loading and filtering items
                    List<TrendingWatch> page = await LoadAndFilterDataAsync(pageData, filter);
                    result.AddRange(page);
                    pageData.Clear();
                }

                count++;
            }

            if (pageData.Count > 0)
            {
                // loading last page
                List<TrendingWatch> page = await LoadAndFilterDataAsync(pageData, filter);
                result.AddRange(page);
                pageData.Clear();
            }

            // Skip
            result = result.Skip(skip).ToList();

            // Take
            if (filter.Take.HasValue)
            {
                result = result.Take(filter.Take.Value).ToList();
            }

            return result;
        }

        #region helpers

        private async Task<List<TrendingWatch>> LoadAndFilterDataAsync(IEnumerable<DomainMostSignaledItem> pageData, DataQueryOptions filter)
        {
            // Preparing
            int? productId = null;
            WatchState? videoState = null;
            List<DataFilterRule> filters = filter.Filters.Where(f => f.Value != null).ToList();
            foreach (DataFilterRule dataFilterRule in filters)
            {
                DataFilterRule f = dataFilterRule;

                if (string.Compare(f.Name, NameOfHelper.PropertyName<TrendingWatch>(x => x.Generator),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by product id
                    productId = Int32.Parse(f.Value.ToString());
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<TrendingWatch>(x => x.State),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by video state
                    videoState = (WatchState)Enum.Parse(typeof (WatchState), f.Value.ToString());
                }
            }


            // Loading
            List<DomainMostSignaledItem> items = pageData.ToList();
            List<Watch> watches = await _watchProjectService.GetByIdsAsync(items.Select(i => i.ItemId).ToArray(), string.Empty);


            // Processing projects
            var filtered = new List<TrendingWatch>();
            foreach (DomainMostSignaledItem i in items)
            {
                DomainMostSignaledItem item = i;
                Watch watch = watches.FirstOrDefault(w => w.Id == item.ItemId);

                // Display only existing & public videos
                if (watch == null || watch.Access != ProjectAccess.Public)
                {
                    continue;
                }

                // filtering by product id
                if (productId.HasValue)
                {
                    if (watch.Generator != productId.Value)
                    {
                        continue;
                    }
                }

                // filterring by state
                if (videoState.HasValue)
                {
                    if (watch.State != videoState.Value)
                    {
                        continue;
                    }
                }

                TrendingWatch trendingWatch = _mapper.Map<Watch, TrendingWatch>(watch);
                trendingWatch.Version = item.Version;

                filtered.Add(trendingWatch);
            }

            return filtered;
        }

        #endregion
    }
}