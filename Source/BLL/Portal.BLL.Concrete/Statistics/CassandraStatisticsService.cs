// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Statistics;
using Portal.DAL.Entities.Statistics;
using Portal.DAL.Statistics;
using Portal.Domain.StatisticContext;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Statistics
{
    public class CassandraStatisticsService : ICassandraStatisticsService
    {
        private readonly IAffinityGroupCountsRepository _affinityGroupCountsRepository;
        private readonly IAffinityGroupItemCountsRepository _affinityGroupItemCountsRepository;
        private readonly IAffinityGroupMostSignaledRepository _affinityGroupMostSignaledRepository;
        private readonly IAffinityGroupMostSignaledVersionRepository _affinityGroupMostSignaledVersionRepository;
        private readonly IItemCountsRepository _itemCountsRepository;
        private readonly IItemSignalsRepository _itemSignalsRepository;
        private readonly IMapper _mapper;
        private readonly ITimeSeriesRollupsDayRepository _timeSeriesRollupsDayRepository;
        private readonly ITimeSeriesRollupsHourRepository _timeSeriesRollupsHourRepository;
        private readonly ITimeSeriesRollupsMinuteRepository _timeSeriesRollupsMinuteRepository;
        private readonly IUserCountsRepository _userCountsRepository;
        private readonly IUserSignalsRepository _userSignalsRepository;
        private readonly IUserSignalsUnorderedRepository _userSignalsUnorderedRepository;

        public CassandraStatisticsService(IAffinityGroupCountsRepository affinityGroupCountsRepository,
            IAffinityGroupItemCountsRepository affinityGroupItemCountsRepository,
            IAffinityGroupMostSignaledRepository affinityGroupMostSignaledRepository,
            IAffinityGroupMostSignaledVersionRepository affinityGroupMostSignaledVersionRepository,
            IItemCountsRepository itemCountsRepository,
            IItemSignalsRepository itemSignalsRepository,
            IUserCountsRepository userCountsRepository,
            IUserSignalsRepository userSignalsRepository,
            IUserSignalsUnorderedRepository userSignalsUnorderedRepository,
            ITimeSeriesRollupsDayRepository timeSeriesRollupsDayRepository,
            ITimeSeriesRollupsHourRepository timeSeriesRollupsHourRepository,
            ITimeSeriesRollupsMinuteRepository timeSeriesRollupsMinuteRepository,
            IMapper mapper
            )
        {
            _affinityGroupCountsRepository = affinityGroupCountsRepository;
            _affinityGroupItemCountsRepository = affinityGroupItemCountsRepository;
            _affinityGroupMostSignaledRepository = affinityGroupMostSignaledRepository;
            _affinityGroupMostSignaledVersionRepository = affinityGroupMostSignaledVersionRepository;
            _itemCountsRepository = itemCountsRepository;
            _itemSignalsRepository = itemSignalsRepository;
            _userCountsRepository = userCountsRepository;
            _userSignalsRepository = userSignalsRepository;
            _userSignalsUnorderedRepository = userSignalsUnorderedRepository;
            _timeSeriesRollupsDayRepository = timeSeriesRollupsDayRepository;
            _timeSeriesRollupsHourRepository = timeSeriesRollupsHourRepository;
            _timeSeriesRollupsMinuteRepository = timeSeriesRollupsMinuteRepository;
            _mapper = mapper;
        }

        public async Task<DomainItemCounts> GetItemCountsAsync(string space, string itemId)
        {
            // Building composite itemId to isolated different spaces of statistics
            itemId = BuildGlobalId(space, itemId);

            ItemCountsEntity result = await _itemCountsRepository.GetAsync(itemId);
            return _mapper.Map<ItemCountsEntity, DomainItemCounts>(result);
        }

        public async Task<DomainUserCounts> GetUserCountsAsync(string space, string userId)
        {
            // Building composite userId to isolated different spaces of statistics
            userId = BuildGlobalId(space, userId);

            UserCountsEntity result = await _userCountsRepository.GetAsync(userId);
            return _mapper.Map<UserCountsEntity, DomainUserCounts>(result);
        }

        public Task AddViewAsync(string space, string itemId, string userId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                throw new ArgumentNullException("itemId");
            }

            // Writing to all required key families
            const SignalType signal = SignalType.View;
            DateTime dateTime = DateTime.UtcNow;
            var tasks = new List<Task>();


            // 1. Item and user relations

            // Building composite row key to isolated different spaces of statistics
            string itemRowKey = BuildGlobalId(space, itemId);

            // Item cumulative counts
            tasks.Add(_itemCountsRepository.IncViewsAsync(itemRowKey));

            if (!string.IsNullOrEmpty(userId))
            {
                // Building composite row key to isolated different spaces of statistics
                string userRowKey = BuildGlobalId(space, userId);

                // User cumulative counts
                tasks.Add(_userCountsRepository.IncViewsAsync(userRowKey));

                // Users who viewed an item
                tasks.Add(_itemSignalsRepository.AddAsync(new ItemSignalsInsertDeleteOptions { ItemId = itemRowKey, UserId = userId, DateTime = dateTime, Signal = signal }));

                // User viewed items
                tasks.Add(Task.Run(() =>
                {
                    // add new view signal
                    var addTasks = new List<Task>();

                    var userSignal = new UserSignalsInsertDeleteOptions { ItemId = itemId, UserId = userRowKey, DateTime = dateTime, Signal = signal };
                    addTasks.Add(_userSignalsRepository.AddAsync(userSignal));
                    addTasks.Add(_userSignalsUnorderedRepository.AddAsync(userSignal));

                    return Task.WhenAll(addTasks);
                }));
            }


            // 2. Affinity groups
            IEnumerable<string> affinityGroups = BuildAffinityGroups(dateTime);
            foreach (string affinityGroup in affinityGroups)
            {
                // Building composite row keys to isolated different spaces of statistics
                string groupId = BuildGlobalId(space, affinityGroup);

                // Affinity group most viewed
                tasks.Add(Task.Run(async () =>
                {
                    // getting current version of most viewed items
                    AffinityGroupMostSignaledVersionEntity currentVersion = await _affinityGroupMostSignaledVersionRepository.GetAsync(groupId, signal);

                    // getting item counts
                    IEnumerable<AffinityGroupItemCountsEntity> itemCounts = await _affinityGroupItemCountsRepository.GetSequenceAsync(groupId, signal, 1000);

                    // building new version of most viewed items
                    Dictionary<string, AffinityGroupItemCountsEntity> itemCountsDict = itemCounts.ToDictionary(i => i.ItemId);
                    var insertOptions = new List<AffinityGroupMostSignaledInsertOptions>();
                    foreach (var i in itemCountsDict)
                    {
                        var insertItem = new AffinityGroupMostSignaledInsertOptions { ItemId = i.Key, Count = i.Value.Count };
                        if (insertItem.ItemId == itemId)
                        {
                            insertItem.Count = insertItem.Count + 1;
                        }

                        insertOptions.Add(insertItem);
                    }

                    // ensuring current item
                    if (!itemCountsDict.ContainsKey(itemId))
                    {
                        insertOptions.Add(new AffinityGroupMostSignaledInsertOptions { ItemId = itemId, Count = 1 });
                    }

                    await _affinityGroupMostSignaledRepository.AddAsync(groupId, signal, currentVersion.Version + 1, insertOptions);

                    // incrementing counter of itemId
                    await _affinityGroupItemCountsRepository.IncAsync(groupId, signal, itemId);

                    // incrementing version
                    await _affinityGroupMostSignaledVersionRepository.UpdateAsync(groupId, signal, currentVersion.Version + 1);
                }));

                // incrementing affinity group cumulative counts
                tasks.Add(_affinityGroupCountsRepository.IncAsync(groupId, signal));
            }


            // 3. Time series
            string eventId = BuildTimeSeriesRowKey(itemRowKey, signal);

            // Rollups
            var rollupOptions = new TimeSeriesRollupsInsertOptions
            {
                EventId = eventId,
                DateTime = dateTime
            };

            // Rollups for days
            tasks.Add(_timeSeriesRollupsDayRepository.IncAsync(rollupOptions));

            // Rollups for hour
            tasks.Add(_timeSeriesRollupsHourRepository.IncAsync(rollupOptions));

            // Rollups for minute
            tasks.Add(_timeSeriesRollupsMinuteRepository.IncAsync(rollupOptions));


            return Task.WhenAll(tasks);
        }

        public async Task<IEnumerable<DomainItemSignal>> GetItemViewsSequenceAsync(string space, string itemId, int pageSize)
        {
            // Building composite itemId to isolated different spaces of statistics
            itemId = BuildGlobalId(space, itemId);

            IEnumerable<ItemSignalsEntity> result = await _itemSignalsRepository.GetSequenceAsync(itemId, SignalType.View, pageSize);
            return result.Select(s => _mapper.Map<ItemSignalsEntity, DomainItemSignal>(s));
        }

        public async Task<IEnumerable<DomainUserSignal>> GetUserViewsSequenceAsync(string space, string userId, int pageSize)
        {
            // Building composite userId to isolated different spaces of statistics
            userId = BuildGlobalId(space, userId);

            IEnumerable<UserSignalsEntity> result = await _userSignalsRepository.GetSequenceAsync(userId, SignalType.View, pageSize);
            return result.Select(s => _mapper.Map<UserSignalsEntity, DomainUserSignal>(s));
        }

        public async Task<IEnumerable<DomainMostSignaledItem>> GetMostViewedForLastWeekAsync(string space, int pageSize, long? version)
        {
            DateTime dateTime = DateTime.UtcNow;
            string groupId = string.Format("{0:yyyy-MM-dd}|{1:yyyy-MM-dd}", dateTime.AddDays(-7), dateTime);

            // Building composite groupId to isolated different spaces of statistics
            groupId = BuildGlobalId(space, groupId);

            // Retrieve most signaled items
            if (!version.HasValue)
            {
                AffinityGroupMostSignaledVersionEntity v = await _affinityGroupMostSignaledVersionRepository.GetAsync(groupId, SignalType.View);
                version = v.Version;
            }

            IEnumerable<AffinityGroupMostSignaledEntity> result = await _affinityGroupMostSignaledRepository.GetSequenceAsync(groupId, SignalType.View, version.Value, pageSize);

            return result.Select(i => _mapper.Map<Tuple<AffinityGroupMostSignaledEntity, long>, DomainMostSignaledItem>(new Tuple<AffinityGroupMostSignaledEntity, long>(i, version.Value)));
        }

        public async Task AddLikeAsync(string space, string itemId, string userId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                throw new ArgumentNullException("itemId");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }

            // Writing to all required key families
            const SignalType signal = SignalType.Like;
            DateTime dateTime = DateTime.UtcNow;
            var tasks = new List<Task>();


            // 1. Item and user relations

            // Building composite row key to isolated different spaces of statistics
            string itemRowKey = BuildGlobalId(space, itemId);
            string userRowKey = BuildGlobalId(space, userId);

            // Item cumulative counts
            tasks.Add(_itemCountsRepository.IncLikesAsync(itemRowKey));

            // User cumulative counts
            tasks.Add(_userCountsRepository.IncLikesAsync(userRowKey));

            // User likes
            tasks.Add(Task.Run(() =>
            {
                var addTasks = new List<Task>();
                var userSignal = new UserSignalsInsertDeleteOptions { ItemId = itemId, UserId = userRowKey, DateTime = dateTime, Signal = signal };

                addTasks.Add(_userSignalsRepository.AddAsync(userSignal));
                addTasks.Add(_userSignalsUnorderedRepository.AddAsync(userSignal));

                return Task.WhenAll(addTasks);
            }));

            // Users who liked an item
            tasks.Add(_itemSignalsRepository.AddAsync(new ItemSignalsInsertDeleteOptions { ItemId = itemRowKey, UserId = userId, DateTime = dateTime, Signal = signal }));


            // 2. Affinity groups
            IEnumerable<string> affinityGroups = BuildAffinityGroups(dateTime);
            foreach (string affinityGroup in affinityGroups)
            {
                // Building composite row keys to isolated different spaces of statistics
                string groupId = BuildGlobalId(space, affinityGroup);

                // Affinity group most liked
                tasks.Add(Task.Run(async () =>
                {
                    // getting current version of most signaled items
                    AffinityGroupMostSignaledVersionEntity currentVersion = await _affinityGroupMostSignaledVersionRepository.GetAsync(groupId, signal);

                    // getting item counts
                    IEnumerable<AffinityGroupItemCountsEntity> itemCounts = await _affinityGroupItemCountsRepository.GetSequenceAsync(groupId, signal, 1000);

                    // building new version of most liked
                    Dictionary<string, AffinityGroupItemCountsEntity> itemCountsDict = itemCounts.ToDictionary(i => i.ItemId);
                    var insertOptions = new List<AffinityGroupMostSignaledInsertOptions>();
                    foreach (var i in itemCountsDict)
                    {
                        var insertItem = new AffinityGroupMostSignaledInsertOptions { ItemId = i.Key, Count = i.Value.Count };
                        if (insertItem.ItemId == itemId)
                        {
                            insertItem.Count = insertItem.Count + 1;
                        }

                        insertOptions.Add(insertItem);
                    }

                    // ensuring current item
                    if (!itemCountsDict.ContainsKey(itemId))
                    {
                        insertOptions.Add(new AffinityGroupMostSignaledInsertOptions { ItemId = itemId, Count = 1 });
                    }

                    await _affinityGroupMostSignaledRepository.AddAsync(groupId, signal, currentVersion.Version + 1, insertOptions);

                    // incrementing counter of itemId
                    await _affinityGroupItemCountsRepository.IncAsync(groupId, signal, itemId);

                    // incrementing version
                    await _affinityGroupMostSignaledVersionRepository.UpdateAsync(groupId, signal, currentVersion.Version + 1);
                }));

                // incrementing affinity group cumulative counts
                tasks.Add(_affinityGroupCountsRepository.IncAsync(groupId, signal));
            }


            // 3. Time series
            string eventId = BuildTimeSeriesRowKey(itemRowKey, signal);

            // Rollups
            var rollupOptions = new TimeSeriesRollupsInsertOptions
            {
                EventId = eventId,
                DateTime = dateTime
            };

            // Rollups for days
            tasks.Add(_timeSeriesRollupsDayRepository.IncAsync(rollupOptions));

            // Rollups for hour
            tasks.Add(_timeSeriesRollupsHourRepository.IncAsync(rollupOptions));

            // Rollups for minute
            tasks.Add(_timeSeriesRollupsMinuteRepository.IncAsync(rollupOptions));


            await Task.WhenAll(tasks);
        }

        public async Task DeleteLikeAsync(string space, string itemId, string userId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                throw new ArgumentNullException("itemId");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }

            // Writing to all required key families
            const SignalType signal = SignalType.Like;
            DateTime dateTime = DateTime.UtcNow;
            var tasks = new List<Task>();


            // 1. Item and user relations

            // Building composite row key to isolated different spaces of statistics
            string itemRowKey = BuildGlobalId(space, itemId);
            string userRowKey = BuildGlobalId(space, userId);

            // Item cumulative counts
            tasks.Add(_itemCountsRepository.DecLikesAsync(itemRowKey));

            // User cumulative counts
            tasks.Add(_userCountsRepository.DecLikesAsync(userRowKey));

            // User likes
            tasks.Add(Task.Run(() =>
            {
                // cancel like
                var deleteTasks = new List<Task>();
                var deleteOptions = new UserSignalsInsertDeleteOptions { ItemId = itemId, Signal = signal, UserId = userRowKey, DateTime = dateTime };

                deleteTasks.Add(_userSignalsUnorderedRepository.DeleteAsync(deleteOptions));
                deleteTasks.Add(_userSignalsRepository.DeleteAsync(deleteOptions));

                return Task.WhenAll(deleteTasks);
            }));

            // Users who liked an item
            tasks.Add(_itemSignalsRepository.DeleteAsync(new ItemSignalsInsertDeleteOptions { ItemId = itemRowKey, UserId = userId, Signal = signal, DateTime = dateTime }));


            // 2. Affinity groups
            IEnumerable<string> affinityGroups = BuildAffinityGroups(dateTime);
            foreach (string affinityGroup in affinityGroups)
            {
                // Building composite row keys to isolated different spaces of statistics
                string groupId = BuildGlobalId(space, affinityGroup);

                // Affinity group most liked
                tasks.Add(Task.Run(async () =>
                {
                    // getting current version of most signaled items
                    AffinityGroupMostSignaledVersionEntity currentVersion = await _affinityGroupMostSignaledVersionRepository.GetAsync(groupId, signal);

                    // getting item counts
                    IEnumerable<AffinityGroupItemCountsEntity> itemCounts = await _affinityGroupItemCountsRepository.GetSequenceAsync(groupId, signal, 1000);

                    // building new version of most liked
                    Dictionary<string, AffinityGroupItemCountsEntity> itemCountsDict = itemCounts.ToDictionary(i => i.ItemId);
                    var insertOptions = new List<AffinityGroupMostSignaledInsertOptions>();
                    foreach (var i in itemCountsDict)
                    {
                        var insertItem = new AffinityGroupMostSignaledInsertOptions { ItemId = i.Key, Count = i.Value.Count };
                        if (insertItem.ItemId == itemId)
                        {
                            insertItem.Count = insertItem.Count - 1;
                        }

                        insertOptions.Add(insertItem);
                    }

                    await _affinityGroupMostSignaledRepository.AddAsync(groupId, signal, currentVersion.Version + 1, insertOptions);

                    // decrementing counter of itemId
                    await _affinityGroupItemCountsRepository.DecAsync(groupId, signal, itemId);

                    // incrementing version
                    await _affinityGroupMostSignaledVersionRepository.UpdateAsync(groupId, signal, currentVersion.Version + 1);
                }));

                // decrementing affinity group cumulative counts
                tasks.Add(_affinityGroupCountsRepository.DecAsync(groupId, signal));
            }

            await Task.WhenAll(tasks);
        }

        public async Task<IEnumerable<DomainItemSignal>> GetItemLikesSequenceAsync(string space, string itemId, int pageSize)
        {
            // Building composite itemId to isolated different spaces of statistics
            itemId = BuildGlobalId(space, itemId);

            IEnumerable<ItemSignalsEntity> antiResults = await _itemSignalsRepository.GetAntiSequenceAsync(itemId, SignalType.Like, pageSize);
            IEnumerable<ItemSignalsEntity> results = await _itemSignalsRepository.GetSequenceAsync(itemId, SignalType.Like, pageSize);

            // Filter cancelled signals
            ILookup<string, ItemSignalsEntity> filter = antiResults.ToLookup(i => i.UserId);
            results = results.Where(r => !filter.Contains(r.UserId) || filter[r.UserId].All(i => i.DateTime < r.DateTime));

            // Project results
            return results.Select(s => _mapper.Map<ItemSignalsEntity, DomainItemSignal>(s));
        }

        public async Task<IEnumerable<DomainUserSignal>> GetUserLikesSequenceAsync(string space, string userId, int pageSize)
        {
            // Building composite userId to isolate different spaces of statistics
            userId = BuildGlobalId(space, userId);

            IEnumerable<UserSignalsEntity> antiResults = await _userSignalsRepository.GetAntiSequenceAsync(userId, SignalType.Like, pageSize);
            IEnumerable<UserSignalsEntity> results = await _userSignalsRepository.GetSequenceAsync(userId, SignalType.Like, pageSize);

            // Filter cancelled signals
            ILookup<string, UserSignalsEntity> filter = antiResults.ToLookup(r => r.ItemId);
            results = results.Where(r => !filter.Contains(r.ItemId) || filter[r.ItemId].All(i => i.DateTime < r.DateTime));

            // Project results
            return results.Select(s => _mapper.Map<UserSignalsEntity, DomainUserSignal>(s));
        }

        public async Task<bool> IsLikedAsync(string space, string itemId, string userId)
        {
            // Building composite userId to isolate different spaces of statistics
            userId = BuildGlobalId(space, userId);

            UserSignalsUnorderedEntity result = await _userSignalsUnorderedRepository.GetAsync(userId, SignalType.Like, itemId);
            return result != null;
        }

        public async Task AddDislikeAsync(string space, string itemId, string userId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                throw new ArgumentNullException("itemId");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }

            // Writing to all required key families
            const SignalType signal = SignalType.Dislike;
            DateTime dateTime = DateTime.UtcNow;
            var tasks = new List<Task>();


            // 1. Item and user relations

            // Building composite row key to isolated different spaces of statistics
            string itemRowKey = BuildGlobalId(space, itemId);
            string userRowKey = BuildGlobalId(space, userId);

            // Item cumulative counts
            tasks.Add(_itemCountsRepository.IncDislikesAsync(itemRowKey));

            // User cumulative counts
            tasks.Add(_userCountsRepository.IncDislikesAsync(userRowKey));

            // User dislikes
            tasks.Add(Task.Run(() =>
            {
                var addTasks = new List<Task>();
                var userSignal = new UserSignalsInsertDeleteOptions { ItemId = itemId, UserId = userRowKey, DateTime = dateTime, Signal = signal };

                addTasks.Add(_userSignalsRepository.AddAsync(userSignal));
                addTasks.Add(_userSignalsUnorderedRepository.AddAsync(userSignal));

                return Task.WhenAll(addTasks);
            }));

            // Users who disliked an item
            tasks.Add(_itemSignalsRepository.AddAsync(new ItemSignalsInsertDeleteOptions { ItemId = itemRowKey, UserId = userId, DateTime = dateTime, Signal = signal }));


            // 2. Affinity groups
            IEnumerable<string> affinityGroups = BuildAffinityGroups(dateTime);
            foreach (string affinityGroup in affinityGroups)
            {
                // Building composite row keys to isolated different spaces of statistics
                string groupId = BuildGlobalId(space, affinityGroup);

                // Affinity group most liked
                tasks.Add(Task.Run(async () =>
                {
                    // getting current version of most signaled items
                    AffinityGroupMostSignaledVersionEntity currentVersion = await _affinityGroupMostSignaledVersionRepository.GetAsync(groupId, signal);

                    // getting item counts
                    IEnumerable<AffinityGroupItemCountsEntity> itemCounts = await _affinityGroupItemCountsRepository.GetSequenceAsync(groupId, signal, 1000);

                    // building new version of most liked
                    Dictionary<string, AffinityGroupItemCountsEntity> itemCountsDict = itemCounts.ToDictionary(i => i.ItemId);
                    var insertOptions = new List<AffinityGroupMostSignaledInsertOptions>();
                    foreach (var i in itemCountsDict)
                    {
                        var insertItem = new AffinityGroupMostSignaledInsertOptions { ItemId = i.Key, Count = i.Value.Count };
                        if (insertItem.ItemId == itemId)
                        {
                            insertItem.Count = insertItem.Count + 1;
                        }

                        insertOptions.Add(insertItem);
                    }

                    // ensuring current item
                    if (!itemCountsDict.ContainsKey(itemId))
                    {
                        insertOptions.Add(new AffinityGroupMostSignaledInsertOptions { ItemId = itemId, Count = 1 });
                    }

                    await _affinityGroupMostSignaledRepository.AddAsync(groupId, signal, currentVersion.Version + 1, insertOptions);

                    // incrementing counter of itemId
                    await _affinityGroupItemCountsRepository.IncAsync(groupId, signal, itemId);

                    // incrementing version
                    await _affinityGroupMostSignaledVersionRepository.UpdateAsync(groupId, signal, currentVersion.Version + 1);
                }));

                // incrementing affinity group cumulative counts
                tasks.Add(_affinityGroupCountsRepository.IncAsync(groupId, signal));
            }


            // 3. Time series
            string eventId = BuildTimeSeriesRowKey(itemRowKey, signal);

            // Rollups
            var rollupOptions = new TimeSeriesRollupsInsertOptions
            {
                EventId = eventId,
                DateTime = dateTime
            };

            // Rollups for days
            tasks.Add(_timeSeriesRollupsDayRepository.IncAsync(rollupOptions));

            // Rollups for hour
            tasks.Add(_timeSeriesRollupsHourRepository.IncAsync(rollupOptions));

            // Rollups for minute
            tasks.Add(_timeSeriesRollupsMinuteRepository.IncAsync(rollupOptions));


            await Task.WhenAll(tasks);
        }

        public async Task DeleteDislikeAsync(string space, string itemId, string userId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                throw new ArgumentNullException("itemId");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }

            // Writing to all required key families
            const SignalType signal = SignalType.Dislike;
            DateTime dateTime = DateTime.UtcNow;
            var tasks = new List<Task>();


            // 1. Item and user relations

            // Building composite row key to isolated different spaces of statistics
            string itemRowKey = BuildGlobalId(space, itemId);
            string userRowKey = BuildGlobalId(space, userId);

            // Item cumulative counts
            tasks.Add(_itemCountsRepository.DecDislikesAsync(itemRowKey));

            // User cumulative counts
            tasks.Add(_userCountsRepository.DecDislikesAsync(userRowKey));

            // User dislikes
            tasks.Add(Task.Run(() =>
            {
                // cancel dislike
                var deleteTasks = new List<Task>();
                var deleteOptions = new UserSignalsInsertDeleteOptions { ItemId = itemId, Signal = signal, UserId = userRowKey, DateTime = dateTime };

                deleteTasks.Add(_userSignalsUnorderedRepository.DeleteAsync(deleteOptions));
                deleteTasks.Add(_userSignalsRepository.DeleteAsync(deleteOptions));

                return Task.WhenAll(deleteTasks);
            }));

            // Users who disliked an item
            tasks.Add(_itemSignalsRepository.DeleteAsync(new ItemSignalsInsertDeleteOptions { ItemId = itemRowKey, UserId = userId, Signal = signal, DateTime = dateTime }));


            // 2. Affinity groups
            IEnumerable<string> affinityGroups = BuildAffinityGroups(dateTime);
            foreach (string affinityGroup in affinityGroups)
            {
                // Building composite row keys to isolated different spaces of statistics
                string groupId = BuildGlobalId(space, affinityGroup);

                // Affinity group most liked
                tasks.Add(Task.Run(async () =>
                {
                    // getting current version of most signaled items
                    AffinityGroupMostSignaledVersionEntity currentVersion = await _affinityGroupMostSignaledVersionRepository.GetAsync(groupId, signal);

                    // getting item counts
                    IEnumerable<AffinityGroupItemCountsEntity> itemCounts = await _affinityGroupItemCountsRepository.GetSequenceAsync(groupId, signal, 1000);

                    // building new version of most liked
                    Dictionary<string, AffinityGroupItemCountsEntity> itemCountsDict = itemCounts.ToDictionary(i => i.ItemId);
                    var insertOptions = new List<AffinityGroupMostSignaledInsertOptions>();
                    foreach (var i in itemCountsDict)
                    {
                        var insertItem = new AffinityGroupMostSignaledInsertOptions { ItemId = i.Key, Count = i.Value.Count };
                        if (insertItem.ItemId == itemId)
                        {
                            insertItem.Count = insertItem.Count - 1;
                        }

                        insertOptions.Add(insertItem);
                    }

                    await _affinityGroupMostSignaledRepository.AddAsync(groupId, signal, currentVersion.Version + 1, insertOptions);

                    // decrementing counter of itemId
                    await _affinityGroupItemCountsRepository.DecAsync(groupId, signal, itemId);

                    // incrementing version
                    await _affinityGroupMostSignaledVersionRepository.UpdateAsync(groupId, signal, currentVersion.Version + 1);
                }));

                // decrementing affinity group cumulative counts
                tasks.Add(_affinityGroupCountsRepository.DecAsync(groupId, signal));
            }

            await Task.WhenAll(tasks);
        }

        public async Task<IEnumerable<DomainItemSignal>> GetItemDislikesSequenceAsync(string space, string itemId, int pageSize)
        {
            // Building composite itemId to isolated different spaces of statistics
            itemId = BuildGlobalId(space, itemId);

            IEnumerable<ItemSignalsEntity> antiResults = await _itemSignalsRepository.GetAntiSequenceAsync(itemId, SignalType.Dislike, pageSize);
            IEnumerable<ItemSignalsEntity> results = await _itemSignalsRepository.GetSequenceAsync(itemId, SignalType.Dislike, pageSize);

            // Filter cancelled signals
            ILookup<string, ItemSignalsEntity> filter = antiResults.ToLookup(i => i.UserId);
            results = results.Where(r => !filter.Contains(r.UserId) || filter[r.UserId].All(i => i.DateTime < r.DateTime));

            // Project results
            return results.Select(s => _mapper.Map<ItemSignalsEntity, DomainItemSignal>(s));
        }

        public async Task<IEnumerable<DomainUserSignal>> GetUserDislikesSequenceAsync(string space, string userId, int pageSize)
        {
            // Building composite userId to isolate different spaces of statistics
            userId = BuildGlobalId(space, userId);

            IEnumerable<UserSignalsEntity> antiResults = await _userSignalsRepository.GetAntiSequenceAsync(userId, SignalType.Dislike, pageSize);
            IEnumerable<UserSignalsEntity> results = await _userSignalsRepository.GetSequenceAsync(userId, SignalType.Dislike, pageSize);

            // Filter cancelled signals
            ILookup<string, UserSignalsEntity> filter = antiResults.ToLookup(r => r.ItemId);
            results = results.Where(r => !filter.Contains(r.ItemId) || filter[r.ItemId].All(i => i.DateTime < r.DateTime));

            // Project results
            return results.Select(s => _mapper.Map<UserSignalsEntity, DomainUserSignal>(s));
        }

        public async Task<bool> IsDislikedAsync(string space, string itemId, string userId)
        {
            // Building composite userId to isolate different spaces of statistics
            userId = BuildGlobalId(space, userId);

            UserSignalsUnorderedEntity result = await _userSignalsUnorderedRepository.GetAsync(userId, SignalType.Dislike, itemId);
            return result != null;
        }

        public Task AddAbuseAsync(string space, string itemId, string userId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                throw new ArgumentNullException("itemId");
            }

            // Writing to all required key families
            const SignalType signal = SignalType.Abuse;
            DateTime dateTime = DateTime.UtcNow;
            var tasks = new List<Task>();


            // 1. Item and user relations

            // Building composite row key to isolated different spaces of statistics
            string itemRowKey = BuildGlobalId(space, itemId);

            // Item cumulative counts
            tasks.Add(_itemCountsRepository.IncAbuseAsync(itemRowKey));

            if (!string.IsNullOrEmpty(userId))
            {
                // Building composite row key to isolated different spaces of statistics
                string userRowKey = BuildGlobalId(space, userId);

                // User cumulative counts
                tasks.Add(_userCountsRepository.IncAbuseAsync(userRowKey));

                // Users who report abuse for item
                tasks.Add(_itemSignalsRepository.AddAsync(new ItemSignalsInsertDeleteOptions { ItemId = itemRowKey, UserId = userId, DateTime = dateTime, Signal = signal }));

                // Reported by user items
                tasks.Add(Task.Run(() =>
                {
                    // add new view signal
                    var addTasks = new List<Task>();

                    var userSignal = new UserSignalsInsertDeleteOptions { ItemId = itemId, UserId = userRowKey, DateTime = dateTime, Signal = signal };
                    addTasks.Add(_userSignalsRepository.AddAsync(userSignal));
                    addTasks.Add(_userSignalsUnorderedRepository.AddAsync(userSignal));

                    return Task.WhenAll(addTasks);
                }));
            }


            // 2. Affinity groups
            IEnumerable<string> affinityGroups = BuildAffinityGroups(dateTime);
            foreach (string affinityGroup in affinityGroups)
            {
                // Building composite row keys to isolated different spaces of statistics
                string groupId = BuildGlobalId(space, affinityGroup);

                // Affinity group most viewed
                tasks.Add(Task.Run(async () =>
                {
                    // getting current version of most viewed items
                    AffinityGroupMostSignaledVersionEntity currentVersion = await _affinityGroupMostSignaledVersionRepository.GetAsync(groupId, signal);

                    // getting item counts
                    IEnumerable<AffinityGroupItemCountsEntity> itemCounts = await _affinityGroupItemCountsRepository.GetSequenceAsync(groupId, signal, 1000);

                    // building new version of most viewed items
                    Dictionary<string, AffinityGroupItemCountsEntity> itemCountsDict = itemCounts.ToDictionary(i => i.ItemId);
                    var insertOptions = new List<AffinityGroupMostSignaledInsertOptions>();
                    foreach (var i in itemCountsDict)
                    {
                        var insertItem = new AffinityGroupMostSignaledInsertOptions { ItemId = i.Key, Count = i.Value.Count };
                        if (insertItem.ItemId == itemId)
                        {
                            insertItem.Count = insertItem.Count + 1;
                        }

                        insertOptions.Add(insertItem);
                    }

                    // ensuring current item
                    if (!itemCountsDict.ContainsKey(itemId))
                    {
                        insertOptions.Add(new AffinityGroupMostSignaledInsertOptions { ItemId = itemId, Count = 1 });
                    }

                    await _affinityGroupMostSignaledRepository.AddAsync(groupId, signal, currentVersion.Version + 1, insertOptions);

                    // incrementing counter of itemId
                    await _affinityGroupItemCountsRepository.IncAsync(groupId, signal, itemId);

                    // incrementing version
                    await _affinityGroupMostSignaledVersionRepository.UpdateAsync(groupId, signal, currentVersion.Version + 1);
                }));

                // incrementing affinity group cumulative counts
                tasks.Add(_affinityGroupCountsRepository.IncAsync(groupId, signal));
            }


            // 3. Time series
            string eventId = BuildTimeSeriesRowKey(itemRowKey, signal);

            // Rollups
            var rollupOptions = new TimeSeriesRollupsInsertOptions
            {
                EventId = eventId,
                DateTime = dateTime
            };

            // Rollups for days
            tasks.Add(_timeSeriesRollupsDayRepository.IncAsync(rollupOptions));

            // Rollups for hour
            tasks.Add(_timeSeriesRollupsHourRepository.IncAsync(rollupOptions));

            // Rollups for minute
            tasks.Add(_timeSeriesRollupsMinuteRepository.IncAsync(rollupOptions));


            return Task.WhenAll(tasks);
        }

        public async Task<IEnumerable<DomainItemSignal>> GetItemAbuseSequenceAsync(string space, string itemId, int pageSize)
        {
            // Building composite itemId to isolated different spaces of statistics
            itemId = BuildGlobalId(space, itemId);

            IEnumerable<ItemSignalsEntity> result = await _itemSignalsRepository.GetSequenceAsync(itemId, SignalType.Abuse, pageSize);
            return result.Select(s => _mapper.Map<ItemSignalsEntity, DomainItemSignal>(s));
        }

        public async Task<bool> IsAbuseReportedAsync(string space, string itemId, string userId)
        {
            // Building composite userId to isolate different spaces of statistics
            userId = BuildGlobalId(space, userId);

            UserSignalsUnorderedEntity result = await _userSignalsUnorderedRepository.GetAsync(userId, SignalType.Abuse, itemId);
            return result != null;
        }

        #region helpers

        private static IEnumerable<string> BuildAffinityGroups(DateTime dateTime)
        {
            var groups = new List<string>();

            // Last 7 days groups
            DateTime dateTo = dateTime.Date;
            for (int i = 0; i <= 7; i++)
            {
                DateTime dateFrom = dateTo.AddDays(-7);
                string groupId = string.Format("{0:yyyy-MM-dd}|{1:yyyy-MM-dd}", dateFrom, dateTo);
                groups.Add(groupId);

                dateTo = dateTo.AddDays(1);
            }

            return groups;
        }

        private static string BuildGlobalId(string space, string itemId)
        {
            return space + "|" + itemId;
        }

        private static string BuildTimeSeriesRowKey(string itemId, SignalType signal)
        {
            return itemId + "|" + signal;
        }

        #endregion
    }
}