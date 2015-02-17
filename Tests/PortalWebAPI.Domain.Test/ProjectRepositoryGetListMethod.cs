using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure;
using PortalWebAPI.Entities;
using PortalWebAPI.Repository;

namespace PortalWebAPI.Domain.Test
{
    [TestClass]
    public class ProjectRepositoryGetListMethod
    {
        [TestMethod]
        public void TestGetListMethod()
        {
            //
            // Functional Test against Azure Emulator
            //

            var projectRepository = new ProjectRepository(CloudStorageAccount.DevelopmentStorageAccount, ProjectRepositoryTestConfig.StorageVersion);
            List<Project> projectList = projectRepository.GetList(ProjectRepositoryTestConfig.UserName);
        }
    }
}