using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Aggregator;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.Domain.StatisticContext;
using Portal.Exceptions.CRUD;
using Portal.Mappers.Statistics;
using TestExtension;
using Wrappers.Interface;

namespace Portal.BLL.Tests.StatisticsTests.AggregatorTest
{
    [TestClass]
    public class StatProjectStateServiceTest
    {
        [TestMethod]
        public async Task AddProjectCreatingTest()
        {
            const string projectId = "projectId";
            const string actionType = "actionType";
            var dateTime = new DateTime(2013, 2, 13);

            var domain = new DomainActionData();
            var statProjectStateV2Entity = new StatProjectStateV3Entity();

            var dateTimeWrapper = new Mock<IDateTimeWrapper>();
            var statProjectStateRepository = new Mock<IRepository<StatProjectStateV3Entity>>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var statEntityFactory = new Mock<IStatEntityFactory>();

            dateTimeWrapper.Setup(m => m.CurrentDateTime()).Returns(dateTime);

            repositoryFactory.Setup(m => m.Create<StatProjectStateV3Entity>(Tables.StatProjectStateV3)).Returns(statProjectStateRepository.Object);

            statEntityFactory.Setup(m => m.CreateProjectCreatingEntity(dateTime, domain, projectId, actionType)).Returns(statProjectStateV2Entity);
            statProjectStateRepository.Setup(m => m.AddAsync(statProjectStateV2Entity, It.IsAny<CancellationToken>())).Returns(async () => statProjectStateV2Entity);

            var statProjectCreatingService = new StatProjectStateService(repositoryFactory.Object, statEntityFactory.Object, dateTimeWrapper.Object);

            //Act & Assert
            await statProjectCreatingService.AddProjectState(domain, projectId, actionType);
        }

        [TestMethod]
        public void AddProjectCreatingCatchConflictExceptionTest()
        {
            const string projectId = "projectId";
            const string actionType = "actionType";
            var dateTime = new DateTime(2013, 2, 13);

            var domain = new DomainActionData();
            var statProjectStateV2Entity = new StatProjectStateV3Entity();

            var dateTimeWrapper = new Mock<IDateTimeWrapper>();
            var statProjectStateRepository = new Mock<IRepository<StatProjectStateV3Entity>>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var statEntityFactory = new Mock<IStatEntityFactory>();

            dateTimeWrapper.Setup(m => m.CurrentDateTime()).Returns(dateTime);

            repositoryFactory.Setup(m => m.Create<StatProjectStateV3Entity>(Tables.StatProjectStateV3)).Returns(statProjectStateRepository.Object);

            statEntityFactory.Setup(m => m.CreateProjectCreatingEntity(dateTime, domain, projectId, actionType)).Returns(statProjectStateV2Entity);
            statProjectStateRepository.Setup(m => m.AddAsync(statProjectStateV2Entity, It.IsAny<CancellationToken>())).Throws(new ConflictException());

            var statProjectCreatingService = new StatProjectStateService(repositoryFactory.Object, statEntityFactory.Object, dateTimeWrapper.Object);

            //Act & Assert
            ExceptionAssert.NotThrows<ConflictException>(()=> statProjectCreatingService.AddProjectState(domain, projectId, actionType).Wait());
        }
    }
}