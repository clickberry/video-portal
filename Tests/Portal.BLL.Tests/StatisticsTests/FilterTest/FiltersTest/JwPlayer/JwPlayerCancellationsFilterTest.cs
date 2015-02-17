using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.JwPlayer;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.JwPlayer
{
    [TestClass]
    public class JwPlayerCancellationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.JwPlayer, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.JwPlayer, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = ProductName.JwPlayer, IsSuccessfulUpload = false };
            var domainStatProjectState4 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectDeletionFilter = new Mock<IStatProjectCancellationFilter>();

            var jwPlayerCancellationFilter = new JwPlayerCancellationFilter();
            jwPlayerCancellationFilter.Set(statProjectDeletionFilter.Object);

            //Act
            jwPlayerCancellationFilter.Call(domainStatProjectState1, domainReport);
            jwPlayerCancellationFilter.Call(domainStatProjectState2, domainReport);
            jwPlayerCancellationFilter.Call(domainStatProjectState3, domainReport);
            jwPlayerCancellationFilter.Call(domainStatProjectState4, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.JwPlayerUploadCancels);
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState4, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.JwPlayer, IsSuccessfulUpload = false };
            var domainReport = new DomainReport();

            var jwPlayerCancellationFilter = new JwPlayerCancellationFilter();

            //Act
            jwPlayerCancellationFilter.Call(domainStatProjectState, domainReport);
            jwPlayerCancellationFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.JwPlayerUploadCancels);
        }
    }
}