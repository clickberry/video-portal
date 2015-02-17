using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Player;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.Player
{
    [TestClass]
    public class PlayerUploadsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.Player, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.Player, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectUploadingFilter = new Mock<IStatProjectUploadingFilter>();

            var playerUploadsFilter = new PlayerUploadsFilter();
            playerUploadsFilter.Set(statProjectUploadingFilter.Object);

            //Act
            playerUploadsFilter.Call(domainStatProjectState1, domainReport);
            playerUploadsFilter.Call(domainStatProjectState2, domainReport);
            playerUploadsFilter.Call(domainStatProjectState3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.PlayerSuccessfulUploads);
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.Player, IsSuccessfulUpload = true};
            var domainReport = new DomainReport();

            var playerUploadsFilter = new PlayerUploadsFilter();

            //Act
            playerUploadsFilter.Call(domainStatProjectState, domainReport);
            playerUploadsFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.PlayerSuccessfulUploads);
        }
    }
}