using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.CicPc;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.CicPc
{
    [TestClass]
    public class CicPcUploadsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.CicPc, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.CicPc, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectUploadingFilter = new Mock<IStatProjectUploadingFilter>();

            var cicPcUploadsFilter = new CicPcUploadsFilter();
            cicPcUploadsFilter.Set(statProjectUploadingFilter.Object);

            //Act
            cicPcUploadsFilter.Call(domainStatProjectState1, domainReport);
            cicPcUploadsFilter.Call(domainStatProjectState2, domainReport);
            cicPcUploadsFilter.Call(domainStatProjectState3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.CicPcSuccessfulUploads);
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.CicPc, IsSuccessfulUpload = true};
            var domainReport = new DomainReport();

            var cicPcUploadsFilter = new CicPcUploadsFilter();

            //Act
            cicPcUploadsFilter.Call(domainStatProjectState, domainReport);
            cicPcUploadsFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.CicPcSuccessfulUploads);
        }
    }
}