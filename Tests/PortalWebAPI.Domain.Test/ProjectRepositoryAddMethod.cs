using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using PortalDAL.Entities;
using PortalDAL.Table;
using PortalWebAPI.Repository;

namespace PortalWebAPI.Domain.Test
{
    [TestClass]
    public class ProjectRepositoryAddMethod
    {
        [TestMethod]
        public void TestAddMethod()
        {
            //
            // Functional Test against Azure Emulator
            //

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient();

            InsertStorageConfiguration(tableClient);
            InsertStorageState(tableClient);
            InsertUserAttributes(tableClient);

            var projectRepository = new ProjectRepository(cloudStorageAccount, ProjectRepositoryTestConfig.StorageVersion);
            projectRepository.Add(ProjectRepositoryTestConfig.UserName,
                                  ProjectRepositoryTestConfig.ProjectName,
                                  ProjectRepositoryTestConfig.LocalVideoUri,
                                  ProjectRepositoryTestConfig.LocalDataUri,
                                  ProjectRepositoryTestConfig.VideoFileName,
                                  ProjectRepositoryTestConfig.DataFileName);
        }

        private void InsertStorageConfiguration(CloudTableClient cloudTableClient)
        {
            var storageConfigurationManager = new TableStorageConfigurationManager(cloudTableClient);
            storageConfigurationManager.Insert(new StorageConfiguration
                                                   {
                                                       MaxVideoFileLength = ProjectRepositoryTestConfig.MaxVideoFileLength,
                                                       MaxUserCapacity = ProjectRepositoryTestConfig.MaxUserCapacity,
                                                       MaxStorageCapacity = ProjectRepositoryTestConfig.MaxStorageCapacity
                                                   });
        }

        private void InsertStorageState(CloudTableClient cloudTableClient)
        {
            var storageStateManager = new TableStorageStateManager(cloudTableClient);
            storageStateManager.Insert(new StorageState());
        }

        private void InsertUserAttributes(CloudTableClient cloudTableClient)
        {
            var userAttributesManager = new TableUserAttributesManager(cloudTableClient);
            userAttributesManager.Insert(new UserAttribute(ProjectRepositoryTestConfig.UserName)
                                             {
                                                 MaxStorage = ProjectRepositoryTestConfig.MaxStorage
                                             });
        }
    }
}