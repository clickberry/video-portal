using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Generator;
using Portal.BLL.Statistics.Reporter;
using Portal.Domain.StatisticContext;
using Portal.Exceptions.CRUD;

namespace Portal.BLL.Tests.StatisticsTests.GeneratorTest
{
    [TestClass]
    public class ReportGeneratorTest
    {
        [TestMethod]
        public async Task GenerateTest()
        {
            //Arrange
            var dateTime = new DateTime(2013, 4, 1);
            var domains=new List<DomainReport>();
            var reportService = new Mock<IStandardReportService>();
            var reportBuilder = new Mock<IReportBuilderService>();

            var reportGenerator = new ReportGenerator(reportService.Object, reportBuilder.Object);

            reportBuilder.Setup(m => m.BuildReports(dateTime)).Returns(domains);
            reportService.Setup(m => m.WriteReports(domains, dateTime)).Returns(async () => { });

            //Act & Assert
            await reportGenerator.Generate(dateTime);
        }

        [TestMethod]
        public async Task GenerateIfNotExistReportExistTest()
        {
            //Arrange
            var dateTime = new DateTime(2013, 4, 1);
            var domains = new List<DomainReport>();
            var reportService = new Mock<IStandardReportService>();
            var reportBuilder = new Mock<IReportBuilderService>();

            var reportGenerator = new ReportGenerator(reportService.Object, reportBuilder.Object);

            reportBuilder.Setup(m => m.BuildReports(dateTime)).Returns(domains);
            reportService.Setup(m => m.GetReports(dateTime)).Returns(async () => domains);

            //Act & Assert
            await reportGenerator.GenerateIfNotExist(dateTime);

            //Assert
            reportBuilder.Verify(m => m.BuildReports(It.IsAny<DateTime>()), Times.Never());
        }

        [TestMethod]
        public async Task GenerateIfNotExistReportNotExistTest()
        {
            //Arrange
            var dateTime = new DateTime(2013, 4, 1);
            var domains = new List<DomainReport>();
            var reportService = new Mock<IStandardReportService>();
            var reportBuilder = new Mock<IReportBuilderService>();

            var reportGenerator = new ReportGenerator(reportService.Object, reportBuilder.Object);
            
            reportBuilder.Setup(m => m.BuildReports(dateTime)).Returns(domains);
            reportService.Setup(m => m.GetReports(dateTime)).Throws(new NotFoundException());
            reportService.Setup(m => m.WriteReports(domains, dateTime)).Returns(async () => { });

            //Act & Assert
            await reportGenerator.GenerateIfNotExist(dateTime);
        }

       
    }
}
