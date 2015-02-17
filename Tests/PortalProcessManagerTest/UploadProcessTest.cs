using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.Domain;
using Portal.Repository.Screenshot;
using Portal.Repository.Video;
using PortalEncoder;
using PortalProcessManager.Data;
using PortalProcessManager.Process;
using Wrappers;

namespace PortalProcessManagerTest
{
    [TestClass]
    public class UploadProcessTest
    {
        private Mock<IVideoRepository> _videoRepository;
        private Mock<IScreenshotRepository> _screenshotRepository;
        private Mock<IFileSystemWrapper> _fileSystem;

        [TestInitialize]
        public void Initialize()
        {
            _videoRepository = new Mock<IVideoRepository>();
            _fileSystem = new Mock<IFileSystemWrapper>();
            _screenshotRepository=new Mock<IScreenshotRepository>();
        }

        [TestMethod]
        public void UploadSuccessfulTest()
        {
            //Arrange
            var uploadProcess = new UploadProcess(5, _videoRepository.Object, _screenshotRepository.Object, _fileSystem.Object);
            var encodeInfo = new EncodeInformation()
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
            var uploadInfo = uploadProcess.ProcessMethod(encodeInfo, new CancellationToken());

            //Assert
            Assert.AreEqual(encodeInfo.DownloadInformation, uploadInfo.DownloadInformation);
        }

        [TestMethod]
        public void UploadVerifyTest()
        {
            //Arrange
            var uploadProcess = new UploadProcess(5, _videoRepository.Object, _screenshotRepository.Object, _fileSystem.Object);

            var encodeInfo = new EncodeInformation()
                                 {
                                     EncodeScreenshotList = new List<ScreenshotData>()
                                                                {
                                                                    new ScreenshotData(),
                                                                    new ScreenshotData()
                                                                },
                                     EncodeVideoList = new List<VideoData>()
                                                           {
                                                               new VideoData(),
                                                               new VideoData()
                                                           },
                                     DownloadInformation = new DownloadInformation()
                                                               {
                                                                   QueueInformation = new QueueInformation()
                                                                                          {
                                                                                              VideoMessage = new VideoMessage()
                                                                                          }
                                                               }
                                 };

            //Act
            uploadProcess.ProcessMethod(encodeInfo, new CancellationToken());

            //Assert
            _videoRepository.Verify(m=>m.SetEncodingState(It.IsAny<string>(),EncodingState.InProcess, EncodingStage.Uploading, null), Times.Once());
            _videoRepository.Verify(m => m.UploadEncodedVideo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), 
                Times.Exactly(encodeInfo.EncodeVideoList.Count));

            _screenshotRepository.Verify(m => m.UploadScreenshot(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string>()), 
                Times.Exactly(encodeInfo.EncodeScreenshotList.Count));

            _videoRepository.Verify(m => m.DeleteWatchVideos(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            _videoRepository.Verify(m => m.AddWatchVideos(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once());
            _screenshotRepository.Verify(m => m.DeleteScreenshots(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            _screenshotRepository.Verify(m => m.AddScreenshots(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once());
        }

        [TestMethod]
        public void BrunchOfDeleteTest()
        {
            //Arragnge
            var uploadProcess = new UploadProcess(5, _videoRepository.Object, _screenshotRepository.Object, _fileSystem.Object);

            var encodeInfo = new EncodeInformation()
            {
                DownloadInformation = new DownloadInformation()
                {
                    QueueInformation = new QueueInformation()
                    {
                        VideoMessage = new VideoMessage() { Delete=true}
                    }
                }
            };

            //Act
            var uploadInfo = uploadProcess.ProcessMethod(encodeInfo, new CancellationToken());

            //Assert
            Assert.AreEqual(encodeInfo.DownloadInformation, uploadInfo.DownloadInformation);

            _videoRepository.Verify(m => m.DeleteWatchVideos(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            _screenshotRepository.Verify(m => m.DeleteScreenshots(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void UploadExceptionHandlerTest()
        {
            //Arrange
            const string localPath = "local path";
            var uploadProcess = new UploadProcess(5, _videoRepository.Object, _screenshotRepository.Object, _fileSystem.Object);

            _fileSystem.Setup(m => m.DirectoryExists(localPath)).Returns(true);

            var encodeInfo = new EncodeInformation()
                                 {
                                     DownloadInformation = new DownloadInformation()
                                                               {
                                                                   LocalPath = localPath,
                                                                   QueueInformation = new QueueInformation()
                                                                                          {
                                                                                              VideoMessage = new VideoMessage()
                                                                                          }
                                                               }
                                 };

            //Act
            uploadProcess.ExceptionHandler(new Exception(), encodeInfo);

            //Asert
            _videoRepository.Verify(m => m.SetEncodingState(It.IsAny<string>(), EncodingState.Failed, EncodingStage.Uploading, null));
            _fileSystem.Verify(m => m.DirectoryExists(localPath), Times.Once());
            _fileSystem.Verify(m => m.DirectoryDelete(localPath), Times.Once());
        }
    }
}
