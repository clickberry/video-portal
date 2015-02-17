using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Portal.BackEnd.Encoder.Pipeline.Step;
using Portal.DAL.Entities.Storage;
using Portal.DAL.FileSystem;
using Portal.Domain.BackendContext.Entity.Base;
using Portal.Domain.BackendContext.Enum;
using Portal.Exceptions.CRUD;
using Wrappers;
using Wrappers.Interface;

namespace Portal.BackEnd.Encoder.Test.PipelineTests
{
    [TestClass]
    public class DownloadStepTest
    {
        private Mock<IStepMediator> _pipelineMediator;
        private Mock<IFileSystem> _fileSystem;
        private Mock<IFileWrapper> _fileWrapper;
        private Mock<ITempFileManager> _tempFileCreator;
        private Mock<CancellationTokenSourceWrapper> _tokenSource;
        private Mock<IEncodeData> _encodeData;
        private DownloadStep _pipelineStep;

        [TestInitialize]
        public void Initialize()
        {
            _pipelineMediator = new Mock<IStepMediator>();
            _fileSystem = new Mock<IFileSystem>();
            _fileWrapper = new Mock<IFileWrapper>();
            _tempFileCreator = new Mock<ITempFileManager>();
            _tokenSource = new Mock<CancellationTokenSourceWrapper>();
            _encodeData = new Mock<IEncodeData>();

            _pipelineStep = new DownloadStep(_pipelineMediator.Object, _tempFileCreator.Object, _fileSystem.Object, _fileWrapper.Object);

            var stepData = new GettingEntityStepData()
            {
                EncoderState = EncoderState.Completed,
                EncodeData = _encodeData.Object
            };
            _pipelineStep.SetData(stepData);
        }

        [TestMethod]
        public void CreateDownloadStepTest()
        {
            //Arrange
            var mediator = new Mock<IStepMediator>();
            var fileSystem = new Mock<IFileSystem>();
            var tempFileCreator = new Mock<ITempFileManager>();
            var fileWrapper = new Mock<IFileWrapper>();

            //Act
            var pipelineStep = new DownloadStep(mediator.Object, tempFileCreator.Object, fileSystem.Object, fileWrapper.Object);
            
            //Assert
            Assert.IsInstanceOfType(pipelineStep, typeof(PipelineStepBase<GettingEntityStepData>));
            Assert.IsInstanceOfType(pipelineStep, typeof(PipelineStep<GettingEntityStepData>));
            mediator.Verify(m => m.AddDownloadStep(pipelineStep), Times.Once());
        }

        [TestMethod]
        public void ExecuteDownloadStepTest()
        {
            //Arrange
            const string filePath = "filePath";
            const string videoFileId = "videoFileId";
            var fileStream=new MemoryStream();

            _encodeData.Setup(p => p.FileId).Returns(videoFileId);
            _tempFileCreator.Setup(m => m.GetOriginalTempFilePath()).Returns(filePath);
            _fileWrapper.Setup(m=>m.OpenWrite(filePath)).Returns(fileStream);
            _fileSystem.Setup(m => m.DownloadFileToStreamAsync(It.Is<StorageFile>(s=>s.Id==videoFileId && s.Stream==fileStream), CancellationToken.None)).Returns(() =>
                {
                    var tcs = new TaskCompletionSource<StorageFile>();
                    tcs.SetResult(null);
                    return tcs.Task;
                });

            //Act
            _pipelineStep.Execute(_tokenSource.Object);

            //Assert
            _pipelineMediator.Verify(m => m.Send(It.Is<GettingEntityStepData>(d => d.EncoderState == EncoderState.Completed &&
                                                                           d.EncodeData == _encodeData.Object), _pipelineStep),
                             Times.Once());
        }

        [TestMethod]
        public void ExecuteDownloadStepThrowExceptionTest()
        {
            //Arrange
            const string message = "messaga";

            _fileSystem.Setup(m => m.DownloadFileToStreamAsync(It.IsAny<StorageFile>(), It.IsAny<CancellationToken>())).Returns(() =>
                {
                    var tcs = new TaskCompletionSource<StorageFile>();
                    tcs.SetException(new Exception(message));
                    return tcs.Task;
                });

            //Act
            _pipelineStep.Execute(_tokenSource.Object);

            //Assert
            _pipelineMediator.Verify(m => m.Send(It.Is<GettingEntityStepData>(d => d.EncoderState == EncoderState.Failed &&
                                                                           d.ErrorMessage == message), _pipelineStep),
                             Times.Once());
        }

        [TestMethod]
        public void ExecuteDownloadStepThrowNotFoundExceptionTest()
        {
            //Arrange
            _fileSystem.Setup(m => m.DownloadFileToStreamAsync(It.IsAny<StorageFile>(), It.IsAny<CancellationToken>())).Returns(() =>
            {
                var tcs = new TaskCompletionSource<StorageFile>();
                tcs.SetException(new NotFoundException());
                return tcs.Task;
            });

            //Act
            _pipelineStep.Execute(_tokenSource.Object);

            //Assert
            _pipelineMediator.Verify(m => m.Send(It.Is<GettingEntityStepData>(d => d.EncoderState == EncoderState.Deleted), _pipelineStep),
                             Times.Once());
        }
    }
}