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
    public class StatUserLoginService : AggregatorBase, IStatUserLoginService
    {
        public StatUserLoginService(
            IRepositoryFactory repositoryFactory,
            IStatEntityFactory statEntityFactory,
            IGuidWrapper guid,
            IDateTimeWrapper dateTime)
            : base(repositoryFactory, statEntityFactory, guid, dateTime)
        {
        }

        public Task AddUserLogin(DomainActionData domain)
        {
            string eventId = GuidWraper.Generate();
            DateTime curDateTime = DateTimeWrapper.CurrentDateTime();

            StatUserLoginV2Entity userLoginEntity = StatEntityFactory.CreateUserLoginEntity(domain);
            ITableRepository<StatUserLoginV2Entity> userLoginRepository = RepositoryFactory.Create<StatUserLoginV2Entity>();

            return userLoginRepository.AddAsync(userLoginEntity);
        }
    }
}