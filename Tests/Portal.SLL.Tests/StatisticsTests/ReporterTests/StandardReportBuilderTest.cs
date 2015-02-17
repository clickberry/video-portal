using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Statistics.Helper;
using Portal.BLL.Statistics.Reporter;
using Portal.DTO.Reports;
using Portal.Domain.ProfileContext;
using Portal.Domain.StatisticContext;
using Portal.Mappers.Statistics;
using Portal.SLL.Statistics.Reporter.Abstract;
using Portal.SLL.Statistics.Reporter.Concrete;
using TestExtension;

namespace Portal.SLL.Tests.StatisticsTests.ReporterTests
{
    [TestClass]
    public class StandardReportBuilderTest
    {
        [TestMethod]
        public async Task BuildReportTest()
        {
            //Arrange
            var dayInterval = new Interval();
            var weekInterval = new Interval();
            var monthInterval = new Interval();
            var allInterval = new Interval();
            var dateTime = new DateTime(2013, 01, 11);

            var dayDomainReport = new DomainReport() {Interval = "1"};
            var weekDomainReport = new DomainReport() { Interval = "7" };
            var monthDomainReport = new DomainReport() { Interval = "30" };
            var allDaysDomainReport = new DomainReport() {Interval = "All"};
            var dayReport = new Report();
            var weekReport = new Report();
            var monthReport = new Report();
            var allDaysReport = new Report();

            var repoerts = new List<DomainReport>() {dayDomainReport, weekDomainReport, monthDomainReport, allDaysDomainReport};
            
            var reportService = new Mock<IStandardReportService>();
            var mapper = new Mock<IReportMapper>();
            var intervalHelper = new Mock<IIntervalHelper>();

            var builder = new StandardReportBuilder(reportService.Object, mapper.Object, intervalHelper.Object);

            reportService.Setup(m => m.GetReports(dateTime)).Returns(async ()=>repoerts);
            intervalHelper.Setup(m => m.GetLastDay(dateTime)).Returns(dayInterval);
            intervalHelper.Setup(m => m.GetLastWeek(dateTime)).Returns(weekInterval);
            intervalHelper.Setup(m => m.GetLastMonth(dateTime)).Returns(monthInterval);
            intervalHelper.Setup(m => m.GetAllDays(dateTime)).Returns(allInterval);
            mapper.Setup(m => m.DomainReportToDto(dayDomainReport, dayInterval)).Returns(dayReport);
            mapper.Setup(m => m.DomainReportToDto(weekDomainReport, weekInterval)).Returns(weekReport);
            mapper.Setup(m => m.DomainReportToDto(monthDomainReport, monthInterval)).Returns(monthReport);
            mapper.Setup(m => m.DomainReportToDto(allDaysDomainReport, allInterval)).Returns(allDaysReport);
            
            //Act
            var standartRreport = await builder.BuildReport(dateTime);

            //Assert
            Assert.AreEqual(dateTime, standartRreport.DateTime);
            Assert.AreEqual(dayReport, standartRreport.LastDay);
            Assert.AreEqual(weekReport, standartRreport.LastWeek);
            Assert.AreEqual(monthReport, standartRreport.LastMonth);
            Assert.AreEqual(allDaysReport, standartRreport.AllDays);
        }

        [TestMethod]
        public void BuildReportsTest()
        {
            //Arrange
            var start = new DateTime(2013, 8, 1);
            var end = new DateTime(2013, 8, 12);
            var interval = new Interval() {Start = start, Finish = end};
            
            var reportService = new Mock<IStandardReportService>();
            var mapper = new Mock<IReportMapper>();
            var intervalHelper = new Mock<IIntervalHelper>();
            var interval1 = new Interval();
            var interval2 = new Interval();
            var interval3 = new Interval();
            var domainReport1 = new DomainReport() { Tick = new DateTime(2013, 8, 1) };
            var domainReport2 = new DomainReport() { Tick = new DateTime(2013, 8, 2) };
            var domainReport3 = new DomainReport() { Tick = new DateTime(2013, 8, 4) };
            var report1 = new Report();
            var report2 = new Report();
            var report3 = new Report();
            var domainReports = new List<DomainReport>() {domainReport1, domainReport2, domainReport3};

            var builder = new StandardReportBuilder(reportService.Object, mapper.Object, intervalHelper.Object);

            intervalHelper.Setup(m => m.GetInterval(start, end)).Returns(interval);
            intervalHelper.Setup(m => m.GetInterval(domainReport1.Tick, domainReport1.Tick)).Returns(interval1);
            intervalHelper.Setup(m => m.GetInterval(domainReport2.Tick, domainReport2.Tick)).Returns(interval2);
            intervalHelper.Setup(m => m.GetInterval(domainReport3.Tick, domainReport3.Tick)).Returns(interval3);
            reportService.Setup(m => m.GetDayReports(interval)).Returns(domainReports);
            mapper.Setup(m => m.DomainReportToDto(domainReport1, interval1)).Returns(report1);
            mapper.Setup(m => m.DomainReportToDto(domainReport2, interval2)).Returns(report2);
            mapper.Setup(m => m.DomainReportToDto(domainReport3, interval3)).Returns(report3);

            //Act
            var reports = builder.BuildReports(start, end).ToList();

            //Assert
            Assert.AreEqual(3, reports.Count);
            CollectionAssert.Contains(reports, report1);
            CollectionAssert.Contains(reports, report2);
            CollectionAssert.Contains(reports, report3);
        }
    }
}
