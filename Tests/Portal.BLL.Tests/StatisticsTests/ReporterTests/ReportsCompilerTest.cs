using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Reporter;
using Portal.BLL.Statistics.Helper;
using Portal.BLL.Statistics.Reporter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.ReporterTests
{
    [TestClass]
    public class ReportsCompilerTest
    {
        [TestMethod]
        public void CompileReportTest()
        {
            //Arrange
            const int days = 12;
            var startDate = new DateTime(3452345234);
            var finishDate = startDate.Add(TimeSpan.FromDays(days));
            var interval = new Interval() { Start = startDate, Finish = finishDate };

            var report1 = new DomainReport();
            var report2 = new DomainReport();

            var reports = new List<DomainReport>() { report1, report2 };

            var reportAccumulator = new Mock<IReportAccumulator>();
            var standartReportService = new Mock<IStandardReportService>();

            var compiler = new ReportsCompiler(reportAccumulator.Object, standartReportService.Object, interval);

            standartReportService.Setup(m => m.GetDayReports(interval)).Returns(reports);
            
            //Act
            var report = compiler.CompileReport();

            //Assert
            Assert.AreEqual(days.ToString(CultureInfo.InvariantCulture), report.Interval);
            reportAccumulator.Verify(m => m.Accumulate(report1, report), Times.Once());
            reportAccumulator.Verify(m => m.Accumulate(report2, report), Times.Once());
            reportAccumulator.Verify(m => m.Accumulate(null, report), Times.Never());
        }

        [TestMethod]
        public void CompileReportWithParamTest()
        {
            //Arrange
            var interval = new Interval();

            var additionalReport = new DomainReport();
            var report1 = new DomainReport();
            var report2 = new DomainReport();

            var reports = new List<DomainReport>() { report1, report2 };

            var reportAccumulator = new Mock<IReportAccumulator>();
            var standartReportService = new Mock<IStandardReportService>();

            var compiler = new ReportsCompiler(reportAccumulator.Object, standartReportService.Object, interval);

            standartReportService.Setup(m => m.GetDayReports(interval)).Returns(reports);

            //Act
            var report = compiler.CompileReport(additionalReport);

            //Assert
            reportAccumulator.Verify(m => m.Accumulate(report1, report), Times.Once());
            reportAccumulator.Verify(m => m.Accumulate(report2, report), Times.Once());
            reportAccumulator.Verify(m => m.Accumulate(additionalReport, report), Times.Once());
        }
    }
}