using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.BLL.Concrete.Statistics.Reporter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.ReporterTests
{
    [TestClass]
    public class ReportAccumulatorTest
    {
        [TestMethod]
        public void AggrigateTest()
        {
            //Arrange
            var dayReport = new DomainReport()
                {
                    AllRegistrations = 123,
                    CicIPadDeletions = 234,
                    CicIPadRegistrations = 345,
                    CicIPadSuccessfulUploads = 456,
                    CicIPadUploadCancels = 4560,
                    CicMacDeletions = 567,
                    CicMacRegistrations = 678,
                    CicMacSuccessfulUploads = 789,
                    CicMacUploadCancels = 7890,
                    CicPcDeletions = 890,
                    CicPcRegistrations = 901,
                    CicPcSuccessfulUploads = 12,
                    CicPcUploadCancels = 120,
                    EmailRegistrations = 23,
                    EmbeddedViews = 34,
                    FacebookRegistrations = 45,
                    GoogleRegistrations = 56,
                    ImageShackDeletions = 67,
                    ImageShackRegistrations = 78,
                    ImageShackSuccessfulUploads = 89,
                    ImageShackUploadCancels = 890,
                    TaggerIPhoneDeletions = 90,
                    TaggerIPhoneRegistrations = 1,
                    TaggerIPhoneUploads = 2,
                    TaggerIPhoneUploadCancels = 20,
                    TotalViews = 3,
                    WindowsLiveRegistrations = 4,
                    YahooRegistrations = 5,
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
                    JwPlayerSuccessfulUploads = 336,
                    JwPlayerUploadCancels = 337,
                    OdnoklassnikiRegistrations = 338
                };

            var domainReport = new DomainReport();

            var reportAccumulator = new ReportAccumulator();

            //Act
            reportAccumulator.Accumulate(dayReport, domainReport);
            reportAccumulator.Accumulate(dayReport, domainReport);

            //Assert
            Assert.AreEqual(dayReport.AllRegistrations*2, domainReport.AllRegistrations);
            Assert.AreEqual(dayReport.CicIPadDeletions*2, domainReport.CicIPadDeletions);
            Assert.AreEqual(dayReport.CicIPadRegistrations * 2, domainReport.CicIPadRegistrations);
            Assert.AreEqual(dayReport.CicIPadSuccessfulUploads * 2, domainReport.CicIPadSuccessfulUploads);
            Assert.AreEqual(dayReport.CicIPadUploadCancels * 2, domainReport.CicIPadUploadCancels);
            Assert.AreEqual(dayReport.CicMacDeletions * 2, domainReport.CicMacDeletions);
            Assert.AreEqual(dayReport.CicMacRegistrations * 2, domainReport.CicMacRegistrations);
            Assert.AreEqual(dayReport.CicMacSuccessfulUploads * 2, domainReport.CicMacSuccessfulUploads);
            Assert.AreEqual(dayReport.CicMacUploadCancels * 2, domainReport.CicMacUploadCancels);
            Assert.AreEqual(dayReport.CicPcDeletions * 2, domainReport.CicPcDeletions);
            Assert.AreEqual(dayReport.CicPcRegistrations * 2, domainReport.CicPcRegistrations);
            Assert.AreEqual(dayReport.CicPcSuccessfulUploads * 2, domainReport.CicPcSuccessfulUploads);
            Assert.AreEqual(dayReport.CicPcUploadCancels*2, domainReport.CicPcUploadCancels);
            Assert.AreEqual(dayReport.EmailRegistrations * 2, domainReport.EmailRegistrations);
            Assert.AreEqual(dayReport.EmbeddedViews * 2, domainReport.EmbeddedViews);
            Assert.AreEqual(dayReport.FacebookRegistrations * 2, domainReport.FacebookRegistrations);
            Assert.AreEqual(dayReport.GoogleRegistrations * 2, domainReport.GoogleRegistrations);
            Assert.AreEqual(dayReport.ImageShackDeletions * 2, domainReport.ImageShackDeletions);
            Assert.AreEqual(dayReport.ImageShackRegistrations * 2, domainReport.ImageShackRegistrations);
            Assert.AreEqual(dayReport.ImageShackSuccessfulUploads*2, domainReport.ImageShackSuccessfulUploads);
            Assert.AreEqual(dayReport.ImageShackUploadCancels * 2, domainReport.ImageShackUploadCancels);
            Assert.AreEqual(dayReport.TaggerIPhoneDeletions * 2, domainReport.TaggerIPhoneDeletions);
            Assert.AreEqual(dayReport.TaggerIPhoneRegistrations * 2, domainReport.TaggerIPhoneRegistrations);
            Assert.AreEqual(dayReport.TaggerIPhoneUploads * 2, domainReport.TaggerIPhoneUploads);
            Assert.AreEqual(dayReport.TaggerIPhoneUploadCancels * 2, domainReport.TaggerIPhoneUploadCancels);
            Assert.AreEqual(dayReport.TotalViews * 2, domainReport.TotalViews);
            Assert.AreEqual(dayReport.WindowsLiveRegistrations * 2, domainReport.WindowsLiveRegistrations);
            Assert.AreEqual(dayReport.YahooRegistrations * 2, domainReport.YahooRegistrations);
            Assert.AreEqual(dayReport.TwitterRegistrations * 2, domainReport.TwitterRegistrations);
            Assert.AreEqual(dayReport.OdnoklassnikiRegistrations * 2, domainReport.OdnoklassnikiRegistrations);

            Assert.AreEqual(dayReport.BrowserRegistrations * 2, domainReport.BrowserRegistrations);
            Assert.AreEqual(dayReport.OtherRegistrations * 2, domainReport.OtherRegistrations);
            Assert.AreEqual(dayReport.TaggerAndroidDeletions * 2, domainReport.TaggerAndroidDeletions);
            Assert.AreEqual(dayReport.TaggerAndroidRegistrations * 2, domainReport.TaggerAndroidRegistrations);
            Assert.AreEqual(dayReport.TaggerAndroidSuccessfulUploads * 2, domainReport.TaggerAndroidSuccessfulUploads);
            Assert.AreEqual(dayReport.TaggerAndroidUploadCancels * 2, domainReport.TaggerAndroidUploadCancels);
            Assert.AreEqual(dayReport.StandaloneDeletions * 2, domainReport.StandaloneDeletions);
            Assert.AreEqual(dayReport.StandaloneRegistrations * 2, domainReport.StandaloneRegistrations);
            Assert.AreEqual(dayReport.StandaloneSuccessfulUploads * 2, domainReport.StandaloneSuccessfulUploads);
            Assert.AreEqual(dayReport.StandaloneUploadCancels * 2, domainReport.StandaloneUploadCancels);
            Assert.AreEqual(dayReport.PlayerDeletions * 2, domainReport.PlayerDeletions);
            Assert.AreEqual(dayReport.PlayerRegistrations * 2, domainReport.PlayerRegistrations);
            Assert.AreEqual(dayReport.PlayerSuccessfulUploads * 2, domainReport.PlayerSuccessfulUploads);
            Assert.AreEqual(dayReport.PlayerUploadCancels * 2, domainReport.PlayerUploadCancels);
            Assert.AreEqual(dayReport.DailyMotionDeletions * 2, domainReport.DailyMotionDeletions);
            Assert.AreEqual(dayReport.DailyMotionRegistrations * 2, domainReport.DailyMotionRegistrations);
            Assert.AreEqual(dayReport.DailyMotionSuccessfulUploads * 2, domainReport.DailyMotionSuccessfulUploads);
            Assert.AreEqual(dayReport.DailyMotionUploadCancels * 2, domainReport.DailyMotionUploadCancels);
            Assert.AreEqual(dayReport.VkRegistrations*2, domainReport.VkRegistrations);
            Assert.AreEqual(dayReport.JwPlayerDeletions*2, domainReport.JwPlayerDeletions);
            Assert.AreEqual(dayReport.JwPlayerRegistrations*2, domainReport.JwPlayerRegistrations);
            Assert.AreEqual(dayReport.JwPlayerSuccessfulUploads*2, domainReport.JwPlayerSuccessfulUploads);
            Assert.AreEqual(dayReport.JwPlayerUploadCancels*2, domainReport.JwPlayerUploadCancels);
        }
    }
}
