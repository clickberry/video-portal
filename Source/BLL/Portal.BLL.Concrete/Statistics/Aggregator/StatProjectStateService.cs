// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Portal.BLL.Statistics.Aggregator;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.Domain.StatisticContext;
using Portal.Exceptions.CRUD;
using Portal.Mappers.Statistics;
using Wrappers.Interface;

namespace Portal.BLL.Concrete.Statistics.Aggregator
{
    public class StatProjectStateService : IStatProjectStateService
    {
        private readonly IDateTimeWrapper _dateTimeWrapper;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IStatEntityFactory _statEntityFactory;

        public StatProjectStateService(
            IRepositoryFactory repositoryFactory,
            IStatEntityFactory statEntityFactory,
            IDateTimeWrapper dateTimeWrapper)
        {
            _repositoryFactory = repositoryFactory;
            _statEntityFactory = statEntityFactory;
            _dateTimeWrapper = dateTimeWrapper;
        }

        public async Task AddProjectState(DomainActionData domain, string projectId, string actionType)
        {
            DateTime curDateTime = _dateTimeWrapper.CurrentDateTime();

            StatProjectStateV3Entity statProjectStateEntity = _statEntityFactory.CreateProjectCreatingEntity(curDateTime, domain, projectId, actionType);
            ITableRepository<StatProjectStateV3Entity> statProjectStateRepository = _repositoryFactory.Create<StatProjectStateV3Entity>();

            try
            {
                await statProjectStateRepository.AddAsync(statProjectStateEntity);
            }
            catch (ConflictException)
            {
            }
        }
    }
}