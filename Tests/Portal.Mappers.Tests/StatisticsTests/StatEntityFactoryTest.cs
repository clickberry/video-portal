using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.DTO.Projects;
using Portal.Domain.StatisticContext;
using Portal.Mappers.Statistics;

namespace Portal.Mappers.Tests.StatisticsTests
{
    [TestClass]
    public class StatEntityFactoryTest
    {
        const string TickWithGuid = "tick";
        const string EventId = "eventId";
        readonly DateTime _dateTime = new DateTime(2013, 2, 8);

        const string UserId = "userId";
        const string AnonymousId = "AnonymousId";
        const bool IsAuthenticated = true;

        const string HttpMethod = "httpMethod";
        const int StatusCode = 2345;
        const string Url = "url";
        const string UrlReferrer = "urlReferrer";
        const string UserAgent = "userAgent";
        const string UserHostAddress = "userHostAddress";
        const string UserHostName = "userHostName";
        const string ProductName = "productName";
        const string Email = "email";
        const string UserName = "userName";
        const string IdentityProvider = "identityProvider";
        
        readonly string[] _userLanguages = { "en-us", "ru-ru", "bla-bla" };

        private DomainActionData _domain;
        
        [TestInitialize]
        public void Initialize()
        {
            _domain = new DomainActionData()
                {
                    UserAgent = UserAgent,
                    HttpMethod = HttpMethod,
                    IsAuthenticated = IsAuthenticated,
                    StatusCode = StatusCode,
                    Url = Url,
                    UrlReferrer = UrlReferrer,
                    UserHostAddress = UserHostAddress,
                    UserHostName = UserHostName,
                    UserId = UserId,
                    UserLanguages = _userLanguages,
                    AnonymousId = AnonymousId,
                    UserEmail=Email,
                    UserName=UserName,
                    IdentityProvider=IdentityProvider
                };
        }

        [TestMethod]
        public void CreateHttpMessageEntityTest()
        {
            //Arrange
            const string userLanguages = "userLanguages";

            var tableValueConverter = new Mock<ITableValueConverter>();
            var statEntityFactory = new StatEntityFactory(tableValueConverter.Object);

            tableValueConverter.Setup(m => m.DateTimeToTickWithGuid(_dateTime)).Returns(TickWithGuid);
            tableValueConverter.Setup(m => m.ArrayToString(_userLanguages)).Returns(userLanguages);

            //Act
            var httpMessage = statEntityFactory.CreateHttpMessageEntity(EventId, _dateTime, _domain);

            //Assert
            Assert.AreEqual(TickWithGuid, httpMessage.Tick);
            Assert.AreEqual(EventId, httpMessage.EventId);
            Assert.AreEqual(_dateTime, httpMessage.DateTime);
            Assert.AreEqual(AnonymousId, httpMessage.AnonymousId);
            Assert.AreEqual(HttpMethod, httpMessage.HttpMethod);
            Assert.AreEqual(IsAuthenticated, httpMessage.IsAuthenticated);
            Assert.AreEqual(StatusCode, httpMessage.StatusCode);
            Assert.AreEqual(Url, httpMessage.Url);
            Assert.AreEqual(UrlReferrer, httpMessage.UrlReferrer);
            Assert.AreEqual(UserAgent, httpMessage.UserAgent);
            Assert.AreEqual(UserHostAddress, httpMessage.UserHostAddress);
            Assert.AreEqual(UserHostName, httpMessage.UserHostName);
            Assert.AreEqual(UserId, httpMessage.UserId);
            Assert.AreEqual(userLanguages, httpMessage.UserLanguages);
        }

        [TestMethod]
        public void CreateWatchingEntityTest()
        {
            //Arrange
            const string projectId = "projectId";

            var tableValueConverter = new Mock<ITableValueConverter>();
            var statEntityFactory = new StatEntityFactory(tableValueConverter.Object);

            tableValueConverter.Setup(m => m.DateTimeToTickWithGuid(_dateTime)).Returns(TickWithGuid);

            //Act
            var watchingEntity = statEntityFactory.CreateWatchingEntity(EventId, _dateTime, _domain, projectId);

            //Assert
            Assert.AreEqual(TickWithGuid, watchingEntity.Tick);
            Assert.AreEqual(EventId, watchingEntity.EventId);
            Assert.AreEqual(_dateTime, watchingEntity.DateTime);
            Assert.AreEqual(AnonymousId, watchingEntity.AnonymousId);
            Assert.AreEqual(IsAuthenticated, watchingEntity.IsAuthenticated);
            Assert.AreEqual(UrlReferrer, watchingEntity.UrlReferrer);
            Assert.AreEqual(projectId, watchingEntity.ProjectId);
            Assert.AreEqual(UserId, watchingEntity.UserId);
        }
        
        [TestMethod]
        public void CreateProjectDeletionEntityTest()
        {
            //Arrange
            const string productName = "productName";
            const string projectId = "projectId";

            var tableValueConverter = new Mock<ITableValueConverter>();
            var statEntityFactory = new StatEntityFactory(tableValueConverter.Object);

            tableValueConverter.Setup(m => m.DateTimeToTickWithGuid(_dateTime)).Returns(TickWithGuid);
            tableValueConverter.Setup(m => m.UserAgentToProductName(UserAgent)).Returns(productName);

            //Act
            var projectDeletionEntity = statEntityFactory.CreateProjectDeletionEntity(EventId, _dateTime, _domain, projectId);

            //Assert
            Assert.AreEqual(TickWithGuid, projectDeletionEntity.Tick);
            Assert.AreEqual(EventId, projectDeletionEntity.EventId);
            Assert.AreEqual(_dateTime, projectDeletionEntity.DateTime);
            Assert.AreEqual(productName, projectDeletionEntity.ProductName);
            Assert.AreEqual(projectId, projectDeletionEntity.ProjectId);
            Assert.AreEqual(UserId, projectDeletionEntity.UserId);
        }
        
        [TestMethod]
        public void CreateProjectUploadingEntityTest()
        {
            //Arrange
            const string productName = "productName";
            const string projectId = "projectId";
            const string projectName = "projectName";
            const string productVersion = "12.3";

            const ProjectType projectType = ProjectType.Tag;
            const ProjectSubtype projectSubtype = ProjectSubtype.Friend;

            var tableValueConverter = new Mock<ITableValueConverter>();
            var statEntityFactory = new StatEntityFactory(tableValueConverter.Object);

            tableValueConverter.Setup(m => m.DateTimeToTickWithGuid(_dateTime)).Returns(TickWithGuid);
            tableValueConverter.Setup(m => m.UserAgentToProductName(UserAgent)).Returns(productName);
            tableValueConverter.Setup(m => m.UserAgentToVersion(UserAgent)).Returns(productVersion);

            //Act
            var projectUploadingEntity = statEntityFactory.CreateProjectUploadingEntity(EventId, _dateTime, _domain, projectId, projectName, projectType, projectSubtype);

            //Assert
            Assert.AreEqual(TickWithGuid, projectUploadingEntity.Tick);
            Assert.AreEqual(EventId, projectUploadingEntity.EventId);
            Assert.AreEqual(_dateTime, projectUploadingEntity.DateTime);
            Assert.AreEqual(productName, projectUploadingEntity.ProductName);
            Assert.AreEqual(UserId, projectUploadingEntity.UserId);
            Assert.AreEqual(projectId, projectUploadingEntity.ProjectId);
            Assert.AreEqual(projectName, projectUploadingEntity.ProjectName);
            Assert.AreEqual(IdentityProvider, projectUploadingEntity.IdentityProvider);
            Assert.AreEqual(productVersion, projectUploadingEntity.ProductVersion);
            Assert.AreEqual((int)projectType, projectUploadingEntity.ProjectType);
            Assert.AreEqual((int)projectSubtype, projectUploadingEntity.TagType);
        }

        [TestMethod]
        public void CreateUserRegistrationEntityTest()
        {
            //Arrange
            var tableValueConverter = new Mock<ITableValueConverter>();
            var statEntityFactory = new StatEntityFactory(tableValueConverter.Object);

            tableValueConverter.Setup(m => m.DateTimeToTickWithGuid(_dateTime)).Returns(TickWithGuid);
            tableValueConverter.Setup(m => m.UserAgentToProductName(UserAgent)).Returns(ProductName);

            //Act
            var userRegistrationEntity = statEntityFactory.CreateUserRegistrationEntity(EventId, _dateTime, _domain);

            //Assert
            Assert.AreEqual(TickWithGuid, userRegistrationEntity.Tick);
            Assert.AreEqual(EventId, userRegistrationEntity.EventId);
            Assert.AreEqual(_dateTime, userRegistrationEntity.DateTime);
            Assert.AreEqual(ProductName, userRegistrationEntity.ProductName);
            Assert.AreEqual(IdentityProvider, userRegistrationEntity.IdentityProvider);
            Assert.AreEqual(UserId, userRegistrationEntity.UserId);
            Assert.AreEqual(Email, userRegistrationEntity.Email);
            Assert.AreEqual(UserName, userRegistrationEntity.UserName);
        }

        [TestMethod]
        public void CreateUserLoginEntityTest()
        {
            //Arrange
            var tableValueConverter = new Mock<ITableValueConverter>();
            var statEntityFactory = new StatEntityFactory(tableValueConverter.Object);

            tableValueConverter.Setup(m => m.DateTimeToTickWithGuid(_dateTime)).Returns(TickWithGuid);
            tableValueConverter.Setup(m => m.UserAgentToProductName(UserAgent)).Returns(ProductName);

            //Act
            var userLoginEntity = statEntityFactory.CreateUserLoginEntity(EventId, _dateTime, _domain);

            //Assert
            Assert.AreEqual(TickWithGuid, userLoginEntity.Tick);
            Assert.AreEqual(EventId, userLoginEntity.EventId);
            Assert.AreEqual(_dateTime, userLoginEntity.DateTime);
            Assert.AreEqual(ProductName, userLoginEntity.ProductName);
            Assert.AreEqual(IdentityProvider, userLoginEntity.IdentityProvider);
            Assert.AreEqual(UserId, userLoginEntity.UserId);
            Assert.AreEqual(Email, userLoginEntity.Email);
            Assert.AreEqual(UserName, userLoginEntity.UserName);
        }

        [TestMethod]
        public void CreateProjectCreatingEntityTest()
        {
            //Arrange
            const string producer = "producer";
            const string version = "version";
            const string projectId = "projectId";
            const string actionType = "actionType";
            
            var tableValueConverter = new Mock<ITableValueConverter>();
            var statEntityFactory = new StatEntityFactory(tableValueConverter.Object);

            tableValueConverter.Setup(m => m.DateTimeToTickWithGuid(_dateTime)).Returns(TickWithGuid);
            tableValueConverter.Setup(m => m.UserAgentToProductName(UserAgent)).Returns(producer);
            tableValueConverter.Setup(m => m.UserAgentToVersion(UserAgent)).Returns(version);

            //Act
            var statProjectStateEntity = statEntityFactory.CreateProjectCreatingEntity(_dateTime, _domain, projectId, actionType);

            //Assert
            Assert.AreEqual(projectId, statProjectStateEntity.ProjectId);
            Assert.AreEqual(actionType, statProjectStateEntity.ActionType);
            Assert.AreEqual(_dateTime, statProjectStateEntity.DateTime);
            Assert.AreEqual(producer, statProjectStateEntity.Producer);
            Assert.AreEqual(version, statProjectStateEntity.Version);
        }
    }
}
