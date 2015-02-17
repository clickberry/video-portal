using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter;
using Portal.BLL.Statistics.Filter;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest
{
    [TestClass]
    public class FiltersChainBuilderTest
    {
        [TestMethod]
        public void BuildStatWatchingFilterTest()
        {
            //Arrange
            var filter1 = new Mock<IStatWatchingFilter>();
            var filter2 = new Mock<IStatWatchingFilter>();
            var filter3 = new Mock<IStatWatchingFilter>();

            var filters = new List<IStatWatchingFilter>() { filter1.Object, filter2.Object, filter3.Object };
            var filtersChainBuilder = new FiltersChainBuilder();

            //Act
            var filter = filtersChainBuilder.BuildStatWatchingFilter(filters);

            //Assert
            Assert.AreEqual(filter1.Object, filter);
            filter1.Verify(m=>m.Set(filter2.Object));
            filter2.Verify(m => m.Set(filter3.Object));
        }

        [TestMethod]
        public void BuildStatUserRegistrationFilterTest()
        {
            //Arrange
            var filter1 = new Mock<IStatUserRegistrationFilter>();
            var filter2 = new Mock<IStatUserRegistrationFilter>();
            var filter3 = new Mock<IStatUserRegistrationFilter>();

            var filters = new List<IStatUserRegistrationFilter>() { filter1.Object, filter2.Object, filter3.Object };
            var filtersChainBuilder = new FiltersChainBuilder();

            //Act
            var filter = filtersChainBuilder.BuildStatUserRegistrationFilter(filters);

            //Assert
            Assert.AreEqual(filter1.Object, filter);
            filter1.Verify(m => m.Set(filter2.Object));
            filter2.Verify(m => m.Set(filter3.Object));
        }

        [TestMethod]
        public void BuildStatProjectUploadingFilterTest()
        {
            //Arrange
            var filter1 = new Mock<IStatProjectUploadingFilter>();
            var filter2 = new Mock<IStatProjectUploadingFilter>();
            var filter3 = new Mock<IStatProjectUploadingFilter>();

            var filters = new List<IStatProjectUploadingFilter>() { filter1.Object, filter2.Object, filter3.Object };
            var filtersChainBuilder = new FiltersChainBuilder();

            //Act
            var filter = filtersChainBuilder.BuildStatProjectUploadingFilter(filters);

            //Assert
            Assert.AreEqual(filter1.Object, filter);
            filter1.Verify(m => m.Set(filter2.Object));
            filter2.Verify(m => m.Set(filter3.Object));
        }

        [TestMethod]
        public void BuildStatProjectDeletionFilterTest()
        {
            //Arrange
            var filter1 = new Mock<IStatProjectDeletionFilter>();
            var filter2 = new Mock<IStatProjectDeletionFilter>();
            var filter3 = new Mock<IStatProjectDeletionFilter>();

            var filters = new List<IStatProjectDeletionFilter>() { filter1.Object, filter2.Object, filter3.Object };
            var filtersChainBuilder = new FiltersChainBuilder();

            //Act
            var filter = filtersChainBuilder.BuildStatProjectDeletionFilter(filters);

            //Assert
            Assert.AreEqual(filter1.Object, filter);
            filter1.Verify(m => m.Set(filter2.Object));
            filter2.Verify(m => m.Set(filter3.Object));
        }

        [TestMethod]
        public void BuildStatProjectCancellationFilterTest()
        {
            //Arrange
            var filter1 = new Mock<IStatProjectCancellationFilter>();
            var filter2 = new Mock<IStatProjectCancellationFilter>();
            var filter3 = new Mock<IStatProjectCancellationFilter>();

            var filters = new List<IStatProjectCancellationFilter>() { filter1.Object, filter2.Object, filter3.Object };
            var filtersChainBuilder = new FiltersChainBuilder();

            //Act
            var filter = filtersChainBuilder.BuildStatProjectCancellationFilter(filters);

            //Assert
            Assert.AreEqual(filter1.Object, filter);
            filter1.Verify(m => m.Set(filter2.Object));
            filter2.Verify(m => m.Set(filter3.Object));
        }

        [TestMethod]
        public void BuildSuccessfulProjectUploadingsFilterTest()
        {
            //Arrange
            var filter1 = new Mock<IReportFilter>();
            var filter2 = new Mock<IReportFilter>();
            var filter3 = new Mock<IReportFilter>();

            var filters = new List<IReportFilter>() { filter1.Object, filter2.Object, filter3.Object };
            var filtersChainBuilder = new FiltersChainBuilder();

            //Act
            var filter = filtersChainBuilder.BuildSuccessfulProjectUploadingsFilter(filters);

            //Assert
            Assert.AreEqual(filter1.Object, filter);
            filter1.Verify(m => m.Set(filter2.Object));
            filter2.Verify(m => m.Set(filter3.Object));
        }
    }
}
