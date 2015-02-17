using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Player;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.Player
{
    [TestClass]
    public class PlayerDeletionsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.Player, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.Player, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectDeletionFilter = new Mock<IStatProjectDeletionFilter>();

            var playerDeletionsFilter = new PlayerDeletionsFilter();
            playerDeletionsFilter.Set(statProjectDeletionFilter.Object);

            //Act
            playerDeletionsFilter.Call(domainStatProjectState1, domainReport);
            playerDeletionsFilter.Call(domainStatProjectState2, domainReport);
            playerDeletionsFilter.Call(domainStatProjectState3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.PlayerDeletions);
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.Player, IsSuccessfulUpload = true };
            var domainReport = new DomainReport();

            var playerDeletionsFilter = new PlayerDeletionsFilter();

            //Act
            playerDeletionsFilter.Call(domainStatProjectState, domainReport);
            playerDeletionsFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.PlayerDeletions);
        }
    }
}