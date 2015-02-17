using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.TaggerIPhone;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.TaggerIPhone
{
    [TestClass]
    public class TaggerIPhoneDeletionsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.TaggerIPhone, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.TaggerIPhone, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectDeletionFilter = new Mock<IStatProjectDeletionFilter>();

            var taggerIPhoneDeletionsFilter = new TaggerIPhoneDeletionsFilter();
            taggerIPhoneDeletionsFilter.Set(statProjectDeletionFilter.Object);

            //Act
            taggerIPhoneDeletionsFilter.Call(domainStatProjectState1, domainReport);
            taggerIPhoneDeletionsFilter.Call(domainStatProjectState2, domainReport);
            taggerIPhoneDeletionsFilter.Call(domainStatProjectState3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.TaggerIPhoneDeletions);
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.TaggerIPhone, IsSuccessfulUpload = true };
            
            var domainReport = new DomainReport();

            var taggerIPhoneDeletionsFilter = new TaggerIPhoneDeletionsFilter();

            //Act
            taggerIPhoneDeletionsFilter.Call(domainStatProjectState, domainReport);
            taggerIPhoneDeletionsFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.TaggerIPhoneDeletions);
        }
    }
}