using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Reporter;
using Portal.BLL.Statistics.Filter;
using Portal.BLL.Statistics.Helper;
using Portal.BLL.Statistics.Reporter;

namespace Portal.BLL.Tests.StatisticsTests.ReporterTests
{
    [TestClass]
    public class CompilerFactoryTest
    {
        [TestMethod]
        public void CreateTest()
        {
            //Arrange
            var startDate = new DateTime(123456789);
            var finishdate1 = startDate.AddDays(1);
            var finishdate2 = startDate.AddDays(345);
            var dayInterval = new Interval() {Start = startDate, Finish = finishdate1};
            var bigInterval = new Interval() {Start = startDate, Finish = finishdate2};

            var filterManager = new Mock<IFiltersManager>();
            var reportAccumulator = new Mock<IReportAccumulator>();
            var statisticsService = new Mock<IStatisticsService>();
            var reportsService = new Mock<IStandardReportService>();

            var compilerFactory = new CompilerFactory(filterManager.Object, reportAccumulator.Object, statisticsService.Object, reportsService.Object);
            
            //Act
            var statiscticsCompiler = compilerFactory.Create(dayInterval);
            var reportsCompiler = compilerFactory.Create(bigInterval);
            var allDaysReportsCompiler = compilerFactory.Create(null);

            //Assert
            Assert.IsInstanceOfType(statiscticsCompiler, typeof(StatisticsCompiler));
            Assert.IsInstanceOfType(reportsCompiler, typeof(ReportsCompiler));
            Assert.IsInstanceOfType(allDaysReportsCompiler, typeof(AllDaysReportsCompiler));
        }
    }
}
