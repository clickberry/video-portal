using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.CicMac;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.CicMac
{
    [TestClass]
    public class CicMacDeletionsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.CicMac, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.CicMac, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectDeletionFilter = new Mock<IStatProjectDeletionFilter>();

            var cicMacDeletionsFilter = new CicMacDeletionsFilter();
            cicMacDeletionsFilter.Set(statProjectDeletionFilter.Object);

            //Act
            cicMacDeletionsFilter.Call(domainStatProjectState1, domainReport);
            cicMacDeletionsFilter.Call(domainStatProjectState2, domainReport);
            cicMacDeletionsFilter.Call(domainStatProjectState3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.CicMacDeletions);
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.CicMac, IsSuccessfulUpload = true };
            var domainReport = new DomainReport();

            var cicMacDeletionsFilter = new CicMacDeletionsFilter();

            //Act
            cicMacDeletionsFilter.Call(domainStatProjectState, domainReport);
            cicMacDeletionsFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.CicMacDeletions);
        }
    }
}