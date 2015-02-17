using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.DailyMotion;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.DailyMotion
{
    [TestClass]
    public class DailyMotionDeletionsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.DailyMotion, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.DailyMotion, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectDeletionFilter = new Mock<IStatProjectDeletionFilter>();

            var dailyMotionDeletionsFilter = new DailyMotionDeletionsFilter();
            dailyMotionDeletionsFilter.Set(statProjectDeletionFilter.Object);

            //Act
            dailyMotionDeletionsFilter.Call(domainStatProjectState1, domainReport);
            dailyMotionDeletionsFilter.Call(domainStatProjectState2, domainReport);
            dailyMotionDeletionsFilter.Call(domainStatProjectState3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.DailyMotionDeletions);
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.DailyMotion, IsSuccessfulUpload = true };
            var domainReport = new DomainReport();

            var dailyMotionDeletionsFilter = new DailyMotionDeletionsFilter();

            //Act
            dailyMotionDeletionsFilter.Call(domainStatProjectState, domainReport);
            dailyMotionDeletionsFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.DailyMotionDeletions);
        }
    }
}