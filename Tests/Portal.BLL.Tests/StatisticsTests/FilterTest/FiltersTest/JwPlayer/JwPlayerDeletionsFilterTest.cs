using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.JwPlayer;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.JwPlayer
{
    [TestClass]
    public class JwPlayerDeletionsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.JwPlayer, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.JwPlayer, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectDeletionFilter = new Mock<IStatProjectDeletionFilter>();

            var jwPlayerDeletionsFilter = new JwPlayerDeletionsFilter();
            jwPlayerDeletionsFilter.Set(statProjectDeletionFilter.Object);

            //Act
            jwPlayerDeletionsFilter.Call(domainStatProjectState1, domainReport);
            jwPlayerDeletionsFilter.Call(domainStatProjectState2, domainReport);
            jwPlayerDeletionsFilter.Call(domainStatProjectState3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.JwPlayerDeletions);
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.JwPlayer, IsSuccessfulUpload = true };
            var domainReport = new DomainReport();

            var jwPlayerDeletionsFilter = new JwPlayerDeletionsFilter();

            //Act
            jwPlayerDeletionsFilter.Call(domainStatProjectState, domainReport);
            jwPlayerDeletionsFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.JwPlayerDeletions);
        }
    }
}