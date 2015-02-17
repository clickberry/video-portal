using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.ImageShack;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.ImageShack
{
    [TestClass]
    public class ImageShackDeletionsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.ImageShack, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.ImageShack, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectDeletionFilter = new Mock<IStatProjectDeletionFilter>();

            var imageShackDeletionsFilter = new ImageShackDeletionsFilter();
            imageShackDeletionsFilter.Set(statProjectDeletionFilter.Object);

            //Act
            imageShackDeletionsFilter.Call(domainStatProjectState1, domainReport);
            imageShackDeletionsFilter.Call(domainStatProjectState2, domainReport);
            imageShackDeletionsFilter.Call(domainStatProjectState3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.ImageShackDeletions);
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.ImageShack, IsSuccessfulUpload = true };
            var domainReport = new DomainReport();

            var imageShackDeletionsFilter = new ImageShackDeletionsFilter();

            //Act
            imageShackDeletionsFilter.Call(domainStatProjectState, domainReport);
            imageShackDeletionsFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.ImageShackDeletions);
        }
    }
}