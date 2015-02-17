using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest
{
    [TestClass]
    public class FiltersManagerTest
    {
        private DomainReport _domainReport;
        private Mock<IFiltersFactory> _filtersFactory;
        private Mock<IFiltersChainBuilder> _filtersChainBuilder;

        [TestInitialize]
        public void Initialize()
        {
            _domainReport = new DomainReport();
            _filtersFactory = new Mock<IFiltersFactory>();
            _filtersChainBuilder = new Mock<IFiltersChainBuilder>();
        }

        [TestMethod]
        public void FilterStatWatchingTest()
        {
            //Arrange
            var domainStatWatching = new DomainStatWatching();

            var statWatchingFilters = new List<IStatWatchingFilter>();
            var filter = new Mock<IStatWatchingFilter>();

            _filtersFactory.Setup(m => m.CreateStatWatchingFilters()).Returns(statWatchingFilters);
            _filtersChainBuilder.Setup(m => m.BuildStatWatchingFilter(statWatchingFilters)).Returns(filter.Object);

            var filtersManager = new FiltersManager(_filtersFactory.Object, _filtersChainBuilder.Object);

            //Act
            filtersManager.FilterStatWatching(domainStatWatching, _domainReport);

            //Assert
            filter.Verify(m=>m.Call(domainStatWatching, _domainReport), Times.Once());
        }

        [TestMethod]
        public void FilterStatUserRegistrationTest()
        {
            //Arrange
            var domainStatUserRegistration = new DomainStatUserRegistration();

            var statUserRegistrationFilters = new List<IStatUserRegistrationFilter>();
            var filter = new Mock<IStatUserRegistrationFilter>();

            _filtersFactory.Setup(m => m.CreateStatUserRegistrationFilters()).Returns(statUserRegistrationFilters);
            _filtersChainBuilder.Setup(m => m.BuildStatUserRegistrationFilter(statUserRegistrationFilters)).Returns(filter.Object);

            var filtersManager = new FiltersManager(_filtersFactory.Object, _filtersChainBuilder.Object);

            //Act
            filtersManager.FilterStatUserRegistration(domainStatUserRegistration, _domainReport);

            //Assert
            filter.Verify(m => m.Call(domainStatUserRegistration, _domainReport), Times.Once());
        }

        [TestMethod]
        public void FilterStatProjectUploadingTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState();

            var statProjectUploadingFilters = new List<IStatProjectUploadingFilter>();
            var filter = new Mock<IStatProjectUploadingFilter>();

            _filtersFactory.Setup(m => m.CreateStatProjectUploadingFilters()).Returns(statProjectUploadingFilters);
            _filtersChainBuilder.Setup(m => m.BuildStatProjectUploadingFilter(statProjectUploadingFilters)).Returns(filter.Object);

            var filtersManager = new FiltersManager(_filtersFactory.Object, _filtersChainBuilder.Object);

            //Act
            filtersManager.FilterStatProjectUploading(domainStatProjectState, _domainReport);

            //Assert
            filter.Verify(m => m.Call(domainStatProjectState, _domainReport), Times.Once());
        }

        [TestMethod]
        public void FilterStatProjectDeletionTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState();

            var statProjectDeletionFilters = new List<IStatProjectDeletionFilter>();
            var filter = new Mock<IStatProjectDeletionFilter>();

            _filtersFactory.Setup(m => m.CreateStatProjectDeletionFilters()).Returns(statProjectDeletionFilters);
            _filtersChainBuilder.Setup(m => m.BuildStatProjectDeletionFilter(statProjectDeletionFilters)).Returns(filter.Object);

            var filtersManager = new FiltersManager(_filtersFactory.Object, _filtersChainBuilder.Object);

            //Act
            filtersManager.FilterStatProjectDeletion(domainStatProjectState, _domainReport);

            //Assert
            filter.Verify(m => m.Call(domainStatProjectState, _domainReport), Times.Once());
        }

        [TestMethod]
        public void FilterStatProjectCancellationTest()
        {
            //Arrange
            var domainStatProjectState = new DomainStatProjectState();

            var projectCancellationFilters = new List<IStatProjectCancellationFilter>();
            var filter = new Mock<IStatProjectCancellationFilter>();

            _filtersFactory.Setup(m => m.CreateStatProjectCancellationFilters()).Returns(projectCancellationFilters);
            _filtersChainBuilder.Setup(m => m.BuildStatProjectCancellationFilter(projectCancellationFilters)).Returns(filter.Object);

            var filtersManager = new FiltersManager(_filtersFactory.Object, _filtersChainBuilder.Object);

            //Act
            filtersManager.FilterStatProjectCancellation(domainStatProjectState, _domainReport);

            //Assert
            filter.Verify(m => m.Call(domainStatProjectState, _domainReport), Times.Once());
        }
    }
}
