// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Portal.BLL.Statistics.Aggregator;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.Domain.ProjectContext;
using Portal.Domain.StatisticContext;
using Portal.Mappers.Statistics;
using Wrappers.Interface;

namespace Portal.BLL.Concrete.Statistics.Aggregator
{
    public class StatProjectUploadingService : AggregatorBase, IStatProjectUploadingService
    {
        public StatProjectUploadingService(
            IRepositoryFactory repositoryFactory,
            IStatEntityFactory statEntityFactory,
            IGuidWrapper guid,
            IDateTimeWrapper dateTime)
            : base(repositoryFactory, statEntityFactory, guid, dateTime)
        {
        }

        public Task AddProjectUploading(DomainActionData actionData, string projectId, string projectName, ProjectType projectType, ProjectSubtype projectSubtype)
        {
            string eventId = GuidWraper.Generate();
            DateTime curDateTime = DateTimeWrapper.CurrentDateTime();

            StatProjectUploadingV2Entity projectUploadingEntity = StatEntityFactory.CreateProjectUploadingEntity(eventId, curDateTime, actionData, projectId, projectName, projectType, projectSubtype);
            ITableRepository<StatProjectUploadingV2Entity> projectUploadingRepository = RepositoryFactory.Create<StatProjectUploadingV2Entity>();

            return projectUploadingRepository.AddAsync(projectUploadingEntity);
        }
    }
}