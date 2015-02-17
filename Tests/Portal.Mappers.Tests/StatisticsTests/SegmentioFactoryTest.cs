using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Domain.StatisticContext;
using Portal.Mappers.Statistics;

namespace Portal.Mappers.Tests.StatisticsTests
{
    [TestClass]
    public class SegmentioFactoryTest
    {
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

        private DomainStatUser _statUser;
        private DomainStatProject _statProject;
        private SegmentioFactory _segmentioFactory;

        [TestInitialize]
        public void Initialize()
        {
            _statUser = new DomainStatUser(UserId, UserEmail, UserName, IdentityProvider, ProductName, ProductVersion);
            _statProject = new DomainStatProject(_statUser, ProjectId, ProjectName, ProjectType, ProjectSubtype);
            
            _segmentioFactory = new SegmentioFactory();
        }

        [TestMethod]
        public void CreateTraitsTest()
        {
            //Act
            var traits = _segmentioFactory.CreateTraits(_statUser);

            //Assert
            Assert.AreEqual(2,traits.Count);
            Assert.AreEqual(traits["email"], UserEmail);
            Assert.AreEqual(traits["userName"], UserName);
        }

        [TestMethod]
        public void CreateUserPropertiesTest()
        {
            //Act
            var properties = _segmentioFactory.CreateUserProperties(_statUser);

            //Assert
            Assert.AreEqual(6, properties.Count);
            Assert.AreEqual(properties["userId"], UserId);
            Assert.AreEqual(properties["email"], UserEmail);
            Assert.AreEqual(properties["userName"], UserName);
            Assert.AreEqual(properties["identityProvider"], IdentityProvider);
            Assert.AreEqual(properties["productName"], ProductName);
            Assert.AreEqual(properties["productVersion"], ProductVersion);
        }

        [TestMethod]
        public void CreateProjectPropertiesTest()
        {
            //Act
            var properties = _segmentioFactory.CreateProjectProperties(_statProject);

            //Assert
            Assert.AreEqual(10, properties.Count);
            Assert.AreEqual(properties["userId"], UserId);
            Assert.AreEqual(properties["email"], UserEmail);
            Assert.AreEqual(properties["userName"], UserName);
            Assert.AreEqual(properties["identityProvider"], IdentityProvider);
            Assert.AreEqual(properties["productName"], ProductName);
            Assert.AreEqual(properties["productVersion"], ProductVersion);
            Assert.AreEqual(properties["projectId"], ProjectId);
            Assert.AreEqual(properties["projectName"], ProjectName);
            Assert.AreEqual(properties["projectType"], ProjectType);
            Assert.AreEqual(properties["projectSubtype"], ProjectSubtype);
        }
    }
}
