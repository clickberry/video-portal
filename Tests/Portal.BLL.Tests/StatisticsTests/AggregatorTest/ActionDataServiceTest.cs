using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Aggregator;
using Portal.DAL.Authentication;
using Portal.DAL.Context;

namespace Portal.BLL.Tests.StatisticsTests.AggregatorTest
{
    [TestClass]
    public class ActionDataServiceTest
    {
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

        const string UserEmail = "userEmail@mail.com";
        const string UserName = "userName";
        const string IdentityProvider = "identityProvider";
        
        readonly string[] _userLanguages = { "en-us", "ru-ru", "bla-bla" };

        private Mock<IHttpContextRepository> _httpContextRepository;
        private Mock<IAuthenticator> _authenticator;

        [TestInitialize]
        public void Initialize()
        {
            _httpContextRepository = new Mock<IHttpContextRepository>();
            _authenticator = new Mock<IAuthenticator>();

            _authenticator.Setup(m => m.GetUserId()).Returns(UserId);
            _authenticator.Setup(m => m.GetAnonymousId()).Returns(AnonymousId);
            _authenticator.Setup(m => m.IsAuthenticated()).Returns(IsAuthenticated);
            _authenticator.Setup(m=>m.GetUserEmail()).Returns(UserEmail);
            _authenticator.Setup(m => m.GetUserName()).Returns(UserName);
            _authenticator.Setup(m => m.GetIdentityProvider()).Returns(IdentityProvider);

            _httpContextRepository.Setup(m => m.GetHttpMethod()).Returns(HttpMethod);
            _httpContextRepository.Setup(m => m.GetUrl()).Returns(Url);
            _httpContextRepository.Setup(m => m.GetUrlReferrer()).Returns(UrlReferrer);
            _httpContextRepository.Setup(m => m.GetUserHostAddress()).Returns(UserHostAddress);
            _httpContextRepository.Setup(m => m.GetUserHostName()).Returns(UserHostName);

            _httpContextRepository.Setup(m => m.GetUserLanguages()).Returns(_userLanguages);
        }

        [TestMethod]
        public void GetActionDataTest()
        {
            //Arragnge
            var actionDataService = new ActionDataService(_httpContextRepository.Object, _authenticator.Object);
            
            //Act
            var domainActionData = actionDataService.GetActionData(UserAgent, StatusCode);

            //Assert
            Assert.AreEqual(HttpMethod, domainActionData.HttpMethod);
            Assert.AreEqual(StatusCode, domainActionData.StatusCode);
            Assert.AreEqual(Url, domainActionData.Url);
            Assert.AreEqual(UrlReferrer, domainActionData.UrlReferrer);
            Assert.AreEqual(UserAgent, domainActionData.UserAgent);
            Assert.AreEqual(UserHostAddress, domainActionData.UserHostAddress);
            Assert.AreEqual(UserHostName, domainActionData.UserHostName);
            Assert.AreEqual(_userLanguages, domainActionData.UserLanguages);

            Assert.AreEqual(UserId, domainActionData.UserId);
            Assert.AreEqual(AnonymousId, domainActionData.AnonymousId);
            Assert.AreEqual(IsAuthenticated, domainActionData.IsAuthenticated);

            Assert.AreEqual(UserEmail,domainActionData.UserEmail);
            Assert.AreEqual(UserName,domainActionData.UserName);
            Assert.AreEqual(IdentityProvider, domainActionData.IdentityProvider);
        }
    }
}
