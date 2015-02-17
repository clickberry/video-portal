using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.DailyMotion;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.DailyMotion
{
    [TestClass]
    public class DailyMotionUploadsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.DailyMotion, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.DailyMotion, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectUploadingFilter = new Mock<IStatProjectUploadingFilter>();

            var dailyMotionUploadsFilter = new DailyMotionUploadsFilter();
            dailyMotionUploadsFilter.Set(statProjectUploadingFilter.Object);

            //Act
            dailyMotionUploadsFilter.Call(domainStatProjectState1, domainReport);
            dailyMotionUploadsFilter.Call(domainStatProjectState2, domainReport);
            dailyMotionUploadsFilter.Call(domainStatProjectState3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.DailyMotionSuccessfulUploads);
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatUserRegistration = new DomainStatProjectState() { Producer = ProductName.DailyMotion, IsSuccessfulUpload = true};
            var domainReport = new DomainReport();

            var dailyMotionUploadsFilter = new DailyMotionUploadsFilter();

            //Act
            dailyMotionUploadsFilter.Call(domainStatUserRegistration, domainReport);
            dailyMotionUploadsFilter.Call(domainStatUserRegistration, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.DailyMotionSuccessfulUploads);
        }
    }
}