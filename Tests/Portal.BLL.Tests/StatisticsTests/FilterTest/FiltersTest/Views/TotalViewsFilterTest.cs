using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Views;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.Views
{
    [TestClass]
    public class TotalViewsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatWatching = new DomainStatWatching();
            var domainReport = new DomainReport();

            var statWatchingFilter = new Mock<IStatWatchingFilter>();

            var allViewsFilter = new TotalViewsFilter();
            allViewsFilter.Set(statWatchingFilter.Object);

            //Act
            allViewsFilter.Call(domainStatWatching, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.TotalViews);
            statWatchingFilter.Verify(m=>m.Call(domainStatWatching, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatWatching = new DomainStatWatching();
            var domainReport = new DomainReport();
            
            var allViewsFilter = new TotalViewsFilter();

            //Act
            allViewsFilter.Call(domainStatWatching, domainReport);
            allViewsFilter.Call(domainStatWatching, domainReport);
            
            //Assert
            Assert.AreEqual(2, domainReport.TotalViews);
        }
    }
}
