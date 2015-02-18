// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

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
    public class StatUserRegistrationService : AggregatorBase, IStatUserRegistrationService
    {
        public StatUserRegistrationService(
            IRepositoryFactory repositoryFactory,
            IStatEntityFactory statEntityFactory,
            IGuidWrapper guid,
            IDateTimeWrapper dateTime)
            : base(repositoryFactory, statEntityFactory, guid, dateTime)
        {
        }

        public Task AddUserRegistration(DomainActionData domain)
        {
            string eventId = GuidWraper.Generate();
            DateTime curDateTime = DateTimeWrapper.CurrentDateTime();

            StatUserRegistrationV2Entity userRegistrationEntity = StatEntityFactory.CreateUserRegistrationEntity(eventId, curDateTime, domain);
            ITableRepository<StatUserRegistrationV2Entity> userRegistrationRepository = RepositoryFactory.Create<StatUserRegistrationV2Entity>();

            return userRegistrationRepository.AddAsync(userRegistrationEntity);
        }
    }
}