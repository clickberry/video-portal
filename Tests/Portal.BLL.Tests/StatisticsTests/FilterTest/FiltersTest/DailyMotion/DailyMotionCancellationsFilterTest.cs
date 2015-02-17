using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.DailyMotion;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.DailyMotion
{
    [TestClass]
    public class DailyMotionCancellationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatProjectState1 = new DomainStatProjectState() { Producer = ProductName.DailyMotion, IsSuccessfulUpload = true };
            var domainStatProjectState2 = new DomainStatProjectState() { Producer = ProductName.DailyMotion, IsSuccessfulUpload = false };
            var domainStatProjectState3 = new DomainStatProjectState() { Producer = ProductName.DailyMotion, IsSuccessfulUpload = false };
            var domainStatProjectState4 = new DomainStatProjectState() { Producer = "ProductName", IsSuccessfulUpload = true };

            var domainReport = new DomainReport();

            var statProjectDeletionFilter = new Mock<IStatProjectCancellationFilter>();

            var dailyMotionCancellationFilter = new DailyMotionCancellationFilter();
            dailyMotionCancellationFilter.Set(statProjectDeletionFilter.Object);

            //Act
            dailyMotionCancellationFilter.Call(domainStatProjectState1, domainReport);
            dailyMotionCancellationFilter.Call(domainStatProjectState2, domainReport);
            dailyMotionCancellationFilter.Call(domainStatProjectState3, domainReport);
            dailyMotionCancellationFilter.Call(domainStatProjectState4, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.DailyMotionUploadCancels);
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState1, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState2, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState3, domainReport), Times.Once());
            statProjectDeletionFilter.Verify(m => m.Call(domainStatProjectState4, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState() { Producer = ProductName.DailyMotion, IsSuccessfulUpload = false };
            var domainReport = new DomainReport();

            var dailyMotionCancellationFilter = new DailyMotionCancellationFilter();

            //Act
            dailyMotionCancellationFilter.Call(domainStatProjectState, domainReport);
            dailyMotionCancellationFilter.Call(domainStatProjectState, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.DailyMotionUploadCancels);
        }
    }
}