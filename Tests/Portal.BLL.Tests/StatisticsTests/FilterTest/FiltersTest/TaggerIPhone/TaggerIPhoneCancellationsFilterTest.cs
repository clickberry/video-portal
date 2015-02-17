using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.TaggerIPhone;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.TaggerIPhone
{
    [TestClass]
    public class TaggerIPhoneCancellationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.TaggerIPhone, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.TaggerIPhone, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = ProductName.TaggerIPhone, IsSuccessfulUpload = false };
            var domainStatProjectState4 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectDeletionFilter = new Mock<IStatProjectCancellationFilter>();

            var taggerIPhoneCancellationFilter = new TaggerIPhoneCancellationFilter();
            taggerIPhoneCancellationFilter.Set(statProjectDeletionFilter.Object);

            //Act
            taggerIPhoneCancellationFilter.Call(domainStatProjectState1, domainReport);
            taggerIPhoneCancellationFilter.Call(domainStatProjectState2, domainReport);
            taggerIPhoneCancellationFilter.Call(domainStatProjectState3, domainReport);
            taggerIPhoneCancellationFilter.Call(domainStatProjectState4, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.TaggerIPhoneUploadCancels);
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState4, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.TaggerIPhone, IsSuccessfulUpload = false };
            var domainReport = new DomainReport();

            var taggerIPhoneCancellationFilter = new TaggerIPhoneCancellationFilter();

            //Act
            taggerIPhoneCancellationFilter.Call(domainStatProjectState, domainReport);
            taggerIPhoneCancellationFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.TaggerIPhoneUploadCancels);
        }
    }
}