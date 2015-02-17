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
    public class StatProjectDeletionServiceTest
    {
        const string EventId = "eventId";
        const string ProjectId = "projectId";

        private DateTime _dateTime;
        private DomainActionData _domain;
        private Mock<IGuidWrapper> _guidWrapper;
        private Mock<IDateTimeWrapper> _dateTimeWrapper;
        private Mock<IRepository<StatHttpMessageV2Entity>> _httpMessageRepository;
        private Mock<IStatEntityFactory> _statEntityFactory;
        private Mock<IRepositoryFactory> _repositoryFactory;

        [TestInitialize]
        public void Initialize()
        {
            var httpMessageEntity = new StatHttpMessageV2Entity();

            _dateTime = new DateTime(2013, 2, 13);
            _domain = new DomainActionData();
            _guidWrapper = new Mock<IGuidWrapper>();
            _dateTimeWrapper = new Mock<IDateTimeWrapper>();
            _httpMessageRepository = new Mock<IRepository<StatHttpMessageV2Entity>>();
            _statEntityFactory = new Mock<IStatEntityFactory>();
            _repositoryFactory = new Mock<IRepositoryFactory>();

            _httpMessageRepository.Setup(m => m.AddAsync(httpMessageEntity, It.IsAny<CancellationToken>())).Returns(async () => httpMessageEntity);
            _repositoryFactory.Setup(m => m.Create<StatHttpMessageV2Entity>(Tables.StatHttpMessageV2)).Returns(_httpMessageRepository.Object);
            _statEntityFactory.Setup(m => m.CreateHttpMessageEntity(EventId, _dateTime, _domain)).Returns(httpMessageEntity);
            _guidWrapper.Setup(m => m.Generate()).Returns(EventId);
            _dateTimeWrapper.Setup(m => m.CurrentDateTime()).Returns(_dateTime);
        }

        [TestMethod]
        public async Task AddProjectDeletionTest()
        {
            var projectDeletionEntity = new StatProjectDeletionV2Entity();
            var projectDeletionRepository = new Mock<IRepository<StatProjectDeletionV2Entity>>();
            var projectStateRepository = new Mock<IRepository<StatProjectStateV3Entity>>();
           
            _repositoryFactory.Setup(m => m.Create<StatProjectDeletionV2Entity>(Tables.StatProjectDeletionV2)).Returns(projectDeletionRepository.Object);
            _repositoryFactory.Setup(m => m.Create<StatProjectStateV3Entity>(Tables.StatProjectStateV3)).Returns(projectStateRepository.Object);
            
            _statEntityFactory.Setup(m => m.CreateProjectDeletionEntity(EventId, _dateTime, _domain, ProjectId)).Returns(projectDeletionEntity);
            projectDeletionRepository.Setup(m => m.AddAsync(projectDeletionEntity, It.IsAny<CancellationToken>())).Returns(async () => projectDeletionEntity);

            var projectDeletionService = new StatProjectDeletionService(_repositoryFactory.Object, _statEntityFactory.Object, _guidWrapper.Object, _dateTimeWrapper.Object);

            //Act & Assert
            await projectDeletionService.AddProjectDeletion(_domain, ProjectId);
        }
    }
}