using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.TaggerIPhone;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.TaggerIPhone
{
    [TestClass]
    public class TaggerIPhoneUploadsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.TaggerIPhone, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.TaggerIPhone, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectUploadingFilter = new Mock<IStatProjectUploadingFilter>();

            var taggerIPhoneUploadsFilter = new TaggerIPhoneUploadsFilter();
            taggerIPhoneUploadsFilter.Set(statProjectUploadingFilter.Object);

            //Act
            taggerIPhoneUploadsFilter.Call(domainStatProjectState1, domainReport);
            taggerIPhoneUploadsFilter.Call(domainStatProjectState2, domainReport);
            taggerIPhoneUploadsFilter.Call(domainStatProjectState3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.TaggerIPhoneUploads);
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.TaggerIPhone, IsSuccessfulUpload = true };
            var domainReport = new DomainReport();

            var taggerIPhoneUploadsFilter = new TaggerIPhoneUploadsFilter();

            //Act
            taggerIPhoneUploadsFilter.Call(domainStatProjectState, domainReport);
            taggerIPhoneUploadsFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.TaggerIPhoneUploads);
        }
    }
}