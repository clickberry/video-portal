//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Microsoft.WindowsAzure;
//using Microsoft.WindowsAzure.StorageClient;
//using Portal.BLL.Parameters;
//using Portal.DAL.Azure.Context;
//using Portal.DAL.Azure.Entities;
//using Portal.DAL.Context;
//using Portal.Repository.Azure.Table;
//using Portal.Repository.Context;

//namespace Portal.Repository.Tests
//{
//    [TestClass]
//    public class UserProjectRepositoryTests
//    {
//        [TestMethod]
//        public void TestAdd1()
//        {
//            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
//            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
//            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

//            var entityContextInitializer = new TableEntityContextInitializer(cloudTableClient, UserProject.EntitySetName);
//            entityContextInitializer.Initialize();

//            IUserProjectRepository userProjectRepository = new UserProjectTableRepository
//                {
//                    EntityCommandContext = new TableEntityCommandContext<UserProject>
//                        {
//                            DataContext = new DataContext
//                                {
//                                    StorageContext = new StorageContext
//                                        {
//                                            CloudBlobClient = cloudBlobClient,
//                                            DefaultBlobName = "file"
//                                        },
//                                    TableContext = new TableContext(UserProject.EntitySetName)
//                                        {
//                                            TableServiceContext = new TableServiceContext(cloudTableClient.BaseUri.AbsoluteUri, cloudStorageAccount.Credentials)
//                                        }
//                                }
//                        },
//                    ParametersToTableEntity = p => new UserProject
//                        {
//                            ProjectId = p.ProjectId,
//                            ModifiedDate = p.ModifiedDate,
//                            CreateDate = p.CreateDate,
//                            UserId = p.UserId,
//                            Description = p.Description,
//                            IsBlocked = p.IsBlocked,
//                            IsPublic = p.IsPublic,
//                            Name = p.Name
//                        },
//                    TableEntityToParameters = p => new UserProjectParameters
//                        {
//                            ProjectId = p.ProjectId,
//                            ModifiedDate = p.ModifiedDate,
//                            CreateDate = p.CreateDate,
//                            UserId = p.UserId,
//                            Description = p.Description,
//                            IsBlocked = p.IsBlocked,
//                            IsPublic = p.IsPublic,
//                            Name = p.Name
//                        },
//                    Expression = p => q => q.ProjectId == p.ProjectId,
//                    QueryExpression = p => q => q.UserId == p.UserId
//                };

//            var projectParameters = new UserProjectParameters
//                {
//                    CreateDate = DateTime.UtcNow,
//                    Description = "bla-bla",
//                    IsBlocked = false,
//                    IsPublic = true,
//                    ModifiedDate = DateTime.UtcNow,
//                    Name = "The One",
//                    ProjectId = "fc7f9b14-c04e-4bdc-9101-3fa366854ce0",
//                    UserId = "7d880a24-6792-4fef-8692-23126405425b"
//                };

//            Task<UserProjectParameters> task = userProjectRepository.AddAsync(projectParameters);
//            task.Wait();

//            UserProjectParameters result = task.Result;

//            Assert.IsNotNull(result);
//        }

//        [TestMethod]
//        public void TestAdd2()
//        {
//            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
//            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
//            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

//            var entityContextInitializer = new TableEntityContextInitializer(cloudTableClient, UserProject.EntitySetName);
//            entityContextInitializer.Initialize();

//            IUserProjectRepository userProjectRepository = new UserProjectTableRepository
//                {
//                    EntityCommandContext = new TableEntityCommandContext<UserProject>
//                        {
//                            DataContext = new DataContext
//                                {
//                                    StorageContext = new StorageContext
//                                        {
//                                            CloudBlobClient = cloudBlobClient,
//                                            DefaultBlobName = "file"
//                                        },
//                                    TableContext = new TableContext(UserProject.EntitySetName)
//                                        {
//                                            TableServiceContext = new TableServiceContext(cloudTableClient.BaseUri.AbsoluteUri, cloudStorageAccount.Credentials)
//                                        }
//                                }
//                        },
//                    ParametersToTableEntity = p => new UserProject
//                        {
//                            ProjectId = p.ProjectId,
//                            ModifiedDate = p.ModifiedDate,
//                            CreateDate = p.CreateDate,
//                            UserId = p.UserId,
//                            Description = p.Description,
//                            IsBlocked = p.IsBlocked,
//                            IsPublic = p.IsPublic,
//                            Name = p.Name
//                        },
//                    TableEntityToParameters = p => new UserProjectParameters
//                        {
//                            ProjectId = p.ProjectId,
//                            ModifiedDate = p.ModifiedDate,
//                            CreateDate = p.CreateDate,
//                            UserId = p.UserId,
//                            Description = p.Description,
//                            IsBlocked = p.IsBlocked,
//                            IsPublic = p.IsPublic,
//                            Name = p.Name
//                        },
//                    Expression = p => q => q.ProjectId == p.ProjectId,
//                    QueryExpression = p => q => q.UserId == p.UserId
//                };

//            var projectParameters = new UserProjectParameters
//                {
//                    CreateDate = DateTime.UtcNow,
//                    Description = @"C# is a multi-paradigm programming language...",
//                    IsBlocked = false,
//                    IsPublic = true,
//                    ModifiedDate = DateTime.UtcNow,
//                    Name = "C#",
//                    ProjectId = "15a38eda-3bb8-45ae-ab0b-5bcb2571fa1c",
//                    UserId = "7d880a24-6792-4fef-8692-23126405425b"
//                };

//            Task<UserProjectParameters> task = userProjectRepository.AddAsync(projectParameters);
//            task.Wait();

//            UserProjectParameters result = task.Result;

//            Assert.IsNotNull(result);
//        }

//        [TestMethod]
//        public void TestGet()
//        {
//            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
//            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
//            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

//            var entityContextInitializer = new TableEntityContextInitializer(cloudTableClient, UserProject.EntitySetName);
//            entityContextInitializer.Initialize();

//            IUserProjectRepository userProjectRepository = new UserProjectTableRepository
//                {
//                    EntityCommandContext = new TableEntityCommandContext<UserProject>
//                        {
//                            DataContext = new DataContext
//                                {
//                                    StorageContext = new StorageContext
//                                        {
//                                            CloudBlobClient = cloudBlobClient,
//                                            DefaultBlobName = "file"
//                                        },
//                                    TableContext = new TableContext(UserProject.EntitySetName)
//                                        {
//                                            TableServiceContext = new TableServiceContext(cloudTableClient.BaseUri.AbsoluteUri, cloudStorageAccount.Credentials)
//                                        }
//                                }
//                        },
//                    ParametersToTableEntity = p => new UserProject
//                        {
//                            ProjectId = p.ProjectId,
//                            ModifiedDate = p.ModifiedDate,
//                            CreateDate = p.CreateDate,
//                            UserId = p.UserId,
//                            Description = p.Description,
//                            IsBlocked = p.IsBlocked,
//                            IsPublic = p.IsPublic,
//                            Name = p.Name
//                        },
//                    TableEntityToParameters = p => new UserProjectParameters
//                        {
//                            ProjectId = p.ProjectId,
//                            ModifiedDate = p.ModifiedDate,
//                            CreateDate = p.CreateDate,
//                            UserId = p.UserId,
//                            Description = p.Description,
//                            IsBlocked = p.IsBlocked,
//                            IsPublic = p.IsPublic,
//                            Name = p.Name
//                        },
//                    Expression = p => q => q.ProjectId == p.ProjectId,
//                    QueryExpression = p => q => q.UserId == p.UserId
//                };

//            var projectParameters = new UserProjectParameters
//                {
//                    ProjectId = "fc7f9b14-c04e-4bdc-9101-3fa366854ce0",
//                    UserId = "7d880a24-6792-4fef-8692-23126405425b"
//                };

//            Task<UserProjectParameters> task = userProjectRepository.GetAsync(projectParameters);
//            task.Wait();

//            UserProjectParameters result = task.Result;

//            Assert.IsNotNull(result);
//        }

//        [TestMethod]
//        public void TestGetCollection()
//        {
//            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
//            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
//            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

//            var entityContextInitializer = new TableEntityContextInitializer(cloudTableClient, UserProject.EntitySetName);
//            entityContextInitializer.Initialize();

//            IUserProjectRepository userProjectRepository = new UserProjectTableRepository
//                {
//                    EntityCommandContext = new TableEntityCommandContext<UserProject>
//                        {
//                            DataContext = new DataContext
//                                {
//                                    StorageContext = new StorageContext
//                                        {
//                                            CloudBlobClient = cloudBlobClient,
//                                            DefaultBlobName = "file"
//                                        },
//                                    TableContext = new TableContext(UserProject.EntitySetName)
//                                        {
//                                            TableServiceContext = new TableServiceContext(cloudTableClient.BaseUri.AbsoluteUri, cloudStorageAccount.Credentials)
//                                        }
//                                }
//                        },
//                    ParametersToTableEntity = p => new UserProject
//                        {
//                            ProjectId = p.ProjectId,
//                            ModifiedDate = p.ModifiedDate,
//                            CreateDate = p.CreateDate,
//                            UserId = p.UserId,
//                            Description = p.Description,
//                            IsBlocked = p.IsBlocked,
//                            IsPublic = p.IsPublic,
//                            Name = p.Name
//                        },
//                    TableEntityToParameters = p => new UserProjectParameters
//                        {
//                            ProjectId = p.ProjectId,
//                            ModifiedDate = p.ModifiedDate,
//                            CreateDate = p.CreateDate,
//                            UserId = p.UserId,
//                            Description = p.Description,
//                            IsBlocked = p.IsBlocked,
//                            IsPublic = p.IsPublic,
//                            Name = p.Name
//                        },
//                    Expression = p => q => q.ProjectId == p.ProjectId,
//                    QueryExpression = p => q => q.UserId == p.UserId
//                };

//            var projectParameters = new UserProjectParameters
//                {
//                    UserId = "7d880a24-6792-4fef-8692-23126405425b"
//                };

//            Task<List<UserProjectParameters>> task = userProjectRepository.GetListAsync(projectParameters);
//            task.Wait();

//            List<UserProjectParameters> result = task.Result;

//            Assert.IsNotNull(result);
//        }

//        [TestMethod]
//        public void TestDelete1()
//        {
//            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
//            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
//            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

//            var entityContextInitializer = new TableEntityContextInitializer(cloudTableClient, UserProject.EntitySetName);
//            entityContextInitializer.Initialize();

//            IUserProjectRepository userProjectRepository = new UserProjectTableRepository
//                {
//                    EntityCommandContext = new TableEntityCommandContext<UserProject>
//                        {
//                            DataContext = new DataContext
//                                {
//                                    StorageContext = new StorageContext
//                                        {
//                                            CloudBlobClient = cloudBlobClient,
//                                            DefaultBlobName = "file"
//                                        },
//                                    TableContext = new TableContext(UserProject.EntitySetName)
//                                        {
//                                            TableServiceContext = new TableServiceContext(cloudTableClient.BaseUri.AbsoluteUri, cloudStorageAccount.Credentials)
//                                        }
//                                }
//                        },
//                    ParametersToTableEntity = p => new UserProject
//                        {
//                            ProjectId = p.ProjectId,
//                            ModifiedDate = p.ModifiedDate,
//                            CreateDate = p.CreateDate,
//                            UserId = p.UserId,
//                            Description = p.Description,
//                            IsBlocked = p.IsBlocked,
//                            IsPublic = p.IsPublic,
//                            Name = p.Name
//                        },
//                    TableEntityToParameters = p => new UserProjectParameters
//                        {
//                            ProjectId = p.ProjectId,
//                            ModifiedDate = p.ModifiedDate,
//                            CreateDate = p.CreateDate,
//                            UserId = p.UserId,
//                            Description = p.Description,
//                            IsBlocked = p.IsBlocked,
//                            IsPublic = p.IsPublic,
//                            Name = p.Name
//                        },
//                    Expression = p => q => q.ProjectId == p.ProjectId,
//                    QueryExpression = p => q => q.UserId == p.UserId
//                };

//            var projectParameters = new UserProjectParameters
//                {
//                    ProjectId = "fc7f9b14-c04e-4bdc-9101-3fa366854ce0",
//                    UserId = "7d880a24-6792-4fef-8692-23126405425b"
//                };

//            Task task = userProjectRepository.DeleteAsync(projectParameters);
//            task.Wait();
//        }

//        [TestMethod]
//        public void TestDelete2()
//        {
//            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
//            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
//            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

//            var entityContextInitializer = new TableEntityContextInitializer(cloudTableClient, UserProject.EntitySetName);
//            entityContextInitializer.Initialize();

//            IUserProjectRepository userProjectRepository = new UserProjectTableRepository
//                {
//                    EntityCommandContext = new TableEntityCommandContext<UserProject>
//                        {
//                            DataContext = new DataContext
//                                {
//                                    StorageContext = new StorageContext
//                                        {
//                                            CloudBlobClient = cloudBlobClient,
//                                            DefaultBlobName = "file"
//                                        },
//                                    TableContext = new TableContext(UserProject.EntitySetName)
//                                        {
//                                            TableServiceContext = new TableServiceContext(cloudTableClient.BaseUri.AbsoluteUri, cloudStorageAccount.Credentials)
//                                        }
//                                }
//                        },
//                    ParametersToTableEntity = p => new UserProject
//                        {
//                            ProjectId = p.ProjectId,
//                            ModifiedDate = p.ModifiedDate,
//                            CreateDate = p.CreateDate,
//                            UserId = p.UserId,
//                            Description = p.Description,
//                            IsBlocked = p.IsBlocked,
//                            IsPublic = p.IsPublic,
//                            Name = p.Name
//                        },
//                    TableEntityToParameters = p => new UserProjectParameters
//                        {
//                            ProjectId = p.ProjectId,
//                            ModifiedDate = p.ModifiedDate,
//                            CreateDate = p.CreateDate,
//                            UserId = p.UserId,
//                            Description = p.Description,
//                            IsBlocked = p.IsBlocked,
//                            IsPublic = p.IsPublic,
//                            Name = p.Name
//                        },
//                    Expression = p => q => q.ProjectId == p.ProjectId,
//                    QueryExpression = p => q => q.UserId == p.UserId
//                };

//            var projectParameters = new UserProjectParameters
//                {
//                    ProjectId = "15a38eda-3bb8-45ae-ab0b-5bcb2571fa1c",
//                    UserId = "7d880a24-6792-4fef-8692-23126405425b"
//                };

//            Task task = userProjectRepository.DeleteAsync(projectParameters);
//            task.Wait();
//        }
//    }
//}