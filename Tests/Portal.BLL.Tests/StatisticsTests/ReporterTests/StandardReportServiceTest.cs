using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Reporter;
using Portal.BLL.Statistics.Helper;
using Portal.DAL.Context;
using Portal.DAL.Entities.QueryObject;
using Portal.DAL.Entities.Table;
using Portal.Domain.StatisticContext;
using Portal.Exceptions.CRUD;
using Portal.Mappers.Statistics;
using TestExtension;
using TestFake;

namespace Portal.BLL.Tests.StatisticsTests.ReporterTests
{
    [TestClass]
    public class StandardReportServiceTest
    {
        const string Tick1 = "11";
        const string Tick2 = "12";

        [TestMethod]
        public async Task GetReportsTest()
        {
            //Arrange
            const string tick = "tick";
            var dateTime = new DateTime(2013, 03, 25, 2, 35, 10);
            
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var valueConverter = new Mock<ITableValueConverter>();
            var mapper = new Mock<IReportMapper>();

            var reportEntity = new StandardReportV3Entity() { Tick = tick };
            var domainReport = new DomainReport();
            var reportEntities = new List<StandardReportV3Entity>() { new StandardReportV3Entity(), reportEntity };
            var domainReports = new List<DomainReport>() { domainReport };

            var fakeRepository = new FakeRepository<StandardReportV3Entity>(reportEntities);

            valueConverter.Setup(m => m.DateTimeToTick(dateTime)).Returns(tick);
            repositoryFactory.Setup(m => m.Create<StandardReportV3Entity>(Tables.StandardReportV3)).Returns(fakeRepository);
            mapper.Setup(m => m.ReportEntityToDomain(reportEntity)).Returns(domainReport);

            var reportService = new StandardReportService(repositoryFactory.Object, valueConverter.Object, mapper.Object);

            //Act
            var reports = await reportService.GetReports(dateTime);

            //Assert
            Assert.AreEqual(1, reports.Count);
            Assert.AreEqual(domainReports[0], reports[0]);
        }

        [TestMethod]
        public async Task GetReportsForNotExistDateTest()
        {
            //Arrange
            var dateTime = new DateTime(2013, 03, 25, 2, 35, 10);

            var repositoryFactory = new Mock<IRepositoryFactory>();
            var valueConverter = new Mock<ITableValueConverter>();
            var mapper = new Mock<IReportMapper>();

            var reportEntities = new List<StandardReportV3Entity>();
            var fakeRepository = new FakeRepository<StandardReportV3Entity>(reportEntities);

            repositoryFactory.Setup(m => m.Create<StandardReportV3Entity>(Tables.StandardReportV3)).Returns(fakeRepository);

            var reportService = new StandardReportService(repositoryFactory.Object, valueConverter.Object, mapper.Object);

            //Act & Assert
            await ExceptionAssert.ThrowsAsync<NotFoundException>(()=>reportService.GetReports(dateTime));
        }

        [TestMethod]
        public async Task WriteReportsTest()
        {
            //Arrange
            const string tick = "tick";
            var dateTime = new DateTime(2013, 03, 25, 2, 35, 10);

            var repositoryFactory = new Mock<IRepositoryFactory>();
            var valueConverter = new Mock<ITableValueConverter>();
            var mapper = new Mock<IReportMapper>();

            var reportEntity1 = new StandardReportV3Entity();
            var reportEntity2 = new StandardReportV3Entity();
            var domainReport1 = new DomainReport();
            var domainReport2 = new DomainReport();
            var reportEntities = new List<StandardReportV3Entity>();
            var domainReports = new List<DomainReport>() {domainReport1, domainReport2};

            var fakeRepository = new FakeRepository<StandardReportV3Entity>(reportEntities);

            valueConverter.Setup(m => m.DateTimeToTick(dateTime)).Returns(tick);
            repositoryFactory.Setup(m => m.Create<StandardReportV3Entity>(Tables.StandardReportV3)).Returns(fakeRepository);
            mapper.Setup(m => m.DomainReportToEntity(domainReport1, tick)).Returns(reportEntity1);
            mapper.Setup(m => m.DomainReportToEntity(domainReport2, tick)).Returns(reportEntity2);

            var reportService = new StandardReportService(repositoryFactory.Object, valueConverter.Object, mapper.Object);

            //Act
            await reportService.WriteReports(domainReports, dateTime);

            //Assert
            Assert.AreEqual(2, reportEntities.Count);
            CollectionAssert.Contains(reportEntities, reportEntity1);
            CollectionAssert.Contains(reportEntities, reportEntity2);
        }


        [TestMethod]
        public void GetDayReportsTest()
        {
            //Arrange
            var startDate = new DateTime(123214);
            var finishDate = new DateTime(234567);
            var interval = new Interval() {Start = startDate, Finish = finishDate};

            var repositoryFactory = new Mock<IRepositoryFactory>();
            var tableValueConverter = new Mock<ITableValueConverter>();
            var mapper = new Mock<IReportMapper>();

            var reportEntity1_1 = new StandardReportV3Entity() { Tick = Tick1, Interval = "1" };
            var reportEntity2_1 = new StandardReportV3Entity() { Tick = Tick2, Interval = "1" };

            var domainReport1 = new DomainReport();
            var domainReport2 = new DomainReport();

            var reportEntities = new List<StandardReportV3Entity>()
                {
                    reportEntity1_1,
                    reportEntity2_1,
                };

            var repository = new Mock<IRepository<StandardReportV3Entity>>();

            repository.Setup(m => m.GetReportEntities(It.Is<ReportQueryObject>(p => p.StartInterval == Tick2 &&
                                                                                    p.EndInterval == Tick1 &&
                                                                                    p.IsStartInclude == true &&
                                                                                    p.IsEndInclude == false &&
                                                                                    p.Interval == "1")))
                                                                                    .Returns(reportEntities);
            tableValueConverter.Setup(m => m.DateTimeToTick(startDate)).Returns(Tick2);
            tableValueConverter.Setup(m => m.DateTimeToTick(finishDate)).Returns(Tick1);
            repositoryFactory.Setup(m => m.Create<StandardReportV3Entity>(Tables.StandardReportV3)).Returns(repository.Object);
            mapper.Setup(m => m.ReportEntityToDomain(reportEntity1_1)).Returns(domainReport1);
            mapper.Setup(m => m.ReportEntityToDomain(reportEntity2_1)).Returns(domainReport2);

            var reportService = new StandardReportService(repositoryFactory.Object, tableValueConverter.Object, mapper.Object);

            //Act
            var reports = reportService.GetDayReports(interval).ToList();

            //Assert
            Assert.AreEqual(2, reports.Count);
            CollectionAssert.Contains(reports, domainReport1);
            CollectionAssert.Contains(reports, domainReport2);
        }

        [TestMethod]
        public async Task GetLastAllDaysReportTest()
        {
            //Arrange
            var domainReport = new DomainReport();
            var reportEntityAll = new StandardReportV3Entity() { Interval = "All" };
            var repository = new Mock<IRepository<StandardReportV3Entity>>();

            var repositoryFactory = new Mock<IRepositoryFactory>();
            var valueConverter = new Mock<ITableValueConverter>();
            var mapper = new Mock<IReportMapper>();
            
            repositoryFactory.Setup(m => m.Create<StandardReportV3Entity>(Tables.StandardReportV3)).Returns(repository.Object);
            repository.Setup(m => m.GetLastReport(It.Is<ReportQueryObject>(p => p.Interval == "All"))).Returns(async () => reportEntityAll);
            mapper.Setup(m => m.ReportEntityToDomain(reportEntityAll)).Returns(domainReport);

            var standardReportService = new StandardReportService(repositoryFactory.Object,valueConverter.Object,mapper.Object);

            //Act
            var report = await standardReportService.GetLastAllDaysReport();

            //Assert
            Assert.AreEqual(domainReport, report);
        }

        [TestMethod]
        public async Task GetLastAllDaysReportIfNotExistTest()
        {
            //Arrange
            var repository = new Mock<IRepository<StandardReportV3Entity>>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var valueConverter = new Mock<ITableValueConverter>();
            var mapper = new Mock<IReportMapper>();

            repositoryFactory.Setup(m => m.Create<StandardReportV3Entity>(Tables.StandardReportV3)).Returns(repository.Object);
            repository.Setup(m => m.GetLastReport(It.Is<ReportQueryObject>(p=>p.Interval=="All"))).Returns(async () => null);

            var standardReportService = new StandardReportService(repositoryFactory.Object, valueConverter.Object, mapper.Object);

            //Act
            var report = await standardReportService.GetLastAllDaysReport();

            //Assert
            Assert.IsNotNull(report);
            mapper.Verify(m => m.ReportEntityToDomain(It.IsAny<StandardReportV3Entity>()), Times.Never());
        }

        [TestMethod]
        public async Task   DeleteReportsTest()
        {
            //Arrange
            const string tick = "tick";
            var dateTime = new DateTime(2013, 03, 25, 2, 35, 10);

            var repositoryFactory = new Mock<IRepositoryFactory>();
            var valueConverter = new Mock<ITableValueConverter>();
            var mapper = new Mock<IReportMapper>();

            var reportEntity1 = new StandardReportV3Entity();
            var reportEntity2 = new StandardReportV3Entity();
            var domainReport1 = new DomainReport();
            var domainReport2 = new DomainReport();
            var domainReports = new List<DomainReport>() { domainReport1, domainReport2 };

            var repository = new Mock<IRepository<StandardReportV3Entity>>();
            
            valueConverter.Setup(m => m.DateTimeToTick(dateTime)).Returns(tick);
            repositoryFactory.Setup(m => m.Create<StandardReportV3Entity>(Tables.StandardReportV3)).Returns(repository.Object);
            repository.Setup(m => m.DeleteAsync(It.IsAny<IEnumerable<StandardReportV3Entity>>(), It.IsAny<CancellationToken>())).Returns(async () => { });
            mapper.Setup(m => m.DomainReportToEntity(domainReport1, tick)).Returns(reportEntity1);
            mapper.Setup(m => m.DomainReportToEntity(domainReport2, tick)).Returns(reportEntity2);

            var reportService = new StandardReportService(repositoryFactory.Object, valueConverter.Object, mapper.Object);

            //Act
            await reportService.DeleteReports(domainReports, dateTime);

            //Assert
            repository.Verify(m => m.DeleteAsync(It.Is<IEnumerable<StandardReportV3Entity>>(c => c.Contains(reportEntity1)), It.IsAny<CancellationToken>()), Times.Once());
            repository.Verify(m => m.DeleteAsync(It.Is<IEnumerable<StandardReportV3Entity>>(c => c.Contains(reportEntity2)), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
