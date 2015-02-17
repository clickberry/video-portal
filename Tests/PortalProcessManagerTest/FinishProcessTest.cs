using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.Domain;
using Portal.Repository.Infrastructure;
using Portal.Repository.Queue;
using Portal.Repository.Video;
using PortalEncoder;
using PortalProcessManager.Data;
using PortalProcessManager.Process;
using Wrappers;

namespace PortalProcessManagerTest
{
    [TestClass]
    public class FinishProcessTest
    {
        private Mock<IEncoder> _encoder;
        private Mock<IVideoRepository> _videoRepository;
        private Mock<IQueueVideoRepository> _queueVideoRepository;
        private Mock<IMediaInfoReader> _mediaInfoReader;
        private Mock<IFileSystemWrapper> _fileSystem;

        [TestInitialize]
        public void Initialize()
        {
            _encoder = new Mock<IEncoder>();
            _videoRepository = new Mock<IVideoRepository>();
            _queueVideoRepository = new Mock<IQueueVideoRepository>();
            _mediaInfoReader = new Mock<IMediaInfoReader>();
            _fileSystem = new Mock<IFileSystemWrapper>();
        }

        [TestMethod]
        public void FinishSuccessfulTest()
        {
            //Arrange
            var videoMssg = new VideoMessage();
            var finishProcess = new FinishProcess(_queueVideoRepository.Object, _videoRepository.Object, _fileSystem.Object);

            var uploadInfo = new UploadInformation()
                                 {
                                     DownloadInformation = new DownloadInformation()
                                                               {
                                                                   QueueInformation = new QueueInformation()
                                                                                          {
                                                                                              VideoMessage = new VideoMessage()
                                                                                          }
                                                               }
                                 };

            //Act
            var obj = finishProcess.ProcessMethod(uploadInfo, new CancellationToken());

            //Assert
            Assert.AreEqual(null, obj);
        }

        [TestMethod]
        public void FinishNotExistVideoTest()
        {
            //Arrange
            var videoMsg = new VideoMessage();
            var finishProcess = new FinishProcess(_queueVideoRepository.Object, _videoRepository.Object, _fileSystem.Object);

            _videoRepository.Setup(m => m.ExistsEncodedVideo(It.IsAny<string>())).Returns(false);

            var uploadInfo = new UploadInformation()
            {
                DownloadInformation = new DownloadInformation()
                {
                    QueueInformation = new QueueInformation()
                    {
                        VideoMessage = videoMsg
                    }
                }
            };

            //Act
            finishProcess.ProcessMethod(uploadInfo, new CancellationToken());

            //Assert
            _videoRepository.Verify(m => m.SetEncodingState(It.IsAny<string>(),EncodingState.InProcess, EncodingStage.CleanUp, null), Times.Once());
            _videoRepository.Verify(m=>m.ExistsEncodedVideo(It.IsAny<string>()), Times.Once());
            _fileSystem.Verify(m => m.DirectoryDelete(It.IsAny<string>()), Times.Once());
            _queueVideoRepository.Verify(m => m.DeleteMessage(videoMsg));
            _videoRepository.Verify(m => m.SetEncodingState(It.IsAny<string>(), EncodingState.Successed, EncodingStage.CleanUp, null), Times.Once());
        }

        [TestMethod]
        public void FinishExistVideoTest()
        {
            //Arrange
            var videoMsg = new VideoMessage();
            var finishProcess = new FinishProcess(_queueVideoRepository.Object, _videoRepository.Object, _fileSystem.Object);

            _videoRepository.Setup(m => m.ExistsEncodedVideo(It.IsAny<string>())).Returns(true);

            var uploadInfo = new UploadInformation()
            {
                DownloadInformation = new DownloadInformation()
                {
                    QueueInformation = new QueueInformation()
                    {
                        VideoMessage = videoMsg
                    }
                }
            };

            //Act
            finishProcess.ProcessMethod(uploadInfo, new CancellationToken());

            //Assert
            _videoRepository.Verify(m => m.SetEncodingState(It.IsAny<string>(), EncodingState.InProcess, EncodingStage.CleanUp, null), Times.Once());
            _videoRepository.Verify(m => m.ExistsEncodedVideo(It.IsAny<string>()), Times.Once());
            _fileSystem.Verify(m => m.DirectoryDelete(It.IsAny<string>()), Times.Never());
            _queueVideoRepository.Verify(m => m.DeleteMessage(videoMsg));
            _videoRepository.Verify(m => m.SetEncodingState(It.IsAny<string>(), EncodingState.Successed, EncodingStage.CleanUp, null), Times.Once());
        }

        [TestMethod]
        public void BrunchOfDeleteTest()
        {
            //Arrange
            var videoMsg = new VideoMessage() {Delete = true};
            var finishProcess = new FinishProcess(_queueVideoRepository.Object, _videoRepository.Object, _fileSystem.Object);
            
            var uploadInfo = new UploadInformation()
            {
                DownloadInformation = new DownloadInformation()
                {
                    QueueInformation = new QueueInformation()
                    {
                        VideoMessage = videoMsg
                    }
                }
            };

            //Act
            finishProcess.ProcessMethod(uploadInfo, new CancellationToken());

            //Assert
            _videoRepository.Verify(m => m.SetEncodingState(It.IsAny<string>(), EncodingState.InProcess, EncodingStage.CleanUp, null), Times.Never());
            _videoRepository.Verify(m => m.ExistsEncodedVideo(It.IsAny<string>()), Times.Never());
            _fileSystem.Verify(m => m.DirectoryDelete(It.IsAny<string>()), Times.Never());
            _queueVideoRepository.Verify(m => m.DeleteMessage(videoMsg), Times.Never());
            _videoRepository.Verify(m => m.SetEncodingState(It.IsAny<string>(), EncodingState.Successed, EncodingStage.CleanUp, null), Times.Never());
        }

        [TestMethod]
        public void FinishExceptionHandlerTest()
        {
            //Arrange
            var finishProcess = new FinishProcess(_queueVideoRepository.Object, _videoRepository.Object, _fileSystem.Object);

            var uploadInfo = new UploadInformation()
                                 {
                                     DownloadInformation = new DownloadInformation()
                                                               {
                                                                   QueueInformation = new QueueInformation()
                                                                                          {
                                                                                              VideoMessage = new VideoMessage()
                                                                                          }
                                                               }
                                 };

            //Act
             finishProcess.ExceptionHandler(new Exception(), uploadInfo);

            //Asert
             _videoRepository.Verify(m => m.SetEncodingState(It.IsAny<string>(),EncodingState.Failed, EncodingStage.CleanUp, null), Times.Once());
        }
    }
}
