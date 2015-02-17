using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.Domain.StatisticContext;
using Portal.Mappers.Statistics;
using TestFake;

namespace Portal.Mappers.Tests.StatisticsTests
{
    [TestClass]
    public class StatDomainFactoryTest
    {
        const string UserAgent = "userAgent";
        const string UserId = "userId";
        const string UserEmail = "userEmail";
        const string UserName = "userName";
        const string IdentityProvider = "identityProvider";
        const string ProductName = "productName";
        const string ProductVersion = "productVersion";
        const string ProjectId = "projectId";
        const string ProjectName = "projectName";
        const string ProjectType = "projectType";
        const string ProjectSubtype = "projectSubtype";

        private DomainActionData _domain;
        private Mock<ITableValueConverter> _tableValueConverter;

        [TestInitialize]
        public void Initialize()
        {
            _domain = new DomainActionData()
            {
                UserAgent = UserAgent,
                UserId = UserId,
                UserEmail = UserEmail,
                UserName = UserName,
                IdentityProvider = IdentityProvider
            };

            _tableValueConverter = new Mock<ITableValueConverter>();
            _tableValueConverter.Setup(m => m.UserAgentToProductName(UserAgent)).Returns(ProductName);
            _tableValueConverter.Setup(m => m.UserAgentToVersion(UserAgent)).Returns(ProductVersion);
        }

        [TestMethod]
        public void CreateUserTest()
        {
            //Arrange
            var statDomainFactory = new StatDomainFactory(_tableValueConverter.Object);
            
            //Act
            var user = statDomainFactory.CreateUser(_domain);

            //Assert
            Assert.AreEqual(UserId, user.UserId);
            Assert.AreEqual(UserEmail, user.Email);
            Assert.AreEqual(UserName, user.UserName);
            Assert.AreEqual(IdentityProvider, user.IdentityProvider);
            Assert.AreEqual(ProductName, user.ProductName);
            Assert.AreEqual(ProductVersion, user.ProductVersion);
        }

        [TestMethod]
        public void CreateProjectTest()
        {
            //Arrange
            var statDomainFactory = new StatDomainFactory(_tableValueConverter.Object);

            //Act
            var project = statDomainFactory.CreateProject(_domain, ProjectId, ProjectName, ProjectType, ProjectSubtype);

            //Assert
            Assert.AreEqual(ProjectId, project.ProjectId);
            Assert.AreEqual(ProjectName, project.ProjectName);
            Assert.AreEqual(ProjectType, project.ProjectType);
            Assert.AreEqual(ProjectSubtype, project.ProjectSubtype);
            Assert.AreEqual(UserId, project.User.UserId);
            Assert.AreEqual(UserEmail, project.User.Email);
            Assert.AreEqual(UserName, project.User.UserName);
            Assert.AreEqual(IdentityProvider, project.User.IdentityProvider);
            Assert.AreEqual(ProductName, project.User.ProductName);
            Assert.AreEqual(ProductVersion, project.User.ProductVersion);
        }
    }
}
