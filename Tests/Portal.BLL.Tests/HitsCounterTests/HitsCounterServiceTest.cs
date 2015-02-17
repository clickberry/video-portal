using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.HitsCounter;
using Portal.DAL.Context;
using Portal.DAL.Entities.QueryObject;
using Portal.DAL.Entities.Table;
using Portal.Mappers.Statistics;
using TestFake;
using Wrappers.Interface;

namespace Portal.BLL.Tests.HitsCounterTests
{
    [TestClass]
    public class HitsCounterServiceTest
    {
        private readonly Mock<IRepositoryFactory> _repositoryFactory = new Mock<IRepositoryFactory>();
        private readonly Mock<IRepository<HitsCountEntity>> _hitsCountsRepository = new Mock<IRepository<HitsCountEntity>>();
        private readonly Mock<IRepository<HitsCountUpdateEntity>> _hitsCountUpdateRepository = new Mock<IRepository<HitsCountUpdateEntity>>();
        private readonly Mock<ITableValueConverter> _valueConverter = new Mock<ITableValueConverter>();

        [TestInitialize]
        public void Initialize()
        {
            _repositoryFactory.Setup(m => m.Create<HitsCountEntity>(Tables.HitsCountV2)).Returns(_hitsCountsRepository.Object);
            _repositoryFactory.Setup(m => m.Create<HitsCountUpdateEntity>(Tables.HitsCountUpdateV2)).Returns(_hitsCountUpdateRepository.Object);
        }

        [TestMethod]
        public async Task GetHitsCountForEmptyTablesTest()
        {
            //Arrange
            const string projectId = "projectId";

            var hitsCountsEntities = new List<HitsCountEntity>();
            
            _hitsCountsRepository
                .Setup(m => m.GetHitsCounts(It.Is<HitsCountQueryObject>(p => p.ProjectId == projectId)))
                .Returns(hitsCountsEntities);

            var hitsCounterService = new HitsCounterService(_repositoryFactory.Object, _valueConverter.Object);

            //Act
            var result = await hitsCounterService.GetHitsCount(projectId);

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task GetHitsCountForEmptyHitsCountUpdateTest()
        {
            //Arrange
            const string projectId1 = "projectId1";
            const string lastTick1 = "11tick";
            const string lastTick2 = "12tick";
            const string updateTick = "updateTick";
            
            var hitsCountEntities = new List<HitsCountEntity>()
                {
                    new HitsCountEntity() {ProjectId = projectId1, Tick = lastTick1},
                    new HitsCountEntity() {ProjectId = projectId1, Tick = lastTick2}
                };
            
            _valueConverter.Setup(m => m.ChangeGuidPart(lastTick1)).Returns(updateTick);
            _hitsCountsRepository
                .Setup(m => m.GetHitsCounts(It.Is<HitsCountQueryObject>(p => p.ProjectId == projectId1)))
                .Returns(hitsCountEntities);
            _hitsCountUpdateRepository
                .Setup(m => m.GetLastHitsCountUpdate(It.Is<HitsCountQueryObject>(p => p.ProjectId == projectId1)))
                .Returns((HitsCountUpdateEntity) null);
            _hitsCountUpdateRepository
                .Setup(m => m.AddAsync(It.IsAny<HitsCountUpdateEntity>(), It.IsAny<CancellationToken>()))
                .Returns(async () => new HitsCountUpdateEntity());

            var hitsCounterService = new HitsCounterService(_repositoryFactory.Object, _valueConverter.Object);

            //Act
            var result = await hitsCounterService.GetHitsCount(projectId1);

            //Assert
            Assert.AreEqual(2, result);
            _hitsCountUpdateRepository
                .Verify(m => m.AddAsync(It.Is<HitsCountUpdateEntity>(p => p.ProjectId == projectId1 &&
                                                                          p.Tick == updateTick &&
                                                                          p.Count == 2), It.IsAny<CancellationToken>()),
                    Times.Once());
        }

        [TestMethod]
        public async Task GetHitsCountForEmptyHitsCountTest()
        {
            //Arrange
            const string projectId1 = "projectId1";
            const string lastTick3 = "13tick";
            const long hitsCount = 2342;

            var hitsCountEntities = new List<HitsCountEntity>();
            var hitsCountUpdateEntity = new HitsCountUpdateEntity()
            {
                ProjectId = projectId1,
                Tick = lastTick3,
                Count = hitsCount
            };

            _hitsCountsRepository
                .Setup(m => m.GetHitsCounts(It.Is<HitsCountQueryObject>(p => p.ProjectId == projectId1)))
                .Returns(hitsCountEntities);
            _hitsCountUpdateRepository
                .Setup(m => m.GetLastHitsCountUpdate(It.Is<HitsCountQueryObject>(p => p.ProjectId == projectId1)))
                .Returns(hitsCountUpdateEntity);
            _hitsCountUpdateRepository
                .Setup(m => m.AddAsync(It.IsAny<HitsCountUpdateEntity>(), It.IsAny<CancellationToken>()))
                .Returns(async () => new HitsCountUpdateEntity());
            
            var hitsCounterService = new HitsCounterService(_repositoryFactory.Object, _valueConverter.Object);

            //Act
            var result = await hitsCounterService.GetHitsCount(projectId1);

            //Assert
            Assert.AreEqual(hitsCount, result);
            _hitsCountUpdateRepository
                .Verify(m => m.AddAsync(It.IsAny<HitsCountUpdateEntity>(), It.IsAny<CancellationToken>()), Times.Never());
        }


        [TestMethod]
        public async Task GetHitsCountTest()
        {
            //Arrange
            const string projectId1 = "projectId1";
            const string lastTick1 = "11tick";
            const string lastTick2 = "12tick";
            const string updateTick = "updateTick";
            const string tickPart = "tickPart";
            const long hitsCount = 2342;

            var hitsCountEntities = new List<HitsCountEntity>()
                {
                    new HitsCountEntity() {ProjectId = projectId1, Tick = lastTick1},
                    new HitsCountEntity() {ProjectId = projectId1, Tick = lastTick2}
                };
            var hitsCountUpdateEntity = new HitsCountUpdateEntity()
            {
                ProjectId = projectId1,
                Tick = lastTick1,
                Count = hitsCount
            };

            _valueConverter.Setup(m => m.ChangeGuidPart(lastTick1)).Returns(updateTick);
            _valueConverter.Setup(m => m.GetTickPart(lastTick1)).Returns(tickPart);
            _hitsCountsRepository
               .Setup(m => m.GetHitsCounts(It.Is<HitsCountQueryObject>(p => p.ProjectId == projectId1 &&
                                                                                      p.Tick == tickPart)))
               .Returns(hitsCountEntities);
            _hitsCountUpdateRepository
                .Setup(m => m.GetLastHitsCountUpdate(It.Is<HitsCountQueryObject>(p => p.ProjectId == projectId1)))
                .Returns(hitsCountUpdateEntity);
            _hitsCountUpdateRepository
                .Setup(m => m.AddAsync(It.IsAny<HitsCountUpdateEntity>(), It.IsAny<CancellationToken>()))
                .Returns(async () => new HitsCountUpdateEntity());

            var hitsCounterService = new HitsCounterService(_repositoryFactory.Object, _valueConverter.Object);

            //Act
            var result = await hitsCounterService.GetHitsCount(projectId1);

            //Assert
            Assert.AreEqual(hitsCount + 2, result);
            _hitsCountUpdateRepository
               .Verify(m => m.AddAsync(It.Is<HitsCountUpdateEntity>(p => p.ProjectId == projectId1 &&
                                                                         p.Tick == updateTick &&
                                                                         p.Count == hitsCount + 2), It.IsAny<CancellationToken>()),
                   Times.Once());
        }

        [TestMethod]
        public async Task AddHitTest()
        {
            //Arrange
            const string projectId = "projectId";
            const string tick = "tick";
            var curDateTime = new DateTime(1234);
            var hitsCountEntities = new List<HitsCountEntity>();

            var hitsCountRepository = new FakeRepository<HitsCountEntity>(hitsCountEntities);

            _valueConverter.Setup(m => m.DateTimeToTickWithGuid(curDateTime)).Returns(tick);
            _repositoryFactory.Setup(m => m.Create<HitsCountEntity>(Tables.HitsCountV2)).Returns(hitsCountRepository);

            var hitsCounterService = new HitsCounterService(_repositoryFactory.Object, _valueConverter.Object);

            //Act
            await hitsCounterService.AddProjectHit(projectId, curDateTime);

            //Assert
            Assert.AreEqual(1, hitsCountEntities.Count);
            Assert.AreEqual(tick,hitsCountEntities.FirstOrDefault().Tick);
            Assert.AreEqual(projectId, hitsCountEntities.FirstOrDefault().ProjectId);
        }
    }
}
