using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Reporter;
using Portal.BLL.Statistics.Helper;
using Portal.DAL.Context;
using Portal.DAL.Entities.QueryObject;
using Portal.DAL.Entities.Table;
using Portal.Domain.StatisticContext;
using Portal.Mappers.Statistics;
using TestFake;

namespace Portal.BLL.Tests.StatisticsTests.ReporterTests
{
    [TestClass]
    public class StatisticsServiceTest
    {
        const string Tick1 = "11_tick";
        const string Tick2 = "12_tick";

        private DateTime _startDate;
        private DateTime _finishDate;
        private Interval _interval;

        [TestInitialize]
        public void Initialize()
        {
             _startDate = new DateTime(1234234);
             _finishDate = new DateTime(456324543);
             _interval = new Interval() { Start = _startDate, Finish = _finishDate };
        }

        [TestMethod]
        public void GetWatchingsTest()
        {
            //Arrange
            var domain1 = new DomainStatWatching();
            var domain2 = new DomainStatWatching();

            var entity1 = new StatWatchingV2Entity() { Tick = Tick1 };
            var entity2 = new StatWatchingV2Entity() { Tick = Tick2 };
            var entities = new List<StatWatchingV2Entity>() { entity1, entity2 };

            var repository = new Mock<IRepository<StatWatchingV2Entity>>();

            var repositoryFactory = new Mock<IRepositoryFactory>();
            var tableValueConverter = new Mock<ITableValueConverter>();
            var statMapper = new Mock<IStatMapper>();

            repositoryFactory.Setup(m => m.Create<StatWatchingV2Entity>(Tables.StatWatchingV2)).Returns(repository.Object);
            repository.Setup(m => m.GetStatEntities(It.Is<StatQueryObject>(p => p.StartInterval == Tick2 &&
                                                                                p.EndInterval == Tick1 &&
                                                                                p.IsStartInclude == false &&
                                                                                p.IsEndInclude == false)))
                .Returns(entities);

            tableValueConverter.Setup(m => m.DateTimeToComparerTick(_startDate)).Returns(Tick2);
            tableValueConverter.Setup(m => m.DateTimeToComparerTick(_finishDate)).Returns(Tick1);
            statMapper.Setup(m => m.StatWatchingToDomain(entity1)).Returns(domain1);
            statMapper.Setup(m => m.StatWatchingToDomain(entity2)).Returns(domain2);

            var statisticsService = new StatisticsService(repositoryFactory.Object, tableValueConverter.Object, statMapper.Object);

            //Act
            var result = statisticsService.GetWatchings(_interval).ToList();

            //Assert
            Assert.AreEqual(2, result.Count);
            CollectionAssert.Contains(result, domain1);
            CollectionAssert.Contains(result, domain2);
        }

        [TestMethod]
        public void GetUserRegistrationsTest()
        {
            //Arrange
            var domain1 = new DomainStatUserRegistration();
            var domain2 = new DomainStatUserRegistration();

            var entity1 = new StatUserRegistrationV2Entity() { Tick = Tick1 };
            var entity2 = new StatUserRegistrationV2Entity() { Tick = Tick2 };
            var entities = new List<StatUserRegistrationV2Entity>() { entity1, entity2 };

            var repository = new Mock<IRepository<StatUserRegistrationV2Entity>>();

            var repositoryFactory = new Mock<IRepositoryFactory>();
            var tableValueConverter = new Mock<ITableValueConverter>();
            var statMapper = new Mock<IStatMapper>();

            repositoryFactory.Setup(m => m.Create<StatUserRegistrationV2Entity>(Tables.StatUserRegistrationV2)).Returns(repository.Object);
            repository.Setup(m => m.GetStatEntities(It.Is<StatQueryObject>(p => p.StartInterval == Tick2 &&
                                                                                p.EndInterval == Tick1 &&
                                                                                p.IsStartInclude == false &&
                                                                                p.IsEndInclude == false)))
                .Returns(entities);

            tableValueConverter.Setup(m => m.DateTimeToComparerTick(_startDate)).Returns(Tick2);
            tableValueConverter.Setup(m => m.DateTimeToComparerTick(_finishDate)).Returns(Tick1);
            statMapper.Setup(m => m.StatUserRegistrationToDomain(entity1)).Returns(domain1);
            statMapper.Setup(m => m.StatUserRegistrationToDomain(entity2)).Returns(domain2);

            var statisticsService = new StatisticsService(repositoryFactory.Object, tableValueConverter.Object, statMapper.Object);

            //Act
            var result = statisticsService.GetUserRegistrations(_interval).ToList();

            //Assert
            Assert.AreEqual(2, result.Count);
            CollectionAssert.Contains(result, domain1);
            CollectionAssert.Contains(result, domain2);
        }

        [TestMethod]
        public void GetProjectUploadingsTest()
        {
            //Arrange
            var domain1 = new DomainStatProjectUploading();
            var domain2 = new DomainStatProjectUploading();

            var entity1 = new StatProjectUploadingV2Entity() { Tick = Tick1 };
            var entity2 = new StatProjectUploadingV2Entity() { Tick = Tick2 };
            var entities = new List<StatProjectUploadingV2Entity>() { entity1, entity2 };

            var repository = new Mock<IRepository<StatProjectUploadingV2Entity>>();

            var repositoryFactory = new Mock<IRepositoryFactory>();
            var tableValueConverter = new Mock<ITableValueConverter>();
            var statMapper = new Mock<IStatMapper>();

            repositoryFactory.Setup(m => m.Create<StatProjectUploadingV2Entity>(Tables.StatProjectUploadingV2)).Returns(repository.Object);
            repository.Setup(m => m.GetStatEntities(It.Is<StatQueryObject>(p => p.StartInterval == Tick2 &&
                                                                                p.EndInterval == Tick1 &&
                                                                                p.IsStartInclude == false &&
                                                                                p.IsEndInclude == false)))
                .Returns(entities);

            tableValueConverter.Setup(m => m.DateTimeToComparerTick(_startDate)).Returns(Tick2);
            tableValueConverter.Setup(m => m.DateTimeToComparerTick(_finishDate)).Returns(Tick1);
            statMapper.Setup(m => m.StatProjectUploadingToDomain(entity1)).Returns(domain1);
            statMapper.Setup(m => m.StatProjectUploadingToDomain(entity2)).Returns(domain2);

            var statisticsService = new StatisticsService(repositoryFactory.Object, tableValueConverter.Object, statMapper.Object);

            //Act
            var result = statisticsService.GetProjectUploadings(_interval).ToList();

            //Assert
            Assert.AreEqual(2, result.Count);
            CollectionAssert.Contains(result, domain1);
            CollectionAssert.Contains(result, domain2);
        }

        [TestMethod]
        public void GetProjectDeletionsTest()
        {
            //Arrange
            var domain1 = new DomainStatProjectDeletion();
            var domain2 = new DomainStatProjectDeletion();

            var entity1 = new StatProjectDeletionV2Entity() { Tick = Tick1 };
            var entity2 = new StatProjectDeletionV2Entity() { Tick = Tick2 };
            var entities = new List<StatProjectDeletionV2Entity>() { entity1, entity2 };

            var repository = new Mock<IRepository<StatProjectDeletionV2Entity>>();

            var repositoryFactory = new Mock<IRepositoryFactory>();
            var tableValueConverter = new Mock<ITableValueConverter>();
            var statMapper = new Mock<IStatMapper>();

            repositoryFactory.Setup(m => m.Create<StatProjectDeletionV2Entity>(Tables.StatProjectDeletionV2)).Returns(repository.Object);
            repository.Setup(m => m.GetStatEntities(It.Is<StatQueryObject>(p => p.StartInterval == Tick2 &&
                                                                                p.EndInterval == Tick1 &&
                                                                                p.IsStartInclude == false &&
                                                                                p.IsEndInclude == false)))
                .Returns(entities);

            tableValueConverter.Setup(m => m.DateTimeToComparerTick(_startDate)).Returns(Tick2);
            tableValueConverter.Setup(m => m.DateTimeToComparerTick(_finishDate)).Returns(Tick1);
            statMapper.Setup(m => m.StatProjectDeletionToDomain(entity1)).Returns(domain1);
            statMapper.Setup(m => m.StatProjectDeletionToDomain(entity2)).Returns(domain2);

            var statisticsService = new StatisticsService(repositoryFactory.Object, tableValueConverter.Object, statMapper.Object);

            //Act
            var result = statisticsService.GetProjectDeletions(_interval).ToList();

            //Assert
            Assert.AreEqual(2, result.Count);
            CollectionAssert.Contains(result, domain1);
            CollectionAssert.Contains(result, domain2);
        }

        [TestMethod]
        public void GetProjectStateTest()
        {
            //Arrange
            const string projectId1 = "projectId1";
            const string projectId2 = "projectId2";

            var entity11 = new StatProjectStateV3Entity() { ProjectId = projectId1, ActionType = StatActionType.Project };
            var entity12 = new StatProjectStateV3Entity() { ProjectId = projectId1, ActionType = StatActionType.Avsx };
            var entity13 = new StatProjectStateV3Entity() { ProjectId = projectId1, ActionType = StatActionType.Screenshot };
            var entity14 = new StatProjectStateV3Entity() { ProjectId = projectId1, ActionType = StatActionType.Video };
            var entity21 = new StatProjectStateV3Entity() { ProjectId = projectId2, ActionType = StatActionType.Project };
            var entity22 = new StatProjectStateV3Entity() { ProjectId = projectId2, ActionType = StatActionType.Avsx };
            var entity23 = new StatProjectStateV3Entity() { ProjectId = projectId2, ActionType = StatActionType.Screenshot };

            var entities = new List<StatProjectStateV3Entity>() {entity11, entity12, entity13, entity14, entity21, entity22, entity23};

            var domain1 = new DomainStatProjectState();
            var domain2 = new DomainStatProjectState();

            var repositoryFactory = new Mock<IRepositoryFactory>();
            var tableValueConverter = new Mock<ITableValueConverter>();
            var statMapper = new Mock<IStatMapper>();

            var repository = new FakeRepository<StatProjectStateV3Entity>(entities);

            repositoryFactory.Setup(m => m.Create<StatProjectStateV3Entity>(Tables.StatProjectStateV3)).Returns(repository);
            statMapper.Setup(m => m.StatProjectStateToDomain(It.Is<StatProjectStateV3Entity>(p => p.ProjectId == projectId1 && p.ActionType == StatActionType.Project), true)).Returns(domain1);
            statMapper.Setup(m => m.StatProjectStateToDomain(It.Is<StatProjectStateV3Entity>(p => p.ProjectId == projectId2 && p.ActionType == StatActionType.Project), false)).Returns(domain2);
            
            var statisticsService = new StatisticsService(repositoryFactory.Object, tableValueConverter.Object, statMapper.Object);
            
            //Act
            var projectState1 = statisticsService.GetProjectState(projectId1);
            var projectState2 = statisticsService.GetProjectState(projectId2);

            //Assert
            Assert.AreEqual(domain1, projectState1);
            Assert.AreEqual(domain2, projectState2);
        }

        [TestMethod]
        public void GetProjectStateForAbsentProjectIdTest()
        {
            //Arrange
            const string projectId1 = "projectId1";

            var entity1 = new StatProjectStateV3Entity() { ProjectId = projectId1, ActionType = StatActionType.Avsx };
            var entity2 = new StatProjectStateV3Entity() { ProjectId = projectId1, ActionType = StatActionType.Screenshot };
            var entity3 = new StatProjectStateV3Entity() { ProjectId = projectId1, ActionType = StatActionType.Video };

            var entities = new List<StatProjectStateV3Entity>() { entity1, entity2, entity3 };

            var domain1 = new DomainStatProjectState();

            var repositoryFactory = new Mock<IRepositoryFactory>();
            var tableValueConverter = new Mock<ITableValueConverter>();
            var statMapper = new Mock<IStatMapper>();

            var repository = new FakeRepository<StatProjectStateV3Entity>(entities);

            repositoryFactory.Setup(m => m.Create<StatProjectStateV3Entity>(Tables.StatProjectStateV3)).Returns(repository);
            statMapper.Setup(m => m.StatProjectStateToDomain(It.Is<StatProjectStateV3Entity>(p => p.ProjectId == projectId1 &&
                                                                                                  p.ActionType == null &&
                                                                                                  p.Producer == null), false)).Returns(domain1);

            var statisticsService = new StatisticsService(repositoryFactory.Object, tableValueConverter.Object, statMapper.Object);

            //Act
            var projectState1 = statisticsService.GetProjectState(projectId1);

            //Assert
            Assert.AreEqual(domain1, projectState1);
        }
    }
}
