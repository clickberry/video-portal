using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Reporter;
using Portal.BLL.Statistics.Reporter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.ReporterTests
{
    [TestClass]
    public class AllDaysReportsCompilerTest
    {
        [TestMethod]
        public void CompileReportTest()
        {
            //Arrange
            var additionalReport = new DomainReport();
            var lastReport = new DomainReport();
            
            var reportAccumulator = new Mock<IReportAccumulator>();
            var standartReportService = new Mock<IStandardReportService>();

            var compiler = new AllDaysReportsCompiler(reportAccumulator.Object, standartReportService.Object);

            standartReportService.Setup(m => m.GetLastAllDaysReport()).Returns(async () => lastReport);

            //Act
            var report = compiler.CompileReport(additionalReport);

            //Assert
            Assert.AreEqual("All", report.Interval);
            reportAccumulator.Verify(m => m.Accumulate(lastReport, report), Times.Once());
            reportAccumulator.Verify(m => m.Accumulate(additionalReport, report), Times.Once());
        }
    }
}