using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Standalone;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.Standalone
{
    [TestClass]
    public class StandaloneDeletionsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.Standalone, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.Standalone, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectDeletionFilter = new Mock<IStatProjectDeletionFilter>();

            var standaloneDeletionsFilter = new StandaloneDeletionsFilter();
            standaloneDeletionsFilter.Set(statProjectDeletionFilter.Object);

            //Act
            standaloneDeletionsFilter.Call(domainStatProjectState1, domainReport);
            standaloneDeletionsFilter.Call(domainStatProjectState2, domainReport);
            standaloneDeletionsFilter.Call(domainStatProjectState3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.StandaloneDeletions);
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.Standalone, IsSuccessfulUpload = true };
            var domainReport = new DomainReport();

            var standaloneDeletionsFilter = new StandaloneDeletionsFilter();

            //Act
            standaloneDeletionsFilter.Call(domainStatProjectState, domainReport);
            standaloneDeletionsFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.StandaloneDeletions);
        }
    }
}