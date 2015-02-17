using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Reporter;
using Portal.BLL.Statistics.Helper;
using Portal.BLL.Statistics.Reporter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.ReporterTests
{
    [TestClass]
    public class ReportBuilderServiceTest
    {
        [TestMethod]
        public void BuildReportsTest()
        {
            //Arrange
            var dateTime = new DateTime(2013, 3, 30);
            var dayInterval = new Interval();
            var weekInterval = new Interval();
            var monthInterval = new Interval();
            var dayReport = new DomainReport();
            var weekReport = new DomainReport();
            var monthReport = new DomainReport();
            var allDaysReport =new DomainReport();

            var compilerFactory = new Mock<ICompilerFactory>();
            var statisticsCompiler = new Mock<ICompiler>();
            var reportsCompiler1 = new Mock<ICompiler>();
            var reportsCompiler2 = new Mock<ICompiler>();
            var alldaysReportsCompiler = new Mock<ICompiler>();
            
            var intervalHelper = new Mock<IIntervalHelper>();

            var reportBuilder = new ReportBuilderService(compilerFactory.Object, intervalHelper.Object);

            compilerFactory.Setup(m => m.Create(dayInterval)).Returns(statisticsCompiler.Object);
            compilerFactory.Setup(m => m.Create(weekInterval)).Returns(reportsCompiler1.Object);
            compilerFactory.Setup(m => m.Create(monthInterval)).Returns(reportsCompiler2.Object);
            compilerFactory.Setup(m => m.Create(null)).Returns(alldaysReportsCompiler.Object);

            intervalHelper.Setup(m => m.GetLastDay(dateTime)).Returns(dayInterval);
            intervalHelper.Setup(m => m.GetLastWeek(dateTime)).Returns(weekInterval);
            intervalHelper.Setup(m => m.GetLastMonth(dateTime)).Returns(monthInterval);
            statisticsCompiler.Setup(m => m.CompileReport(It.IsAny<DomainReport>())).Returns(dayReport);
            reportsCompiler1.Setup(m => m.CompileReport(dayReport)).Returns(weekReport);
            reportsCompiler2.Setup(m => m.CompileReport(dayReport)).Returns(monthReport);
            alldaysReportsCompiler.Setup(m => m.CompileReport(dayReport)).Returns(allDaysReport);

            //Act
            var reports = reportBuilder.BuildReports(dateTime);

            //Assert
            Assert.AreEqual(4, reports.Count);
            Assert.AreEqual(reports[0], dayReport);
            Assert.AreEqual(reports[1], weekReport);
            Assert.AreEqual(reports[2], monthReport);
            Assert.AreEqual(reports[3], allDaysReport);
        }
    }
}
