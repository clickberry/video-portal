using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.JwPlayer;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.JwPlayer
{
    [TestClass]
    public class JwPlayerUploadsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.JwPlayer, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.JwPlayer, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectUploadingFilter = new Mock<IStatProjectUploadingFilter>();

            var jwPlayerUploadsFilter = new JwPlayerUploadsFilter();
            jwPlayerUploadsFilter.Set(statProjectUploadingFilter.Object);

            //Act
            jwPlayerUploadsFilter.Call(domainStatProjectState1, domainReport);
            jwPlayerUploadsFilter.Call(domainStatProjectState2, domainReport);
            jwPlayerUploadsFilter.Call(domainStatProjectState3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.JwPlayerSuccessfulUploads);
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.JwPlayer, IsSuccessfulUpload = true };
            var domainReport = new DomainReport();

            var jwPlayerUploadsFilter = new JwPlayerUploadsFilter();

            //Act
            jwPlayerUploadsFilter.Call(domainStatProjectState, domainReport);
            jwPlayerUploadsFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.JwPlayerSuccessfulUploads);
        }
    }
}