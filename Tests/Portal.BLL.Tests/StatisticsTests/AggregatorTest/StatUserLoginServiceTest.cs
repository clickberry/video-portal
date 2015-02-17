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
    public class StatUserLoginServiceTest
    {
        [TestMethod]
        public async Task AddUserLoginTest()
        {
            //Arrange
            const string eventId = "eventId";
            var dateTime = new DateTime(2013, 2, 13);

            var domain = new DomainActionData();
            var httpMessageEntity = new StatHttpMessageV2Entity();
            var userLoginEntity = new StatUserLoginV2Entity();

            var guidWrapper = new Mock<IGuidWrapper>();
            var dateTimeWrapper = new Mock<IDateTimeWrapper>();
            var httpMessageRepository = new Mock<IRepository<StatHttpMessageV2Entity>>();
            var userLoginRepository = new Mock<IRepository<StatUserLoginV2Entity>>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var statEntityFactory = new Mock<IStatEntityFactory>();

            guidWrapper.Setup(m => m.Generate()).Returns(eventId);
            dateTimeWrapper.Setup(m => m.CurrentDateTime()).Returns(dateTime);

            repositoryFactory.Setup(m => m.Create<StatHttpMessageV2Entity>(Tables.StatHttpMessageV2)).Returns(httpMessageRepository.Object);
            repositoryFactory.Setup(m => m.Create<StatUserLoginV2Entity>(Tables.StatUserLoginV2)).Returns(userLoginRepository.Object);

            statEntityFactory.Setup(m => m.CreateHttpMessageEntity(eventId, dateTime, domain)).Returns(httpMessageEntity);
            statEntityFactory.Setup(m => m.CreateUserLoginEntity(eventId, dateTime, domain)).Returns(userLoginEntity);
            httpMessageRepository.Setup(m => m.AddAsync(httpMessageEntity, It.IsAny<CancellationToken>())).Returns(async () => httpMessageEntity);
            userLoginRepository.Setup(m => m.AddAsync(userLoginEntity, It.IsAny<CancellationToken>())).Returns(async () => userLoginEntity);

            var userRegistrationService = new StatUserLoginService(repositoryFactory.Object, statEntityFactory.Object, guidWrapper.Object, dateTimeWrapper.Object);

            //Act & Assert
            await userRegistrationService.AddUserLogin(domain);
        }
    }
}