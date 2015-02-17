using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Portal.BackEnd.Encoder.Pipeline.Step;
using Portal.BackEnd.Encoder.Settings;
using Portal.DAL.Entities.Storage;
using Portal.DAL.FileSystem;
using Portal.Domain.BackendContext.Enum;
using Wrappers;
using Wrappers.Interface;

namespace Portal.BackEnd.Encoder.Test.PipelineTests
{
    [TestClass]
    public class UploadStepTest
    {
        const string UserId = "userId";
        const string LocalFileUri = "localFileUri";
        const string ContentType = "contentType";
        const string FileHash = "fileHash";

        private Mock<IStepMediator> _pipelineMediator;
        private Mock<IEncodeWebClient> _webClient;
        private Mock<IFileSystem> _fileSystem;
        private Mock<ITempFileManager> _tempFileManager;
        private UploadStep _pipelineStep;
        private Mock<CancellationTokenSourceWrapper> _tokenSource;
        private Mock<IFileWrapper> _fileWrapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new UploadSettings(UserId);
            var stepData = new EncodeStepData()
            {
                EncoderState = EncoderState.Completed,
                ContentType = ContentType
            };

            _pipelineMediator = new Mock<IStepMediator>();
            _webClient = new Mock<IEncodeWebClient>();
            _fileSystem = new Mock<IFileSystem>();
            _tempFileManager = new Mock<ITempFileManager>();
            _fileWrapper = new Mock<IFileWrapper>();
            _tokenSource = new Mock<CancellationTokenSourceWrapper>();

            _pipelineStep = new UploadStep(_pipelineMediator.Object, _webClient.Object, _tempFileManager.Object, settings, _fileSystem.Object, _fileWrapper.Object);

            _tempFileManager.Setup(m => m.GetEncodingTempFilePath()).Returns(LocalFileUri);
            
            _pipelineStep.SetData(stepData);
        }

        [TestMethod]
        public void CreateUploadStepTest()
        {
            //Arrange
            var pipelineMediator = new Mock<IStepMediator>();
            var webClient = new Mock<IEncodeWebClient>();
            var storageProvider = new Mock<IFileSystem>();
            var tempFileManager = new Mock<ITempFileManager>();
            var fileWrapper = new Mock<IFileWrapper>();

            var settings = new UploadSettings("backendId");

            //Act
            var pipelineStep = new UploadStep(pipelineMediator.Object, webClient.Object, tempFileManager.Object, settings, storageProvider.Object, fileWrapper.Object);

            //Assert
            Assert.IsInstanceOfType(pipelineStep, typeof(PipelineStepBase<EncodeStepData>));
            Assert.IsInstanceOfType(pipelineStep, typeof(PipelineStepBase<EncodeStepData>));
            pipelineMediator.Verify(m => m.AddUploadStep(pipelineStep), Times.Once());
        }

        [TestMethod]
        public void ExecuteUploadStepTest()
        {
            //Arrange
            var fileStream = new MemoryStream();

            _fileWrapper.Setup(m => m.OpenRead(LocalFileUri)).Returns(fileStream);
            _fileSystem.Setup(m => m.UploadArtifactFromStreamAsync(It.Is<StorageFile>(s => s.UserId == UserId &&
                                                                                            s.ContentType == ContentType &&
                                                                                            s.Stream == fileStream),
                                                                    It.IsAny<CancellationToken>()))
                            .Returns(() =>
                                {
                                    var tcs = new TaskCompletionSource<StorageFile>();
                                    var storageFile = new StorageFile() {Hash = FileHash};
                                    tcs.SetResult(storageFile);
                                    return tcs.Task;
                                });

            //Act
            _pipelineStep.Execute(_tokenSource.Object);

            //Assert
            _pipelineMediator.Verify(m => m.Send(It.Is<UploadStepData>(d => d.FileHash == FileHash &&
                                                                            d.EncoderState == EncoderState.Completed), _pipelineStep),
                                     Times.Once());
        }

        [TestMethod]
        public void ExecuteUploadStepThrowExceptionTest()
        {
            //Arrange
            const string message = "messaga";
            var fileStream = new MemoryStream();

            _fileWrapper.Setup(m => m.OpenRead(LocalFileUri)).Returns(fileStream);
            _fileSystem.Setup(m => m.UploadArtifactFromStreamAsync(It.IsAny<StorageFile>(), It.IsAny<CancellationToken>())).Returns(() =>
                {
                    var tcs = new TaskCompletionSource<StorageFile>();
                    tcs.SetException(new Exception(message));
                    return tcs.Task;
                });

            //Act
            _pipelineStep.Execute(_tokenSource.Object);

            //Assert
            _pipelineMediator.Verify(m => m.Send(It.Is<UploadStepData>(d => d.EncoderState == EncoderState.Failed &&
                                                                                   d.ErrorMessage == message), _pipelineStep),
                                     Times.Once());
        }
    }
}