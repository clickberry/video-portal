using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure;
using PortalWebAPI.Entities;
using PortalWebAPI.Repository;

namespace PortalWebAPI.Domain.Test
{
    [TestClass]
    public class ProjectRepositoryGetMethod
    {
        [TestMethod]
        public void TestGetMethod()
        {
            //
            // Functional Test against Azure Emulator
            //

            var projectRepository = new ProjectRepository(CloudStorageAccount.DevelopmentStorageAccount,ProjectRepositoryTestConfig.StorageVersion);
            Project project = projectRepository.Get(ProjectRepositoryTestConfig.UserName, ProjectRepositoryTestConfig.ProjectId);
        }
    }
}