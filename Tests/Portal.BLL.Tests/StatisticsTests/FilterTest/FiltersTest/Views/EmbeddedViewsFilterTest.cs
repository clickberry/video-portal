using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Views;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.Views
{
    [TestClass]
    public class EmbeddedViewsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            const string portalUrl = "portalUrl";

            var domainReport = new DomainReport();
            var domainStatWatching1 = new DomainStatWatching();
            var domainStatWatching2 = new DomainStatWatching() { UrlReferrer = "UrlReferrer" };
            var domainStatWatching3 = new DomainStatWatching() { UrlReferrer = portalUrl };

            var statWatchingFilter = new Mock<IStatWatchingFilter>();

            var embedViewsFilter = new EmbeddedViewsFilter(portalUrl);
            embedViewsFilter.Set(statWatchingFilter.Object);

            //Act
            embedViewsFilter.Call(domainStatWatching1, domainReport);
            embedViewsFilter.Call(domainStatWatching2, domainReport);
            embedViewsFilter.Call(domainStatWatching3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.EmbeddedViews);
            statWatchingFilter.Verify(m => m.Call(domainStatWatching1, domainReport), Times.Once());
            statWatchingFilter.Verify(m => m.Call(domainStatWatching2, domainReport), Times.Once());
            statWatchingFilter.Verify(m => m.Call(domainStatWatching3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            const string portalUrl = "portalUrl";

            var domainStatWatching = new DomainStatWatching() { UrlReferrer = "UrlReferrer" }; ;
            var domainReport = new DomainReport();

            var embedViewsFilter = new EmbeddedViewsFilter(portalUrl);

            //Act
            embedViewsFilter.Call(domainStatWatching, domainReport);
            embedViewsFilter.Call(domainStatWatching, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.EmbeddedViews);
        }
    }
}