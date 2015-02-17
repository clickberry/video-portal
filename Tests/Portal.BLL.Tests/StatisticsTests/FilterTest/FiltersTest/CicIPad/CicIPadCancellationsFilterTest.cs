using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.CicIPad;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.CicIPad
{
    [TestClass]
    public class CicIPadCancellationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.CicIPad, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.CicIPad, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = ProductName.CicIPad, IsSuccessfulUpload = false };
            var domainStatProjectState4 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectCancellationFilter = new Mock<IStatProjectCancellationFilter>();

            var cicIPadCancellationFilter = new CicIPadCancellationFilter();
            cicIPadCancellationFilter.Set(statProjectCancellationFilter.Object);

            //Act
            cicIPadCancellationFilter.Call(domainStatProjectState1, domainReport);
            cicIPadCancellationFilter.Call(domainStatProjectState2, domainReport);
            cicIPadCancellationFilter.Call(domainStatProjectState3, domainReport);
            cicIPadCancellationFilter.Call(domainStatProjectState4, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.CicIPadUploadCancels);
            statProjectCancellationFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectCancellationFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectCancellationFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
            statProjectCancellationFilter.Verify(m => m.Call(domainStatProjectState4, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.CicIPad, IsSuccessfulUpload = false };
            var domainReport = new DomainReport();

            var cicIPadCancellationFilter = new CicIPadCancellationFilter();

            //Act
            cicIPadCancellationFilter.Call(domainStatProjectState, domainReport);
            cicIPadCancellationFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.CicIPadUploadCancels);
        }
    }
}