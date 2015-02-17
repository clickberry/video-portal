using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.CicMac;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.CicMac
{
    [TestClass]
    public class CicMacCancellationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.CicMac, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.CicMac, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = ProductName.CicMac, IsSuccessfulUpload = false };
            var domainStatProjectState4 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectDeletionFilter = new Mock<IStatProjectCancellationFilter>();

            var cicMacCancellationFilter = new CicMacCancellationFilter();
            cicMacCancellationFilter.Set(statProjectDeletionFilter.Object);

            //Act
            cicMacCancellationFilter.Call(domainStatProjectState1, domainReport);
            cicMacCancellationFilter.Call(domainStatProjectState2, domainReport);
            cicMacCancellationFilter.Call(domainStatProjectState3, domainReport);
            cicMacCancellationFilter.Call(domainStatProjectState4, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.CicMacUploadCancels);
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState4, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.CicMac, IsSuccessfulUpload = false };
            var domainReport = new DomainReport();

            var cicMacCancellationFilter = new CicMacCancellationFilter();

            //Act
            cicMacCancellationFilter.Call(domainStatProjectState, domainReport);
            cicMacCancellationFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.CicMacUploadCancels);
        }
    }
}