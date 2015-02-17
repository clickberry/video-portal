using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Statistics.Helper;
using Portal.DAL.Entities.Table;
using Portal.Domain.StatisticContext;
using Portal.Mappers.Statistics;

namespace Portal.Mappers.Tests.StatisticsTests
{
    [TestClass]
    public class ReportMapperTest
    {
        private Mock<ITableValueConverter> _tableValueConverter;

        [TestInitialize]
        public void Initialize()
        {
            _tableValueConverter=new Mock<ITableValueConverter>();
        }

        [TestMethod]
        public void DomainReportToDtoTest()
        {
            //Arrange
            var startDate = new DateTime(21341222);
            var endDate = new DateTime(4352435234);
            var interval = new Interval() {Finish = endDate, Start = startDate};
            var domainReport = new DomainReport()
                {
                    Interval = "interval",
                    AllRegistrations = 12,
                    CicIPadDeletions = 23,
                    CicIPadRegistrations = 34,
                    CicIPadSuccessfulUploads = 45,
                    CicIPadUploadCancels = 445,
                    CicMacDeletions = 56,
                    CicMacRegistrations = 67,
                    CicMacSuccessfulUploads = 78,
                    CicMacUploadCancels = 778,
                    CicPcDeletions = 89,
                    CicPcRegistrations = 90,
                    CicPcSuccessfulUploads = 123,
                    CicPcUploadCancels = 1123,
                    EmailRegistrations = 234,
                    EmbeddedViews = 345,
                    FacebookRegistrations = 456,
                    GoogleRegistrations = 567,
                    TaggerIPhoneDeletions = 678,
                    TaggerIPhoneRegistrations = 789,
                    TaggerIPhoneUploads = 890,
                    TaggerIPhoneUploadCancels = 990,
                    TotalViews = 901,
                    WindowsLiveRegistrations = 1234,
                    YahooRegistrations = 2345,
                    ImageShackDeletions = 11,
                    ImageShackRegistrations = 22,
                    ImageShackSuccessfulUploads = 33,
                    ImageShackUploadCancels = 44,
                    TwitterRegistrations = 111,
                    BrowserRegistrations = 112,
                    OtherRegistrations=113,
                    PlayerDeletions=114,
                    PlayerRegistrations=115,
                    PlayerSuccessfulUploads=116,
                    PlayerUploadCancels=117,
                    StandaloneDeletions=118,
                    StandaloneRegistrations = 119,
                    StandaloneSuccessfulUploads =221,
                    StandaloneUploadCancels=223,
                    TaggerAndroidDeletions=224,
                    TaggerAndroidRegistrations=225,
                    TaggerAndroidSuccessfulUploads=226,
                    TaggerAndroidUploadCancels=227,
                    DailyMotionDeletions = 228,
                    DailyMotionRegistrations = 229,
                    DailyMotionSuccessfulUploads = 331,
                    DailyMotionUploadCancels = 332,
                    VkRegistrations=333,
                    JwPlayerDeletions=334,
                    JwPlayerRegistrations=335,
                    JwPlayerSuccessfulUploads=336,
                    JwPlayerUploadCancels=447,
                    OdnoklassnikiRegistrations = 448
                };

            var mapper = new ReportMapper(_tableValueConverter.Object);

            //Act
            var report = mapper.DomainReportToDto(domainReport, interval);
            
            //Assert
            Assert.AreEqual(startDate, report.Interval.Start);
            Assert.AreEqual(endDate.Subtract(TimeSpan.FromTicks(1)), report.Interval.End);
            Assert.AreEqual(domainReport.AllRegistrations, report.AllRegistrations);
            Assert.AreEqual(domainReport.CicIPadDeletions, report.CicIPadDeletions);
            Assert.AreEqual(domainReport.CicIPadRegistrations, report.CicIPadRegistrations);
            Assert.AreEqual(domainReport.CicIPadSuccessfulUploads, report.CicIPadSuccessfulUploads);
            Assert.AreEqual(domainReport.CicIPadUploadCancels, report.CicIPadUploadCancels);
            Assert.AreEqual(domainReport.CicMacDeletions, report.CicMacDeletions);
            Assert.AreEqual(domainReport.CicMacRegistrations, report.CicMacRegistrations);
            Assert.AreEqual(domainReport.CicMacSuccessfulUploads, report.CicMacSuccessfulUploads);
            Assert.AreEqual(domainReport.CicMacUploadCancels, report.CicMacUploadCancels);
            Assert.AreEqual(domainReport.CicPcDeletions, report.CicPcDeletions);
            Assert.AreEqual(domainReport.CicPcRegistrations, report.CicPcRegistrations);
            Assert.AreEqual(domainReport.CicPcSuccessfulUploads, report.CicPcSuccessfulUploads);
            Assert.AreEqual(domainReport.CicPcUploadCancels, report.CicPcUploadCancels);
            Assert.AreEqual(domainReport.EmailRegistrations, report.EmailRegistrations);
            Assert.AreEqual(domainReport.EmbeddedViews, report.EmbeddedViews);
            Assert.AreEqual(domainReport.FacebookRegistrations, report.FacebookRegistrations);
            Assert.AreEqual(domainReport.GoogleRegistrations, report.GoogleRegistrations);
            Assert.AreEqual(domainReport.TaggerIPhoneDeletions, report.TaggerIPhoneDeletions);
            Assert.AreEqual(domainReport.TaggerIPhoneRegistrations, report.TaggerIPhoneRegistrations);
            Assert.AreEqual(domainReport.TaggerIPhoneUploads, report.TaggerIPhoneSuccessfulUploads);
            Assert.AreEqual(domainReport.TaggerIPhoneUploadCancels, report.TaggerIPhoneUploadCancels);
            Assert.AreEqual(domainReport.TotalViews, report.TotalViews);
            Assert.AreEqual(domainReport.WindowsLiveRegistrations, report.WindowsLiveRegistrations);
            Assert.AreEqual(domainReport.YahooRegistrations, report.YahooRegistrations);
            Assert.AreEqual(domainReport.ImageShackDeletions, report.ImageShackDeletions);
            Assert.AreEqual(domainReport.ImageShackRegistrations, report.ImageShackRegistrations);
            Assert.AreEqual(domainReport.ImageShackSuccessfulUploads, report.ImageShackSuccessfulUploads);
            Assert.AreEqual(domainReport.ImageShackUploadCancels, report.ImageShackUploadCancels);
            Assert.AreEqual(domainReport.TwitterRegistrations, report.TwitterRegistrations);
            Assert.AreEqual(domainReport.OdnoklassnikiRegistrations, report.OdnoklassnikiRegistrations);

            Assert.AreEqual(domainReport.BrowserRegistrations, report.BrowserRegistrations);
            Assert.AreEqual(domainReport.OtherRegistrations, report.OtherRegistrations);
            Assert.AreEqual(domainReport.TaggerAndroidDeletions, report.TaggerAndroidDeletions);
            Assert.AreEqual(domainReport.TaggerAndroidRegistrations, report.TaggerAndroidRegistrations);
            Assert.AreEqual(domainReport.TaggerAndroidSuccessfulUploads, report.TaggerAndroidSuccessfulUploads);
            Assert.AreEqual(domainReport.TaggerAndroidUploadCancels, report.TaggerAndroidUploadCancels);
            Assert.AreEqual(domainReport.StandaloneDeletions, report.StandaloneDeletions);
            Assert.AreEqual(domainReport.StandaloneRegistrations, report.StandaloneRegistrations);
            Assert.AreEqual(domainReport.StandaloneSuccessfulUploads, report.StandaloneSuccessfulUploads);
            Assert.AreEqual(domainReport.StandaloneUploadCancels, report.StandaloneUploadCancels);
            Assert.AreEqual(domainReport.PlayerDeletions, report.PlayerDeletions);
            Assert.AreEqual(domainReport.PlayerRegistrations, report.PlayerRegistrations);
            Assert.AreEqual(domainReport.PlayerSuccessfulUploads, report.PlayerSuccessfulUploads);
            Assert.AreEqual(domainReport.PlayerUploadCancels, report.PlayerUploadCancels);
            Assert.AreEqual(domainReport.DailyMotionDeletions, report.DailyMotionDeletions);
            Assert.AreEqual(domainReport.DailyMotionRegistrations, report.DailyMotionRegistrations);
            Assert.AreEqual(domainReport.DailyMotionSuccessfulUploads, report.DailyMotionSuccessfulUploads);
            Assert.AreEqual(domainReport.DailyMotionUploadCancels, report.DailyMotionUploadCancels);
            Assert.AreEqual(domainReport.VkRegistrations, report.VkRegistrations);
            Assert.AreEqual(domainReport.JwPlayerDeletions, report.JwPlayerDeletions);
            Assert.AreEqual(domainReport.JwPlayerRegistrations, report.JwPlayerRegistrations);
            Assert.AreEqual(domainReport.JwPlayerSuccessfulUploads, report.JwPlayerSuccessfulUpload);
            Assert.AreEqual(domainReport.JwPlayerUploadCancels, report.JwPlayerUploadCancels);
        }

        [TestMethod]
        public void ReportEntityToDomainTest()
        {
            //Arrange
            var dateTime = new DateTime(664245234232223465);

            var reportEntity = new StandardReportV3Entity()
                {
                    Tick="35467568756658785",
                    Interval = "interval",
                    AllRegistrations = 12,
                    CicIPadDeletions = 23,
                    CicIPadRegistrations = 34,
                    CicIPadSuccessfulUploads = 45,
                    CicIPadUploadCancels = 445,
                    CicMacDeletions = 56,
                    CicMacRegistrations = 67,
                    CicMacSuccessfulUploads = 78,
                    CicMacUploadCancels = 778,
                    CicPcDeletions = 89,
                    CicPcRegistrations = 90,
                    CicPcSuccessfulUploads = 123,
                    CicPcUploadCancels = 1123,
                    EmailRegistrations = 234,
                    EmbeddedViews = 345,
                    FacebookRegistrations = 456,
                    GoogleRegistrations = 567,
                    TaggerIPhoneDeletions = 678,
                    TaggerIPhoneRegistrations = 789,
                    TaggerIPhoneSuccessfulUploads = 890,
                    TaggerIPhoneUploadCancels = 990,
                    TotalViews = 901,
                    WindowsLiveRegistrations = 1234,
                    YahooRegistrations = 2345,
                    ImageShackDeletions = 11,
                    ImageShackRegistrations = 22,
                    ImageShackSuccessfulUploads = 33,
                    ImageShackUploadCancels = 44,
                    TwitterRegistrations = 111,
                    BrowserRegistrations = 112,
                    OtherRegistrations = 113,
                    PlayerDeletions = 114,
                    PlayerRegistrations = 115,
                    PlayerSuccessfulUploads = 116,
                    PlayerUploadCancels = 117,
                    StandaloneDeletions = 118,
                    StandaloneRegistrations = 119,
                    StandaloneSuccessfulUploads = 221,
                    StandaloneUploadCancels = 223,
                    TaggerAndroidDeletions = 224,
                    TaggerAndroidRegistrations = 225,
                    TaggerAndroidSuccessfulUploads = 226,
                    TaggerAndroidUploadCancels = 227,
                    DailyMotionDeletions = 228,
                    DailyMotionRegistrations = 229,
                    DailyMotionSuccessfulUploads = 331,
                    DailyMotionUploadCancels = 332,
                    VkRegistrations=333,
                    JwPlayerDeletions=334,
                    JwPlayerRegistrations=335,
                    JwPlayerSuccessfulUploads=336,
                    JwPlayerUploadCancels=337,
                    OdnoklassnikiRegistrations = 448
                };

            _tableValueConverter.Setup(m => m.TickToDateTime(reportEntity.Tick)).Returns(dateTime);

            var mapper = new ReportMapper(_tableValueConverter.Object);

            //Act
            var domainReport = mapper.ReportEntityToDomain(reportEntity);
            
            //Assert
            Assert.AreEqual(dateTime, domainReport.Tick);
            Assert.AreEqual(reportEntity.Interval, domainReport.Interval);
            Assert.AreEqual(reportEntity.AllRegistrations, domainReport.AllRegistrations);
            Assert.AreEqual(reportEntity.CicIPadDeletions, domainReport.CicIPadDeletions);
            Assert.AreEqual(reportEntity.CicIPadRegistrations, domainReport.CicIPadRegistrations);
            Assert.AreEqual(reportEntity.CicIPadSuccessfulUploads, domainReport.CicIPadSuccessfulUploads);
            Assert.AreEqual(reportEntity.CicIPadUploadCancels, domainReport.CicIPadUploadCancels);
            Assert.AreEqual(reportEntity.CicMacDeletions, domainReport.CicMacDeletions);
            Assert.AreEqual(reportEntity.CicMacRegistrations, domainReport.CicMacRegistrations);
            Assert.AreEqual(reportEntity.CicMacSuccessfulUploads, domainReport.CicMacSuccessfulUploads);
            Assert.AreEqual(reportEntity.CicMacUploadCancels, domainReport.CicMacUploadCancels);
            Assert.AreEqual(reportEntity.CicPcDeletions, domainReport.CicPcDeletions);
            Assert.AreEqual(reportEntity.CicPcRegistrations, domainReport.CicPcRegistrations);
            Assert.AreEqual(reportEntity.CicPcSuccessfulUploads, domainReport.CicPcSuccessfulUploads);
            Assert.AreEqual(reportEntity.CicPcUploadCancels, domainReport.CicPcUploadCancels);
            Assert.AreEqual(reportEntity.EmailRegistrations, domainReport.EmailRegistrations);
            Assert.AreEqual(reportEntity.EmbeddedViews, domainReport.EmbeddedViews);
            Assert.AreEqual(reportEntity.FacebookRegistrations, domainReport.FacebookRegistrations);
            Assert.AreEqual(reportEntity.GoogleRegistrations, domainReport.GoogleRegistrations);
            Assert.AreEqual(reportEntity.TaggerIPhoneDeletions, domainReport.TaggerIPhoneDeletions);
            Assert.AreEqual(reportEntity.TaggerIPhoneRegistrations, domainReport.TaggerIPhoneRegistrations);
            Assert.AreEqual(reportEntity.TaggerIPhoneSuccessfulUploads, domainReport.TaggerIPhoneUploads);
            Assert.AreEqual(reportEntity.TaggerIPhoneUploadCancels, domainReport.TaggerIPhoneUploadCancels);
            Assert.AreEqual(reportEntity.TotalViews, domainReport.TotalViews);
            Assert.AreEqual(reportEntity.WindowsLiveRegistrations, domainReport.WindowsLiveRegistrations);
            Assert.AreEqual(reportEntity.YahooRegistrations, domainReport.YahooRegistrations);
            Assert.AreEqual(reportEntity.ImageShackDeletions, domainReport.ImageShackDeletions);
            Assert.AreEqual(reportEntity.ImageShackRegistrations, domainReport.ImageShackRegistrations);
            Assert.AreEqual(reportEntity.ImageShackSuccessfulUploads, domainReport.ImageShackSuccessfulUploads);
            Assert.AreEqual(reportEntity.ImageShackUploadCancels, domainReport.ImageShackUploadCancels);
            Assert.AreEqual(reportEntity.TwitterRegistrations, domainReport.TwitterRegistrations);
            Assert.AreEqual(reportEntity.OdnoklassnikiRegistrations, domainReport.OdnoklassnikiRegistrations);

            Assert.AreEqual(reportEntity.BrowserRegistrations, domainReport.BrowserRegistrations);
            Assert.AreEqual(reportEntity.OtherRegistrations, domainReport.OtherRegistrations);
            Assert.AreEqual(reportEntity.TaggerAndroidDeletions, domainReport.TaggerAndroidDeletions);
            Assert.AreEqual(reportEntity.TaggerAndroidRegistrations, domainReport.TaggerAndroidRegistrations);
            Assert.AreEqual(reportEntity.TaggerAndroidSuccessfulUploads, domainReport.TaggerAndroidSuccessfulUploads);
            Assert.AreEqual(reportEntity.TaggerAndroidUploadCancels, domainReport.TaggerAndroidUploadCancels);
            Assert.AreEqual(reportEntity.StandaloneDeletions, domainReport.StandaloneDeletions);
            Assert.AreEqual(reportEntity.StandaloneRegistrations, domainReport.StandaloneRegistrations);
            Assert.AreEqual(reportEntity.StandaloneSuccessfulUploads, domainReport.StandaloneSuccessfulUploads);
            Assert.AreEqual(reportEntity.StandaloneUploadCancels, domainReport.StandaloneUploadCancels);
            Assert.AreEqual(reportEntity.PlayerDeletions, domainReport.PlayerDeletions);
            Assert.AreEqual(reportEntity.PlayerRegistrations, domainReport.PlayerRegistrations);
            Assert.AreEqual(reportEntity.PlayerSuccessfulUploads, domainReport.PlayerSuccessfulUploads);
            Assert.AreEqual(reportEntity.PlayerUploadCancels, domainReport.PlayerUploadCancels);
            Assert.AreEqual(reportEntity.DailyMotionDeletions, domainReport.DailyMotionDeletions);
            Assert.AreEqual(reportEntity.DailyMotionRegistrations, domainReport.DailyMotionRegistrations);
            Assert.AreEqual(reportEntity.DailyMotionSuccessfulUploads, domainReport.DailyMotionSuccessfulUploads);
            Assert.AreEqual(reportEntity.DailyMotionUploadCancels, domainReport.DailyMotionUploadCancels);
            Assert.AreEqual(reportEntity.VkRegistrations, domainReport.VkRegistrations);
            Assert.AreEqual(reportEntity.JwPlayerDeletions, domainReport.JwPlayerDeletions);
            Assert.AreEqual(reportEntity.JwPlayerRegistrations, domainReport.JwPlayerRegistrations);
            Assert.AreEqual(reportEntity.JwPlayerSuccessfulUploads, domainReport.JwPlayerSuccessfulUploads);
            Assert.AreEqual(reportEntity.JwPlayerUploadCancels, domainReport.JwPlayerUploadCancels);
        }

        [TestMethod]
        public void DomainReportToReportEntityTest()
        {
            //Arrange
            const string formatedDate = "formatedDate";

            var domainReport = new DomainReport()
                {
                    Interval = "interval",
                    AllRegistrations = 12,
                    CicIPadDeletions = 23,
                    CicIPadRegistrations = 34,
                    CicIPadSuccessfulUploads = 45,
                    CicIPadUploadCancels = 445,
                    CicMacDeletions = 56,
                    CicMacRegistrations = 67,
                    CicMacSuccessfulUploads = 78,
                    CicMacUploadCancels = 778,
                    CicPcDeletions = 89,
                    CicPcRegistrations = 90,
                    CicPcSuccessfulUploads = 123,
                    CicPcUploadCancels = 1123,
                    EmailRegistrations = 234,
                    EmbeddedViews = 345,
                    FacebookRegistrations = 456,
                    GoogleRegistrations = 567,
                    TaggerIPhoneDeletions = 678,
                    TaggerIPhoneRegistrations = 789,
                    TaggerIPhoneUploads = 890,
                    TaggerIPhoneUploadCancels = 990,
                    TotalViews = 901,
                    WindowsLiveRegistrations = 1234,
                    YahooRegistrations = 2345,
                    ImageShackDeletions = 11,
                    ImageShackRegistrations = 22,
                    ImageShackSuccessfulUploads = 33,
                    ImageShackUploadCancels = 44,
                    TwitterRegistrations = 111,
                    BrowserRegistrations = 112,
                    OtherRegistrations = 113,
                    PlayerDeletions = 114,
                    PlayerRegistrations = 115,
                    PlayerSuccessfulUploads = 116,
                    PlayerUploadCancels = 117,
                    StandaloneDeletions = 118,
                    StandaloneRegistrations = 119,
                    StandaloneSuccessfulUploads = 221,
                    StandaloneUploadCancels = 223,
                    TaggerAndroidDeletions = 224,
                    TaggerAndroidRegistrations = 225,
                    TaggerAndroidSuccessfulUploads = 226,
                    TaggerAndroidUploadCancels = 227,
                    DailyMotionDeletions = 228,
                    DailyMotionRegistrations = 229,
                    DailyMotionSuccessfulUploads = 331,
                    DailyMotionUploadCancels = 332,
                    VkRegistrations = 333,
                    JwPlayerDeletions = 334,
                    JwPlayerRegistrations = 335,
                    JwPlayerSuccessfulUploads = 336,
                    JwPlayerUploadCancels = 337,
                    OdnoklassnikiRegistrations = 448
                };

            var mapper = new ReportMapper(_tableValueConverter.Object);

            //Act
            var reportEntity = mapper.DomainReportToEntity(domainReport, formatedDate);

            //Assert
            Assert.AreEqual(reportEntity.Tick, formatedDate);
            Assert.AreEqual(domainReport.Interval, reportEntity.Interval);
            Assert.AreEqual(domainReport.AllRegistrations, reportEntity.AllRegistrations);
            Assert.AreEqual(domainReport.CicIPadDeletions, reportEntity.CicIPadDeletions);
            Assert.AreEqual(domainReport.CicIPadRegistrations, reportEntity.CicIPadRegistrations);
            Assert.AreEqual(domainReport.CicIPadSuccessfulUploads, reportEntity.CicIPadSuccessfulUploads);
            Assert.AreEqual(domainReport.CicIPadUploadCancels, reportEntity.CicIPadUploadCancels);
            Assert.AreEqual(domainReport.CicMacDeletions, reportEntity.CicMacDeletions);
            Assert.AreEqual(domainReport.CicMacRegistrations, reportEntity.CicMacRegistrations);
            Assert.AreEqual(domainReport.CicMacSuccessfulUploads, reportEntity.CicMacSuccessfulUploads);
            Assert.AreEqual(domainReport.CicMacUploadCancels, reportEntity.CicMacUploadCancels);
            Assert.AreEqual(domainReport.CicPcDeletions, reportEntity.CicPcDeletions);
            Assert.AreEqual(domainReport.CicPcRegistrations, reportEntity.CicPcRegistrations);
            Assert.AreEqual(domainReport.CicPcSuccessfulUploads, reportEntity.CicPcSuccessfulUploads);
            Assert.AreEqual(domainReport.CicPcUploadCancels, reportEntity.CicPcUploadCancels);
            Assert.AreEqual(domainReport.EmailRegistrations, reportEntity.EmailRegistrations);
            Assert.AreEqual(domainReport.EmbeddedViews, reportEntity.EmbeddedViews);
            Assert.AreEqual(domainReport.FacebookRegistrations, reportEntity.FacebookRegistrations);
            Assert.AreEqual(domainReport.GoogleRegistrations, reportEntity.GoogleRegistrations);
            Assert.AreEqual(domainReport.TaggerIPhoneDeletions, reportEntity.TaggerIPhoneDeletions);
            Assert.AreEqual(domainReport.TaggerIPhoneRegistrations, reportEntity.TaggerIPhoneRegistrations);
            Assert.AreEqual(domainReport.TaggerIPhoneUploads, reportEntity.TaggerIPhoneSuccessfulUploads);
            Assert.AreEqual(domainReport.TaggerIPhoneUploadCancels, reportEntity.TaggerIPhoneUploadCancels);
            Assert.AreEqual(domainReport.TotalViews, reportEntity.TotalViews);
            Assert.AreEqual(domainReport.WindowsLiveRegistrations, reportEntity.WindowsLiveRegistrations);
            Assert.AreEqual(domainReport.YahooRegistrations, reportEntity.YahooRegistrations);
            Assert.AreEqual(domainReport.ImageShackDeletions, reportEntity.ImageShackDeletions);
            Assert.AreEqual(domainReport.ImageShackRegistrations, reportEntity.ImageShackRegistrations);
            Assert.AreEqual(domainReport.ImageShackSuccessfulUploads, reportEntity.ImageShackSuccessfulUploads);
            Assert.AreEqual(domainReport.ImageShackUploadCancels, reportEntity.ImageShackUploadCancels);
            Assert.AreEqual(domainReport.TwitterRegistrations, reportEntity.TwitterRegistrations);
            Assert.AreEqual(domainReport.OdnoklassnikiRegistrations, reportEntity.OdnoklassnikiRegistrations);

            Assert.AreEqual(domainReport.BrowserRegistrations, reportEntity.BrowserRegistrations);
            Assert.AreEqual(domainReport.OtherRegistrations, reportEntity.OtherRegistrations);
            Assert.AreEqual(domainReport.TaggerAndroidDeletions, reportEntity.TaggerAndroidDeletions);
            Assert.AreEqual(domainReport.TaggerAndroidRegistrations, reportEntity.TaggerAndroidRegistrations);
            Assert.AreEqual(domainReport.TaggerAndroidSuccessfulUploads, reportEntity.TaggerAndroidSuccessfulUploads);
            Assert.AreEqual(domainReport.TaggerAndroidUploadCancels, reportEntity.TaggerAndroidUploadCancels);
            Assert.AreEqual(domainReport.StandaloneDeletions, reportEntity.StandaloneDeletions);
            Assert.AreEqual(domainReport.StandaloneRegistrations, reportEntity.StandaloneRegistrations);
            Assert.AreEqual(domainReport.StandaloneSuccessfulUploads, reportEntity.StandaloneSuccessfulUploads);
            Assert.AreEqual(domainReport.StandaloneUploadCancels, reportEntity.StandaloneUploadCancels);
            Assert.AreEqual(domainReport.PlayerDeletions, reportEntity.PlayerDeletions);
            Assert.AreEqual(domainReport.PlayerRegistrations, reportEntity.PlayerRegistrations);
            Assert.AreEqual(domainReport.PlayerSuccessfulUploads, reportEntity.PlayerSuccessfulUploads);
            Assert.AreEqual(domainReport.PlayerUploadCancels, reportEntity.PlayerUploadCancels);
            Assert.AreEqual(domainReport.DailyMotionDeletions, reportEntity.DailyMotionDeletions);
            Assert.AreEqual(domainReport.DailyMotionRegistrations, reportEntity.DailyMotionRegistrations);
            Assert.AreEqual(domainReport.DailyMotionSuccessfulUploads, reportEntity.DailyMotionSuccessfulUploads);
            Assert.AreEqual(domainReport.DailyMotionUploadCancels, reportEntity.DailyMotionUploadCancels);
            Assert.AreEqual(domainReport.VkRegistrations, reportEntity.VkRegistrations);
            Assert.AreEqual(domainReport.JwPlayerDeletions, reportEntity.JwPlayerDeletions);
            Assert.AreEqual(domainReport.JwPlayerRegistrations, reportEntity.JwPlayerRegistrations);
            Assert.AreEqual(domainReport.JwPlayerSuccessfulUploads, reportEntity.JwPlayerSuccessfulUploads);
            Assert.AreEqual(domainReport.JwPlayerUploadCancels, reportEntity.JwPlayerUploadCancels);
        }
    }
}
