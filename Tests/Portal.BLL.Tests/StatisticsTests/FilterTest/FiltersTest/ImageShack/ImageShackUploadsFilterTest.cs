using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.ImageShack;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.ImageShack
{
    [TestClass]
    public class ImageShackUploadsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.ImageShack, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.ImageShack, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };
            
            var domainReport = new DomainReport();

            var statProjectUploadingFilter = new Mock<IStatProjectUploadingFilter>();

            var imageShackUploadsFilter = new ImageShackUploadsFilter();
            imageShackUploadsFilter.Set(statProjectUploadingFilter.Object);

            //Act
            imageShackUploadsFilter.Call(domainStatProjectState1, domainReport);
            imageShackUploadsFilter.Call(domainStatProjectState2, domainReport);
            imageShackUploadsFilter.Call(domainStatProjectState3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.ImageShackSuccessfulUploads);
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectUploadingFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.ImageShack, IsSuccessfulUpload = true};
            var domainReport = new DomainReport();

            var imageShackUploadsFilter = new ImageShackUploadsFilter();

            //Act
            imageShackUploadsFilter.Call(domainStatProjectState, domainReport);
            imageShackUploadsFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.ImageShackSuccessfulUploads);
        }
    }
}