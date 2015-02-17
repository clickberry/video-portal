using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.BLL.Concrete.Statistics.Filter;
using Portal.BLL.Concrete.Statistics.Filter.Filters;
using Portal.BLL.Concrete.Statistics.Filter.Filters.CicIPad;
using Portal.BLL.Concrete.Statistics.Filter.Filters.CicMac;
using Portal.BLL.Concrete.Statistics.Filter.Filters.CicPc;
using Portal.BLL.Concrete.Statistics.Filter.Filters.DailyMotion;
using Portal.BLL.Concrete.Statistics.Filter.Filters.ImageShack;
using Portal.BLL.Concrete.Statistics.Filter.Filters.JwPlayer;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Player;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Providers;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Registrations;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Standalone;
using Portal.BLL.Concrete.Statistics.Filter.Filters.TaggerAndroid;
using Portal.BLL.Concrete.Statistics.Filter.Filters.TaggerIPhone;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Views;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest
{
    [TestClass]
    public class FiltersFactoryTest
    {
        [TestMethod]
        public void CreateStatWatchingFiltersTest()
        {
            //Arrange
            const string portalUri = "portalUri";

            var filtersFactory = new FiltersFactory(portalUri);

            //Act
            var statWatchingFilters = filtersFactory.CreateStatWatchingFilters();

            //Assert
            Assert.AreEqual(2, statWatchingFilters.Count);
            Assert.IsInstanceOfType(statWatchingFilters[0], typeof(EmbeddedViewsFilter));
            Assert.IsInstanceOfType(statWatchingFilters[1], typeof(TotalViewsFilter));
        }

        [TestMethod]
        public void CreateStatUserRegistrationFiltersTest()
        {
            //Arrange
            const string portalUri = "portalUri";

            var filtersFactory = new FiltersFactory(portalUri);

            //Act
            var statUserRegistrationFilters = filtersFactory.CreateStatUserRegistrationFilters();

            //Assert
            Assert.AreEqual(21, statUserRegistrationFilters.Count);
            Assert.IsInstanceOfType(statUserRegistrationFilters[0], typeof(AllRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[1], typeof (CicIPadRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[2], typeof(CicMacRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[3], typeof(CicPcRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[4], typeof(EmailRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[5], typeof(FacebookRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[6], typeof(GoogleRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[7], typeof(ImageShackRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[8], typeof(TaggerIPhoneRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[9], typeof(WindowsLiveRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[10], typeof(YahooRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[11], typeof(TwitterRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[12], typeof(TaggerAndroidRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[13], typeof(StandaloneRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[14], typeof(PlayerRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[15], typeof(BrowserRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[16], typeof(OtherRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[17], typeof(DailyMotionRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[18], typeof(VkRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[19], typeof (JwPlayerRegistrationsFilter));
            Assert.IsInstanceOfType(statUserRegistrationFilters[20], typeof(OdnoklassnikiRegistrationsFilter));
        }

        [TestMethod]
        public void CreateStatProjectUploadingFiltersTest()
        {
            //Arrange
            const string portalUri = "portalUri";

            var filtersFactory = new FiltersFactory(portalUri);

            //Act
            var statProjectUploadingFilters = filtersFactory.CreateStatProjectUploadingFilters();

            //Assert
            Assert.AreEqual(10, statProjectUploadingFilters.Count);
            Assert.IsInstanceOfType(statProjectUploadingFilters[0], typeof(CicIPadUploadsFilter));
            Assert.IsInstanceOfType(statProjectUploadingFilters[1], typeof(CicMacUploadsFilter));
            Assert.IsInstanceOfType(statProjectUploadingFilters[2], typeof(CicPcUploadsFilter));
            Assert.IsInstanceOfType(statProjectUploadingFilters[3], typeof(ImageShackUploadsFilter));
            Assert.IsInstanceOfType(statProjectUploadingFilters[4], typeof(TaggerIPhoneUploadsFilter));
            Assert.IsInstanceOfType(statProjectUploadingFilters[5], typeof(TaggerAndroidUploadsFilter));
            Assert.IsInstanceOfType(statProjectUploadingFilters[6], typeof(StandaloneUploadsFilter));
            Assert.IsInstanceOfType(statProjectUploadingFilters[7], typeof(PlayerUploadsFilter));
            Assert.IsInstanceOfType(statProjectUploadingFilters[8], typeof(DailyMotionUploadsFilter));
            Assert.IsInstanceOfType(statProjectUploadingFilters[9], typeof(JwPlayerUploadsFilter));
        }

        [TestMethod]
        public void CreateStatProjectDeletionFiltersTest()
        {
            //Arrange
            const string portalUri = "portalUri";

            var filtersFactory = new FiltersFactory(portalUri);

            //Act
            var statProjectDeletionFilters = filtersFactory.CreateStatProjectDeletionFilters().ToList();

            //Assert
            Assert.AreEqual(10, statProjectDeletionFilters.Count);
            Assert.IsInstanceOfType(statProjectDeletionFilters[0], typeof (CicIPadDeletionsFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[1], typeof (CicMacDeletionsFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[2], typeof (CicPcDeletionsFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[3], typeof (ImageShackDeletionsFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[4], typeof (TaggerIPhoneDeletionsFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[5], typeof (TaggerAndroidDeletionsFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[6], typeof (StandaloneDeletionsFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[7], typeof (PlayerDeletionsFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[8], typeof (DailyMotionDeletionsFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[9], typeof(JwPlayerDeletionsFilter));
        }

        [TestMethod]
        public void CreateStatProjectCancellationFiltersTest()
        {
            //Arrange
            const string portalUri = "portalUri";

            var filtersFactory = new FiltersFactory(portalUri);

            //Act
            var statProjectDeletionFilters = filtersFactory.CreateStatProjectCancellationFilters().ToList();

            //Assert
            Assert.AreEqual(10, statProjectDeletionFilters.Count);
            Assert.IsInstanceOfType(statProjectDeletionFilters[0], typeof(CicIPadCancellationFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[1], typeof(CicMacCancellationFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[2], typeof(CicPcCancellationFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[3], typeof(ImageShackCancellationFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[4], typeof(TaggerIPhoneCancellationFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[5], typeof(TaggerAndroidCancellationFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[6], typeof(StandaloneCancellationFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[7], typeof(PlayerCancellationFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[8], typeof(DailyMotionCancellationFilter));
            Assert.IsInstanceOfType(statProjectDeletionFilters[9], typeof(JwPlayerCancellationFilter));
        }
    }
}
