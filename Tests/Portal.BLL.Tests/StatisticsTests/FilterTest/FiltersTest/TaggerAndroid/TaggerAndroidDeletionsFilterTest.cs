using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.TaggerAndroid;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.TaggerAndroid
{
    [TestClass]
    public class TaggerAndroidDeletionsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.TaggerAndroid, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.TaggerAndroid, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectDeletionFilter = new Mock<IStatProjectDeletionFilter>();

            var taggerAndroidDeletionsFilter = new TaggerAndroidDeletionsFilter();
            taggerAndroidDeletionsFilter.Set(statProjectDeletionFilter.Object);

            //Act
            taggerAndroidDeletionsFilter.Call(domainStatProjectState1, domainReport);
            taggerAndroidDeletionsFilter.Call(domainStatProjectState2, domainReport);
            taggerAndroidDeletionsFilter.Call(domainStatProjectState3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.TaggerAndroidDeletions);
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.TaggerAndroid, IsSuccessfulUpload = true };
            var domainReport = new DomainReport();

            var cicPcDeletionsFilter = new TaggerAndroidDeletionsFilter();

            //Act
            cicPcDeletionsFilter.Call(domainStatProjectState, domainReport);
            cicPcDeletionsFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.TaggerAndroidDeletions);
        }
    }
}