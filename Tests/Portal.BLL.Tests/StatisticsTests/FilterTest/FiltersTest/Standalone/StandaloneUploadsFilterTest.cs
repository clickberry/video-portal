using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Standalone;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.Standalone
{
    [TestClass]
    public class StandaloneUploadsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.Standalone, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.Standalone, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };
            
            var domainReport = new DomainReport();

            var statProjectUploadingFilter = new Mock<IStatProjectUploadingFilter>();

            var standaloneUploadsFilter = new StandaloneUploadsFilter();
            standaloneUploadsFilter.Set(statProjectUploadingFilter.Object);

            //Act
            standaloneUploadsFilter.Call(domainStatProjectState1, domainReport);
            standaloneUploadsFilter.Call(domainStatProjectState2, domainReport);
            standaloneUploadsFilter.Call(domainStatProjectState3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.StandaloneSuccessfulUploads);
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.Standalone, IsSuccessfulUpload = true};
            var domainReport = new DomainReport();

            var standaloneUploadsFilter = new StandaloneUploadsFilter();

            //Act
            standaloneUploadsFilter.Call(domainStatProjectState, domainReport);
            standaloneUploadsFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.StandaloneSuccessfulUploads);
        }
    }
}