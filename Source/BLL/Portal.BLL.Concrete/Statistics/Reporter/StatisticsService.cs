// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Linq;
using MongoRepository;
using Portal.BLL.Statistics.Helper;
using Portal.BLL.Statistics.Reporter;
using Portal.DAL.Entities.QueryObject;
using Portal.DAL.Entities.Table;
using Portal.DAL.Statistics;
using Portal.Domain.StatisticContext;
using Portal.Mappers.Statistics;

namespace Portal.BLL.Concrete.Statistics.Reporter
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsRepository<StatProjectDeletionV2Entity> _projectDeletionRepository;
        private readonly IStatisticsRepository<StatProjectStateV3Entity> _projectStateRepostitory;
        private readonly IStatisticsRepository<StatProjectUploadingV2Entity> _projectUploadingRepository;
        private readonly IStatMapper _statMapper;
        private readonly ITableValueConverter _tableValueConverter;
        private readonly IStatisticsRepository<StatUserRegistrationV2Entity> _userRegistrationRepository;
        private readonly IStatisticsRepository<StatWatchingV2Entity> _watchingRepository;


        public StatisticsService(
            IStatisticsRepository<StatWatchingV2Entity> watchingRepository,
            IStatisticsRepository<StatUserRegistrationV2Entity> userRegistrationRepository,
            IStatisticsRepository<StatProjectUploadingV2Entity> projectUploadingRepository,
            IStatisticsRepository<StatProjectStateV3Entity> projectStateRepostitory,
            IStatisticsRepository<StatProjectDeletionV2Entity> projectDeletionRepository,
            ITableValueConverter tableValueConverter,
            IStatMapper statMapper)
        {
            _watchingRepository = watchingRepository;
            _userRegistrationRepository = userRegistrationRepository;
            _projectUploadingRepository = projectUploadingRepository;
            _projectStateRepostitory = projectStateRepostitory;
            _projectDeletionRepository = projectDeletionRepository;
            _tableValueConverter = tableValueConverter;
            _statMapper = statMapper;
        }

        public IEnumerable<DomainStatWatching> GetWatchings(Interval interval)
        {
            IEnumerable<DomainStatWatching> result = GetStatEntities(_watchingRepository, interval)
                .Select(_statMapper.StatWatchingToDomain);

            return result;
        }

        public IEnumerable<DomainStatUserRegistration> GetUserRegistrations(Interval interval)
        {
            IEnumerable<DomainStatUserRegistration> result = GetStatEntities(_userRegistrationRepository, interval)
                .Select(_statMapper.StatUserRegistrationToDomain);

            return result;
        }

        public IEnumerable<DomainStatProjectUploading> GetProjectUploadings(Interval interval)
        {
            IEnumerable<DomainStatProjectUploading> result = GetStatEntities(_projectUploadingRepository, interval)
                .Select(_statMapper.StatProjectUploadingToDomain);

            return result;
        }

        public IEnumerable<DomainStatProjectDeletion> GetProjectDeletions(Interval interval)
        {
            IEnumerable<DomainStatProjectDeletion> result = GetStatEntities(_projectDeletionRepository, interval)
                .Select(_statMapper.StatProjectDeletionToDomain);

            return result;
        }

        public DomainStatProjectState GetProjectState(string projectId)
        {
            List<StatProjectStateV3Entity> projectStates = _projectStateRepostitory.AsEnumerable(p => p.ProjectId == projectId).ToList();

            bool isCic = projectStates.All(p => p.Producer == ProductName.CicIPad ||
                                                p.Producer == ProductName.CicMac ||
                                                p.Producer == ProductName.CicPc);

            bool isSuccessfulUpload = isCic
                ? projectStates.Select(p => p.ActionType).ContainsAll(new[] { StatActionType.Project, StatActionType.Avsx, StatActionType.Video })
                : projectStates.Select(p => p.ActionType).ContainsAll(new[] { StatActionType.Project, StatActionType.Avsx, StatActionType.Video, StatActionType.Screenshot });

            StatProjectStateV3Entity entity = projectStates.FirstOrDefault(p => p.ActionType == StatActionType.Project) ?? new StatProjectStateV3Entity { ProjectId = projectId };

            DomainStatProjectState domain = _statMapper.StatProjectStateToDomain(entity, isSuccessfulUpload);

            return domain;
        }

        private IEnumerable<T> GetStatEntities<T>(IStatisticsRepository<T> statEntityRepository, Interval interval) where T : StatEntity, IEntity
        {
            string startTick = _tableValueConverter.DateTimeToComparerTick(interval.Start);
            string finishTick = _tableValueConverter.DateTimeToComparerTick(interval.Finish);
            var queryObject = new StatQueryObject
            {
                StartInterval = startTick,
                EndInterval = finishTick,
                IsStartInclude = false,
                IsEndInclude = false
            };

            IEnumerable<T> result = statEntityRepository.GetStatEntities(queryObject);

            return result;
        }
    }
}