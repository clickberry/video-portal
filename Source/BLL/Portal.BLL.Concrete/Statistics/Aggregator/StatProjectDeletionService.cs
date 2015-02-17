// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Portal.BLL.Statistics.Aggregator;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.Domain.StatisticContext;
using Portal.Mappers.Statistics;
using Wrappers.Interface;

namespace Portal.BLL.Concrete.Statistics.Aggregator
{
    public class StatProjectDeletionService : AggregatorBase, IStatProjectDeletionService
    {
        public StatProjectDeletionService(
            IRepositoryFactory repositoryFactory,
            IStatEntityFactory statEntityFactory,
            IGuidWrapper guid,
            IDateTimeWrapper dateTime)
            : base(repositoryFactory, statEntityFactory, guid, dateTime)
        {
        }

        public Task AddProjectDeletion(DomainActionData domain, string projectId)
        {
            string eventId = GuidWraper.Generate();
            DateTime curDateTime = DateTimeWrapper.CurrentDateTime();

            StatProjectDeletionV2Entity statProjectDeletionEntity = StatEntityFactory.CreateProjectDeletionEntity(eventId, curDateTime, domain, projectId);
            ITableRepository<StatProjectDeletionV2Entity> projectDeletionRepository = RepositoryFactory.Create<StatProjectDeletionV2Entity>();

            return projectDeletionRepository.AddAsync(statProjectDeletionEntity);
        }
    }
}