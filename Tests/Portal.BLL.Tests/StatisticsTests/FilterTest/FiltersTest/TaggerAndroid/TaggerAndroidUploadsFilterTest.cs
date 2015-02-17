using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.TaggerAndroid;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.TaggerAndroid
{
    [TestClass]
    public class TaggerAndroidUploadsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.TaggerAndroid, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.TaggerAndroid, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectUploadingFilter = new Mock<IStatProjectUploadingFilter>();

            var taggerAndroidUploadsFilter = new TaggerAndroidUploadsFilter();
            taggerAndroidUploadsFilter.Set(statProjectUploadingFilter.Object);

            //Act
            taggerAndroidUploadsFilter.Call(domainStatProjectState1, domainReport);
            taggerAndroidUploadsFilter.Call(domainStatProjectState2, domainReport);
            taggerAndroidUploadsFilter.Call(domainStatProjectState3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.TaggerAndroidSuccessfulUploads);
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.TaggerAndroid, IsSuccessfulUpload = true};
            var domainReport = new DomainReport();

            var taggerAndroidUploadsFilter = new TaggerAndroidUploadsFilter();

            //Act
            taggerAndroidUploadsFilter.Call(domainStatProjectState, domainReport);
            taggerAndroidUploadsFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.TaggerAndroidSuccessfulUploads);
        }
    }
}