using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.TaggerAndroid;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.TaggerAndroid
{
    [TestClass]
    public class TaggerAndroidCancellationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.TaggerAndroid, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.TaggerAndroid, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = ProductName.TaggerAndroid, IsSuccessfulUpload = false };
            var domainStatProjectState4 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectDeletionFilter = new Mock<IStatProjectCancellationFilter>();

            var taggerAndroidCancellationFilter = new TaggerAndroidCancellationFilter();
            taggerAndroidCancellationFilter.Set(statProjectDeletionFilter.Object);

            //Act
            taggerAndroidCancellationFilter.Call(domainStatProjectState1, domainReport);
            taggerAndroidCancellationFilter.Call(domainStatProjectState2, domainReport);
            taggerAndroidCancellationFilter.Call(domainStatProjectState3, domainReport);
            taggerAndroidCancellationFilter.Call(domainStatProjectState4, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.TaggerAndroidUploadCancels);
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState4, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.TaggerAndroid, IsSuccessfulUpload = false };
            var domainReport = new DomainReport();

            var taggerAndroidCancellationFilter = new TaggerAndroidCancellationFilter();

            //Act
            taggerAndroidCancellationFilter.Call(domainStatProjectState, domainReport);
            taggerAndroidCancellationFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.TaggerAndroidUploadCancels);
        }
    }
}