using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Domain.StatisticContext;
using TestFake;

namespace Portal.Domain.Tests.StatisticsTests
{
    [TestClass]
    public class DomainStatProjectTest
    {
        [TestMethod]
        public void CreateUserTest()
        {
            //Arrange
            const string projectId = "projectId";
            const string projectName = "projectName";
            const string projectType = "projectType";
            const string projectSubtype = "projectSubtype";

            var user = new FakeDomainStatUser();

            //Act
            var project = new DomainStatProject(user, projectId, projectName, projectType, projectSubtype);

            //Assert
            Assert.AreEqual(user, project.User);
            Assert.AreEqual(projectId, project.ProjectId);
            Assert.AreEqual(projectName, project.ProjectName);
            Assert.AreEqual(projectType, project.ProjectType);
            Assert.AreEqual(projectSubtype, project.ProjectSubtype);
        }
    }
}