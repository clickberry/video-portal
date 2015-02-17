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
    public class StatUserRegistrationServiceTest
    {
        [TestMethod]
        public async Task AddUserRegistrationTest()
        {
            //Arrange
            const string eventId = "eventId";
            var dateTime = new DateTime(2013, 2, 13);

            var domain = new DomainActionData();
            var httpMessageEntity = new StatHttpMessageV2Entity();
            var userRegistrationEntity = new StatUserRegistrationV2Entity();

            var guidWrapper = new Mock<IGuidWrapper>();
            var dateTimeWrapper = new Mock<IDateTimeWrapper>();
            var httpMessageRepository = new Mock<IRepository<StatHttpMessageV2Entity>>();
            var userRegistrationRepository = new Mock<IRepository<StatUserRegistrationV2Entity>>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var statEntityFactory = new Mock<IStatEntityFactory>();
            
            guidWrapper.Setup(m => m.Generate()).Returns(eventId);
            dateTimeWrapper.Setup(m => m.CurrentDateTime()).Returns(dateTime);

            repositoryFactory.Setup(m => m.Create<StatHttpMessageV2Entity>(Tables.StatHttpMessageV2)).Returns(httpMessageRepository.Object);
            repositoryFactory.Setup(m => m.Create<StatUserRegistrationV2Entity>(Tables.StatUserRegistrationV2)).Returns(userRegistrationRepository.Object);

            statEntityFactory.Setup(m => m.CreateHttpMessageEntity(eventId, dateTime, domain)).Returns(httpMessageEntity);
            statEntityFactory.Setup(m => m.CreateUserRegistrationEntity(eventId, dateTime, domain)).Returns(userRegistrationEntity);
            httpMessageRepository.Setup(m => m.AddAsync(httpMessageEntity, It.IsAny<CancellationToken>())).Returns(async () => httpMessageEntity);
            userRegistrationRepository.Setup(m => m.AddAsync(userRegistrationEntity, It.IsAny<CancellationToken>())).Returns(async () => userRegistrationEntity);

            var userRegistrationService = new StatUserRegistrationService(repositoryFactory.Object, statEntityFactory.Object, guidWrapper.Object, dateTimeWrapper.Object);

            //Act & Assert
            await userRegistrationService.AddUserRegistration(domain);
        }
    }
}