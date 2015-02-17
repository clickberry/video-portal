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
    public class StatWatchingService : AggregatorBase, IStatWatchingService
    {
        public StatWatchingService(
            IRepositoryFactory repositoryFactory,
            IStatEntityFactory statEntityFactory,
            IGuidWrapper guid,
            IDateTimeWrapper dateTime)
            : base(repositoryFactory, statEntityFactory, guid, dateTime)
        {
        }

        public Task AddWatching(DomainActionData domain, string projectId)
        {
            string eventId = GuidWraper.Generate();
            DateTime curDateTime = DateTimeWrapper.CurrentDateTime();

            StatWatchingV2Entity watchingEntity = StatEntityFactory.CreateWatchingEntity(eventId, curDateTime, domain, projectId);
            ITableRepository<StatWatchingV2Entity> watchingRepository = RepositoryFactory.Create<StatWatchingV2Entity>();

            return watchingRepository.AddAsync(watchingEntity);
        }
    }
}