using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Aggregator;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.Domain.StatisticContext;
using Portal.Mappers.Statistics;
using Wrappers.Interface;

namespace Portal.BLL.Tests.StatisticsTests.AggregatorTest
{
    [TestClass]
    public class StatWatchingServiceTest
    {
        [TestMethod]
        public async Task AddWatchingTest()
        {
            const string eventId = "eventId";
            const string projectId = "projectId";
            var dateTime = new DateTime(2013, 2, 13);

            var domain = new DomainActionData();
            var httpMessageEntity = new StatHttpMessageV2Entity();
            var watchingEntity = new StatWatchingV2Entity();

            var guidWrapper = new Mock<IGuidWrapper>();
            var dateTimeWrapper = new Mock<IDateTimeWrapper>();
            var httpMessageRepository = new Mock<IRepository<StatHttpMessageV2Entity>>();
            var watchingRepository = new Mock<IRepository<StatWatchingV2Entity>>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var statEntityFactory = new Mock<IStatEntityFactory>();

            guidWrapper.Setup(m => m.Generate()).Returns(eventId);
            dateTimeWrapper.Setup(m => m.CurrentDateTime()).Returns(dateTime);

            repositoryFactory.Setup(m => m.Create<StatHttpMessageV2Entity>(Tables.StatHttpMessageV2)).Returns(httpMessageRepository.Object);
            repositoryFactory.Setup(m => m.Create<StatWatchingV2Entity>(Tables.StatWatchingV2)).Returns(watchingRepository.Object);

            statEntityFactory.Setup(m => m.CreateHttpMessageEntity(eventId, dateTime, domain)).Returns(httpMessageEntity);
            statEntityFactory.Setup(m => m.CreateWatchingEntity(eventId, dateTime, domain, projectId)).Returns(watchingEntity);
            httpMessageRepository.Setup(m => m.AddAsync(httpMessageEntity, It.IsAny<CancellationToken>())).Returns(async () => httpMessageEntity);
            watchingRepository.Setup(m => m.AddAsync(watchingEntity, It.IsAny<CancellationToken>())).Returns(async () => watchingEntity);

            var watchingService = new StatWatchingService(repositoryFactory.Object, statEntityFactory.Object, guidWrapper.Object, dateTimeWrapper.Object);

            //Act & Assert
            await watchingService.AddWatching(domain, projectId);
        }
    }
}