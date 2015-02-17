using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Player;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.Player
{
    [TestClass]
    public class PlayerCancellationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.Player, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.Player, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = ProductName.Player, IsSuccessfulUpload = false };
            var domainStatProjectState4 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectDeletionFilter = new Mock<IStatProjectCancellationFilter>();

            var playerCancellationFilter = new PlayerCancellationFilter();
            playerCancellationFilter.Set(statProjectDeletionFilter.Object);

            //Act
            playerCancellationFilter.Call(domainStatProjectState1, domainReport);
            playerCancellationFilter.Call(domainStatProjectState2, domainReport);
            playerCancellationFilter.Call(domainStatProjectState3, domainReport);
            playerCancellationFilter.Call(domainStatProjectState4, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.PlayerUploadCancels);
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState4, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.Player, IsSuccessfulUpload = false };
            var domainReport = new DomainReport();

            var playerCancellationFilter = new PlayerCancellationFilter();

            //Act
            playerCancellationFilter.Call(domainStatProjectState, domainReport);
            playerCancellationFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.PlayerUploadCancels);
        }
    }
}