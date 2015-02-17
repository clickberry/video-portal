using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.Api.Commands.Projects;
using Portal.Api.Models;
using Portal.Interface;
using Portal.Repository.Exceptions;
using Portal.Repository.Project;
using Portal.Repository.Queue;

namespace Portal.WebApi.Tests.Controllers
{
    [TestClass]
    public class ProjectsControllerTest
    {
        private string _dataName;
        private string _dataUri;
        private bool? _isPublicValue;
        private string _projectDescription;
        private string _projectId;
        private string _projectName;
        private Mock<IProjectRepository> _repositoryMock;
        private string _userId;
        private string _videoName;
        private string _videoUri;

        #region Test stuff

        private const string UserIdConst = "guid";
        private const string ProjectIdConst = "777";
        private const string ProjectInvalidIdConst = "invalidId";
        private const string ProjectNameConst = "My project";
        private const string ProjectDescriptionConst = "My description";
        private const string VideoNameConst = "video.mp4";
        private const string DataNameConst = "project.avsx";
        private const string VideoUriConst = @"c:\video.mp4";
        private const string DataUriConst = @"c:\project.avsx";

        private IEnumerable<Project> GetProjectsList()
        {
            return new List<Project>
                {
                    new Project
                        {
                            Id = "1",
                            Name = "Project 1",
                            Created = DateTime.UtcNow,
                            Modified = DateTime.UtcNow,
                            Screenshots = new List<Screenshot>
                                {
                                    new Screenshot {ImageUri = "http://ya.ru/screenshot1.png"},
                                    new Screenshot {ImageUri = "http://ya.ru/screenshot2.png"}
                                },
                            Videos = new List<Video>
                                {
                                    new Video {ContentType = "video/mp4", VideoUri = "http://ya.ru/video.mp4"},
                                    new Video {ContentType = "video/webm", VideoUri = "http://ya.ru/video.webm"}
                                }
                        },
                    new Project
                        {
                            Id = "2",
                            Name = "Project 2",
                            Created = DateTime.UtcNow,
                            Modified = DateTime.UtcNow,
                            Screenshots = new List<Screenshot>
                                {
                                    new Screenshot {ImageUri = "http://ya.ru/screenshot3.png"},
                                    new Screenshot {ImageUri = "http://ya.ru/screenshot4.png"}
                                },
                            Videos = new List<Video>
                                {
                                    new Video {ContentType = "video/mp4", VideoUri = "http://ya.ru/video.mp4"},
                                    new Video {ContentType = "video/webm", VideoUri = "http://ya.ru/video.webm"}
                                }
                        },
                    new Project
                        {
                            Id = "777",
                            Name = "Project 3",
                            Created = DateTime.UtcNow,
                            Modified = DateTime.UtcNow,
                            Screenshots = new List<Screenshot>
                                {
                                    new Screenshot {ImageUri = "http://ya.ru/screenshot4.png"},
                                    new Screenshot {ImageUri = "http://ya.ru/screenshot5.png"}
                                },
                            Videos = new List<Video>
                                {
                                    new Video {ContentType = "video/mp4", VideoUri = "http://ya.ru/video.mp4"},
                                    new Video {ContentType = "video/webm", VideoUri = "http://ya.ru/video.webm"}
                                }
                        }
                };
        }

        private ApiController GetFakeApiController(bool isMultipart = false)
        {
            var controllerMock = new Mock<ApiController>();

            var requestMock = new Mock<HttpRequestMessage>();
            if (isMultipart)
            {
                HttpContent httpContent = new StringContent(string.Empty);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                httpContent.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", "----WebKitFormBoundaryAIv1HQdnxqjt9a5y"));
                httpContent.Headers.ContentLength = 465;

                requestMock.Object.Content = httpContent;
            }

            requestMock.Object.Properties.Add("MS_HttpConfiguration", GetHttpConfiguration());
            controllerMock.Object.Request = requestMock.Object;

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserIdConst), null);

            return controllerMock.Object;
        }

        private HttpConfiguration GetHttpConfiguration()
        {
            var configuration = new HttpConfiguration();
            configuration.Formatters.Add(new XmlMediaTypeFormatter());
            configuration.Formatters.Add(new JsonMediaTypeFormatter());
            configuration.Formatters.Add(new JQueryMvcFormUrlEncodedFormatter());
            configuration.Formatters.Add(new FormUrlEncodedMediaTypeFormatter());

            return configuration;
        }

        private IProjectRepository GetFakeProjectRepository()
        {
            _repositoryMock = new Mock<IProjectRepository>();

            _repositoryMock.Setup(m => m.AddAsync(It.IsAny<ProjectAddParameters>()))
                .Returns(() =>
                    {
                        var tcs = new TaskCompletionSource<Project>();
                        tcs.SetResult(new Project());
                        return tcs.Task;
                    })
                .Callback<ProjectAddParameters>(p =>
                    {
                        _userId = p.UserId;
                        _projectName = p.ProjectName;
                        _projectDescription = p.ProjectDescription;
                        _videoUri = p.LocalVideoUri;
                        _dataUri = p.LocalDataUri;
                        _videoName = p.VideoFileName;
                        _dataName = p.DataFileName;
                        _isPublicValue = p.IsPublic;
                    });

            _repositoryMock.Setup(m => m.GetAsync(It.IsAny<ProjectGetParameters>()))
                .Callback<ProjectGetParameters>(p =>
                    {
                        _userId = p.UserId;
                        _projectId = p.ProjectId;
                    })
                .Returns((ProjectGetParameters p) =>
                    {
                        var tcs = new TaskCompletionSource<Project>();

                        List<Project> projects = GetProjectsList().ToList();

                        if (!projects.Any(q => q.Id == _projectId))
                        {
                            tcs.SetException(new TableEntityNotFoundException(_projectId));
                        }
                        else
                        {
                            tcs.SetResult(GetProjectsList().FirstOrDefault(q => q.Id == p.ProjectId));
                        }

                        return tcs.Task;
                    });

            _repositoryMock.Setup(m => m.GetListAsync(It.IsAny<ProjectGetListParameters>()))
                .Returns(() =>
                    {
                        var tcs = new TaskCompletionSource<List<Project>>();
                        tcs.SetResult(GetProjectsList().ToList());
                        return tcs.Task;
                    })
                .Callback<ProjectGetListParameters>(p => _userId = p.UserId);

            _repositoryMock.Setup(m => m.EditAsync(It.IsAny<ProjectEditParameters>()))
                .Callback<ProjectEditParameters>(
                    p =>
                        {
                            _userId = p.UserId;
                            _projectId = p.ProjectId;
                            _projectName = p.ProjectName;
                            _projectDescription = p.ProjectDescription;
                            _videoUri = p.LocalVideoUri;
                            _dataUri = p.LocalDataUri;
                            _videoName = p.VideoFileName;
                            _dataName = p.DataFileName;
                        }).Returns(() =>
                            {
                                var tcs = new TaskCompletionSource<Project>();

                                List<Project> projects = GetProjectsList().ToList();

                                if (!projects.Any(p => p.Id == _projectId))
                                {
                                    tcs.SetException(new TableEntityNotFoundException(_projectId));
                                }
                                else
                                {
                                    tcs.SetResult(new Project());
                                }
                                return tcs.Task;
                            });

            _repositoryMock.Setup(m => m.DeleteAsync(It.IsAny<ProjectDeleteParameters>()))
                .Callback<ProjectDeleteParameters>(
                    p =>
                        {
                            _userId = p.UserId;
                            _projectId = p.ProjectId;
                        }).Returns(() =>
                            {
                                var tcs = new TaskCompletionSource<Project>();
                                List<Project> projects = GetProjectsList().ToList();

                                if (!projects.Any(p => p.Id == _projectId))
                                {
                                    tcs.SetException(new TableEntityNotFoundException(_projectId));
                                }
                                else
                                {
                                    tcs.SetResult(new Project());
                                }

                                return tcs.Task;
                            });

            return _repositoryMock.Object;
        }

        #endregion

        #region Add method

        [TestMethod]
        public void TestAddProjectCommand()
        {
            // Arrange
            bool isFaulted = false;
            HttpResponseMessage response = null;

            var project = new ProjectPostModel
                {
                    Name = ProjectNameConst,
                    Description = ProjectDescriptionConst,
                    Data = DataNameConst,
                    DataUri = DataUriConst,
                    Video = VideoNameConst,
                    VideoUri = VideoUriConst,
                    Public = "true"
                };

            var projectsAddCommand = new ProjectsAddCommand(GetFakeApiController(true), GetFakeProjectRepository(), GetFakeQueueVideoRepository(), project);

            // Act
            Task<HttpResponseMessage> responseTask = projectsAddCommand.Execute();
            Task<bool> taskResult = responseTask.ContinueWith(p => isFaulted = p.IsFaulted, TaskContinuationOptions.ExecuteSynchronously);
            taskResult.Wait();

            if (!isFaulted)
            {
                response = responseTask.Result;
            }

            // Assert
            _repositoryMock.Verify(m => m.AddAsync(It.IsAny<ProjectAddParameters>()), Times.Once());
            Assert.IsFalse(isFaulted);
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Assert.AreEqual(UserIdConst, _userId);
            Assert.AreEqual(ProjectNameConst, _projectName);
            Assert.AreEqual(ProjectDescriptionConst, _projectDescription);
            Assert.AreEqual(VideoNameConst, _videoName);
            Assert.AreEqual(VideoUriConst, _videoUri);
            Assert.AreEqual(DataNameConst, _dataName);
            Assert.AreEqual(DataUriConst, _dataUri);
            Assert.AreEqual(_isPublicValue, true);
        }

        #endregion

        #region Get method

        [TestMethod]
        public void TestGetProjectCommand()
        {
            // Arrange
            var projectsGetCommand = new ProjectsGetCommand(GetFakeApiController(), GetFakeProjectRepository(), ProjectIdConst);

            // Act
            Task<Project> getTask = projectsGetCommand.Execute();
            getTask.Wait();

            // Assert
            _repositoryMock.Verify(m => m.GetAsync(It.IsAny<ProjectGetParameters>()), Times.Once());
            Assert.IsFalse(getTask.IsFaulted);
            Assert.IsNotNull(getTask.Result);
            Assert.AreEqual(UserIdConst, _userId);
            Assert.AreEqual(ProjectIdConst, getTask.Result.Id);
            Assert.AreEqual(ProjectIdConst, _projectId);
        }

        [TestMethod]
        public void TestGetProjectCommandByEmptyId()
        {
            // Arrange
            const string id = "";

            Task<Project> getTask = null;
            AggregateException exception = null;
            var projectsGetCommand = new ProjectsGetCommand(GetFakeApiController(), GetFakeProjectRepository(), id);

            // Act
            try
            {
                getTask = projectsGetCommand.Execute();
                getTask.Wait();
            }
            catch (AggregateException e)
            {
                exception = e;
            }

            // Assert
            _repositoryMock.Verify(m => m.GetAsync(It.IsAny<ProjectGetParameters>()), Times.Never());
            Assert.IsNotNull(getTask);
            Assert.IsTrue(getTask.IsFaulted);
            Assert.IsNotNull(exception);
            Assert.AreEqual(((HttpResponseException) exception.InnerException).Response.StatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void TestGetProjectCommandByInvalidId()
        {
            // Arrange
            Task<Project> getTask = null;
            AggregateException exception = null;

            var projectsGetCommand = new ProjectsGetCommand(GetFakeApiController(), GetFakeProjectRepository(), ProjectInvalidIdConst);

            // Act
            try
            {
                getTask = projectsGetCommand.Execute();
                getTask.Wait();
            }
            catch (AggregateException e)
            {
                exception = e;
            }

            // Assert
            _repositoryMock.Verify(m => m.GetAsync(It.IsAny<ProjectGetParameters>()), Times.Once());
            Assert.IsNotNull(getTask);
            Assert.IsTrue(getTask.IsFaulted);
            Assert.AreEqual(_userId, UserIdConst);
            Assert.IsNotNull(exception);
            Assert.AreEqual(((HttpResponseException) exception.InnerException).Response.StatusCode, HttpStatusCode.NotFound);
            Assert.AreEqual(ProjectInvalidIdConst, _projectId);
        }

        #endregion

        #region Edit method

        [TestMethod]
        public void TestEditProjectCommand()
        {
            // Arrange
            var project = new ProjectPutModel
                {
                    Name = ProjectNameConst,
                    Description = ProjectDescriptionConst,
                    Data = DataNameConst,
                    DataUri = DataUriConst,
                    Video = VideoNameConst,
                    VideoUri = VideoUriConst
                };

            var projectsEditCommand = new ProjectsEditCommand(GetFakeApiController(), GetFakeProjectRepository(), GetFakeQueueVideoRepository(), ProjectIdConst, project);

            // Act
            Task<HttpResponseMessage> responseTask = projectsEditCommand.Execute();
            responseTask.Wait();

            HttpResponseMessage response = responseTask.Result;

            // Assert
            _repositoryMock.Verify(m => m.EditAsync(It.IsAny<ProjectEditParameters>()), Times.Once());
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(UserIdConst, _userId);
            Assert.AreEqual(_projectName, ProjectNameConst);
            Assert.AreEqual(_projectDescription, ProjectDescriptionConst);
            Assert.AreEqual(_projectId, ProjectIdConst);
            Assert.AreEqual(_videoName, VideoNameConst);
            Assert.AreEqual(_videoUri, VideoUriConst);
            Assert.AreEqual(_dataName, DataNameConst);
            Assert.AreEqual(_dataUri, DataUriConst);
            Assert.IsNull(_isPublicValue);
        }

        private IQueueVideoRepository GetFakeQueueVideoRepository()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TestEditProjectCommandWithInvalidId()
        {
            // Arrange
            bool isFaulted = false;

            var project = new ProjectPutModel
                {
                    Name = ProjectNameConst,
                    Description = ProjectDescriptionConst
                };

            var projectsEditCommand = new ProjectsEditCommand(GetFakeApiController(), GetFakeProjectRepository(), GetFakeQueueVideoRepository(), ProjectInvalidIdConst, project);

            // Act
            Task<HttpResponseMessage> responseTask = projectsEditCommand.Execute();
            Task<bool> result = responseTask.ContinueWith(p => isFaulted = p.IsFaulted, TaskContinuationOptions.ExecuteSynchronously);
            result.Wait();

            // Assert
            Assert.IsFalse(isFaulted);
            Assert.IsNotNull(responseTask.Result);
            Assert.AreEqual(responseTask.Result.StatusCode, HttpStatusCode.NotFound);
            _repositoryMock.Verify(m => m.EditAsync(It.IsAny<ProjectEditParameters>()), Times.Once());
            Assert.AreEqual(UserIdConst, _userId);
            Assert.AreEqual(_projectName, ProjectNameConst);
            Assert.AreEqual(_projectDescription, ProjectDescriptionConst);
            Assert.AreEqual(_projectId, ProjectInvalidIdConst);
        }

        #endregion

        #region Delete mothod

        [TestMethod]
        public void TestDeleteProjectById()
        {
            // Arrange
            var projectsDeleteCommand = new ProjectsDeleteCommand(GetFakeApiController(), GetFakeProjectRepository(), GetFakeQueueVideoRepository(), ProjectIdConst);

            // Act
            Task<HttpResponseMessage> deleteTask = projectsDeleteCommand.Execute();
            deleteTask.Wait();

            // Assert
            _repositoryMock.Verify(m => m.DeleteAsync(It.IsAny<ProjectDeleteParameters>()), Times.Once());
            Assert.IsFalse(deleteTask.IsFaulted);
            Assert.IsNotNull(deleteTask.Result);
            Assert.AreEqual(deleteTask.Result.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(UserIdConst, _userId);
            Assert.AreEqual(ProjectIdConst, _projectId);
        }

        [TestMethod]
        public void TestDeleteProjectCommandByEmptyId()
        {
            // Arrange
            const string id = "";
            var projectsDeleteCommand = new ProjectsDeleteCommand(GetFakeApiController(), GetFakeProjectRepository(), GetFakeQueueVideoRepository(), id);

            // Act
            Task<HttpResponseMessage> deleteTask = projectsDeleteCommand.Execute();
            deleteTask.Wait();

            // Assert
            _repositoryMock.Verify(m => m.DeleteAsync(It.IsAny<ProjectDeleteParameters>()), Times.Never());
            Assert.IsFalse(deleteTask.IsFaulted);
            Assert.IsNotNull(deleteTask.Result);
            Assert.AreEqual(deleteTask.Result.StatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void TestDeleteProjectCommandByInvalidId()
        {
            // Arrange
            var projectsDeleteCommand = new ProjectsDeleteCommand(GetFakeApiController(), GetFakeProjectRepository(), GetFakeQueueVideoRepository(), ProjectInvalidIdConst);

            // Act
            Task<HttpResponseMessage> deleteTask = projectsDeleteCommand.Execute();
            deleteTask.Wait();

            // Assert
            _repositoryMock.Verify(m => m.DeleteAsync(It.IsAny<ProjectDeleteParameters>()), Times.Once());
            Assert.IsFalse(deleteTask.IsFaulted);
            Assert.IsNotNull(deleteTask.Result);
            Assert.AreEqual(deleteTask.Result.StatusCode, HttpStatusCode.NotFound);
            Assert.AreEqual(ProjectInvalidIdConst, _projectId);
            Assert.AreEqual(UserIdConst, _userId);
        }

        #endregion

        #region GetList Method

        [TestMethod]
        public void TestGetProjects()
        {
            // Arrange
            var projectsGetListCommand = new ProjectsGetListCommand(GetFakeApiController(), GetFakeProjectRepository());

            // Act
            Task<IQueryable<Project>> getAllTask = projectsGetListCommand.Execute();
            getAllTask.Wait();

            // Assert
            _repositoryMock.Verify(m => m.GetListAsync(It.IsAny<ProjectGetListParameters>()), Times.Once());
            Assert.IsFalse(getAllTask.IsFaulted);
            Assert.IsNotNull(getAllTask.Result);
            Assert.AreEqual(GetProjectsList().Count(), getAllTask.Result.Count());
            Assert.AreEqual(UserIdConst, _userId);
        }

        #endregion
    }
}