using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.Api.Commands.Files;
using Portal.Api.Models;
using Portal.Interface;
using Portal.Repository.Exceptions;
using Portal.Repository.File;

namespace Portal.WebApi.Tests.Controllers
{
    [TestClass]
    public sealed class FilesControllerTest
    {
        private const string FileIdConst = "fileid";
        private const string FileIdNotExistConst = "nofileid";
        private const string UserIdConst = "userid";
        private const string FileNameConst = "My cool file";
        private const string FileUriConst = @"c:\path\to\file.png";
        private const string FileFormatConst = "image/jpeg";
        private string _fileId;
        private Mock<IFileRepository> _repositoryMock;

        private ApiController GetFakeApiController()
        {
            var controllerMock = new Mock<ApiController>();

            var requestMock = new Mock<HttpRequestMessage>();
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

        private IFileRepository GetFakeFileRepository()
        {
            _repositoryMock = new Mock<IFileRepository>();
            _repositoryMock.Setup(m => m.AddAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() =>
                    {
                        var tcs = new TaskCompletionSource<string>();
                        tcs.SetResult(string.Empty);
                        return tcs.Task;
                    });
            _repositoryMock.Setup(m => m.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((userId, fileId) =>
                    {
                        var tcs = new TaskCompletionSource<File>();

                        if (fileId == FileIdNotExistConst)
                        {
                            tcs.SetException(new TableEntityNotFoundException(fileId));
                        }
                        else
                        {
                            tcs.SetResult(new File());
                        }

                        return tcs.Task;
                    });
            _repositoryMock.Setup(m => m.GetListAsync(It.IsAny<string>())).Returns(() =>
                {
                    var tcs = new TaskCompletionSource<List<File>>();
                    tcs.SetResult(new List<File> {new File(), new File(), new File()});
                    return tcs.Task;
                });
            _repositoryMock.Setup(m => m.EditAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string, string>((userid, fileId, name, fileUri, mimeType) => { _fileId = fileId; })
                .Returns(() =>
                    {
                        var tcs = new TaskCompletionSource<int>();
                        if (_fileId == FileIdNotExistConst)
                        {
                            tcs.SetException(new TableEntityNotFoundException(_fileId));
                        }
                        else
                        {
                            tcs.SetResult(0);
                        }

                        return tcs.Task;
                    });
            _repositoryMock.Setup(m => m.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((userId, fileId) => { _fileId = fileId; })
                .Returns(() =>
                    {
                        var tcs = new TaskCompletionSource<int>();
                        if (_fileId == FileIdNotExistConst)
                        {
                            tcs.SetException(new TableEntityNotFoundException(_fileId));
                        }
                        else
                        {
                            tcs.SetResult(0);
                        }

                        return tcs.Task;
                    });
            return _repositoryMock.Object;
        }

        [TestMethod]
        public void TestAddFileCommand()
        {
            // Arrange
            var model = new FilePostModel
                {
                    Name = FileNameConst,
                    FileUri = FileUriConst,
                    Format = FileFormatConst
                };

            var addCommand = new FilesAddCommand(GetFakeApiController(), GetFakeFileRepository(), model);

            // Act
            Task<HttpResponseMessage> addTask = addCommand.Execute();
            addTask.Wait();

            // Assert
            Assert.IsFalse(addTask.IsFaulted);
            Assert.IsNotNull(addTask.Result);
            Assert.AreEqual(addTask.Result.StatusCode, HttpStatusCode.Created);
            Assert.IsInstanceOfType(addTask.Result.Content, typeof (ObjectContent<String>));
            _repositoryMock.Verify(m => m.AddAsync(UserIdConst, FileNameConst, FileUriConst, FileFormatConst), Times.Once());
        }

        [TestMethod]
        public void TestGetFileCommand()
        {
            // Arrange
            var getFilecommand = new FilesGetCommand(GetFakeApiController(), GetFakeFileRepository(), FileIdConst);

            // Act
            Task<File> getFileTask = getFilecommand.Execute();
            getFileTask.Wait();

            // Assert
            Assert.IsFalse(getFileTask.IsFaulted);
            Assert.IsNotNull(getFileTask.Result);
            _repositoryMock.Verify(m => m.GetAsync(UserIdConst, FileIdConst), Times.Once());
        }

        [TestMethod]
        public void TestGetNotExistFileCommand()
        {
            // Arrange
            var getFilecommand = new FilesGetCommand(GetFakeApiController(), GetFakeFileRepository(), FileIdNotExistConst);
            AggregateException exception = null;
            Task<File> getFileTask = null;

            // Act
            try
            {
                getFileTask = getFilecommand.Execute();
                getFileTask.Wait();
            }
            catch (AggregateException e)
            {
                exception = e;
            }

            // Assert
            Assert.IsNotNull(getFileTask);
            Assert.IsTrue(getFileTask.IsFaulted);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception.InnerException, typeof (HttpResponseException));
        }

        [TestMethod]
        public void TestGetListFileCommand()
        {
            // Arrange
            var getFilesListcommand = new FilesGetListCommand(GetFakeApiController(), GetFakeFileRepository());

            // Act
            Task<List<File>> getAllTask = getFilesListcommand.Execute();
            getAllTask.Wait();

            // Assert
            Assert.IsFalse(getAllTask.IsFaulted);
            Assert.IsNotNull(getAllTask.Result);
            Assert.IsTrue(getAllTask.Result.Count == 3);
            _repositoryMock.Verify(m => m.GetListAsync(UserIdConst), Times.Once());
        }

        [TestMethod]
        public void TestEditFileCommand()
        {
            // Arrange
            var editFileCommand = new FilesEditCommand(GetFakeApiController(), GetFakeFileRepository(), FileIdConst, new FilePutModel {Name = FileNameConst});

            // Act
            Task<HttpResponseMessage> editTask = editFileCommand.Execute();
            editTask.Wait();

            // Assert
            Assert.IsFalse(editTask.IsFaulted);
            Assert.IsNotNull(editTask.Result);
            Assert.AreEqual(editTask.Result.StatusCode, HttpStatusCode.OK);
            _repositoryMock.Verify(m => m.EditAsync(UserIdConst, FileIdConst, FileNameConst, null, null), Times.Once());
        }

        [TestMethod]
        public void TestEditNotExistFileCommand()
        {
            // Arrange
            var editFileCommand = new FilesEditCommand(GetFakeApiController(), GetFakeFileRepository(), FileIdNotExistConst, new FilePutModel {Name = FileNameConst});

            // Act
            Task<HttpResponseMessage> editTask = editFileCommand.Execute();
            editTask.Wait();

            // Assert
            Assert.IsFalse(editTask.IsFaulted);
            Assert.IsNotNull(editTask.Result);
            Assert.AreEqual(editTask.Result.StatusCode, HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void TestDeleteFileCommand()
        {
            // Arrange
            var deleteFilecommand = new FilesDeleteCommand(GetFakeApiController(), GetFakeFileRepository(), FileIdConst);

            // Act
            Task<HttpResponseMessage> deleteTask = deleteFilecommand.Execute();
            deleteTask.Wait();

            // Assert
            Assert.IsFalse(deleteTask.IsFaulted);
            Assert.IsNotNull(deleteTask.Result);
            Assert.AreEqual(deleteTask.Result.StatusCode, HttpStatusCode.OK);
            _repositoryMock.Verify(m => m.DeleteAsync(UserIdConst, FileIdConst), Times.Once());
        }

        [TestMethod]
        public void TestDeleteNotExistFileCommand()
        {
            // Arrange
            var deleteFilecommand = new FilesDeleteCommand(GetFakeApiController(), GetFakeFileRepository(), FileIdNotExistConst);

            // Act
            Task<HttpResponseMessage> deleteTask = deleteFilecommand.Execute();
            deleteTask.Wait();

            // Assert
            Assert.IsFalse(deleteTask.IsFaulted);
            Assert.IsNotNull(deleteTask.Result);
            Assert.AreEqual(deleteTask.Result.StatusCode, HttpStatusCode.NotFound);
        }
    }
}