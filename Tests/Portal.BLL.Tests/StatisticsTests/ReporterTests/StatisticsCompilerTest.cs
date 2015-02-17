using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Reporter;
using Portal.BLL.Statistics.Filter;
using Portal.BLL.Statistics.Helper;
using Portal.BLL.Statistics.Reporter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.ReporterTests
{
    [TestClass]
    public class StatisticsCompilerTest
    {
        [TestMethod]
        public void CompileReportTest()
        {
            //Arrange
            const string projectId1 = "projectId1";
            const string projectId2 = "projectId2";
            const int days = 3;
            var startDate = new DateTime(3452345234);
            var finishDate = startDate.Add(TimeSpan.FromDays(days));
            var interval = new Interval() { Start = startDate, Finish = finishDate };
            
            var watching1 = new DomainStatWatching();
            var watching2 = new DomainStatWatching();
            var registration1 = new DomainStatUserRegistration();
            var registration2 = new DomainStatUserRegistration();
            var uploading1 = new DomainStatProjectUploading() { ProjectId = projectId1 };
            var uploading2 = new DomainStatProjectUploading() { ProjectId = projectId2 };
            var deletion1 = new DomainStatProjectDeletion() {ProjectId = projectId1};
            var deletion2 = new DomainStatProjectDeletion() {ProjectId = projectId2};
            var projectState1 = new DomainStatProjectState();
            var projectState2 = new DomainStatProjectState();

            var watchings = new List<DomainStatWatching>(){watching1,watching2};
            var registratios = new List<DomainStatUserRegistration>(){registration1,registration2};
            var uploadings = new List<DomainStatProjectUploading>(){uploading1,uploading2};
            var deletions = new List<DomainStatProjectDeletion>(){deletion1,deletion2};

            var filtersManager = new Mock<IFiltersManager>();
            var statisticsService = new Mock<IStatisticsService>();

            var compiler = new StatisticsCompiler(filtersManager.Object, statisticsService.Object, interval);

            statisticsService.Setup(m => m.GetWatchings(interval)).Returns(watchings);
            statisticsService.Setup(m => m.GetUserRegistrations(interval)).Returns(registratios);
            statisticsService.Setup(m => m.GetProjectUploadings(interval)).Returns(uploadings);
            statisticsService.Setup(m => m.GetProjectDeletions(interval)).Returns(deletions);
            statisticsService.Setup(m => m.GetProjectState(projectId1)).Returns(projectState1);
            statisticsService.Setup(m => m.GetProjectState(projectId2)).Returns(projectState2);

            //Act
            var report = compiler.CompileReport();

            //Assert
            Assert.AreEqual(days.ToString(CultureInfo.InvariantCulture), report.Interval);
            filtersManager.Verify(m => m.FilterStatWatching(watching1, report), Times.Once());
            filtersManager.Verify(m => m.FilterStatWatching(watching2, report), Times.Once());
            filtersManager.Verify(m => m.FilterStatUserRegistration(registration1, report), Times.Once());
            filtersManager.Verify(m => m.FilterStatUserRegistration(registration2, report), Times.Once());
            filtersManager.Verify(m => m.FilterStatProjectUploading(projectState1, report), Times.Once());
            filtersManager.Verify(m => m.FilterStatProjectUploading(projectState2, report), Times.Once());
            filtersManager.Verify(m => m.FilterStatProjectDeletion(projectState1, report), Times.Once());
            filtersManager.Verify(m => m.FilterStatProjectDeletion(projectState2, report), Times.Once());
            filtersManager.Verify(m => m.FilterStatProjectCancellation(projectState1, report), Times.Once());
            filtersManager.Verify(m => m.FilterStatProjectCancellation(projectState2, report), Times.Once());
        }
    }
}
