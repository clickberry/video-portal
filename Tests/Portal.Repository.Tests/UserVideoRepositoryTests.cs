//using System;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Microsoft.WindowsAzure;
//using Microsoft.WindowsAzure.StorageClient;
//using Portal.BLL.Parameters;
//using Portal.DAL.Azure.Context;
//using Portal.DAL.Azure.Entities;
//using Portal.Repository.Azure.Context;
//using Portal.Repository.Context;

//namespace Portal.Repository.Tests
//{
//    [TestClass]
//    public class UserVideoRepositoryTests
//    {
//        [TestMethod]
//        public void TestAdd1()
//        {
//            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
//            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
//            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

//            var entityContextInitializer = new TableEntityContextInitializer(cloudTableClient, ProjectVideo.EntitySetName);
//            entityContextInitializer.Initialize();

//            IProjectVideoRepository projectVideoRepository = new ProjectVideoRepository
//                {
//                };

//            const string videoUri = @"video.flv";

//            var projectVideoParameters = new ProjectVideoParameters
//                {
//                    ProjectId = "fc7f9b14-c04e-4bdc-9101-3fa366854ce0",
//                    CreateDate = DateTime.UtcNow,
//                    ModifiedDate = DateTime.UtcNow,
//                    FileUri = videoUri
//                };

//            Task<ProjectVideoParameters> task = projectVideoRepository.AddAsync(projectVideoParameters);
//            task.Wait();

//            ProjectVideoParameters result = task.Result;

//            Assert.IsNotNull(result);
//        }
//    }
//}