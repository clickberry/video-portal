using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.Domain;
using Portal.Repository.Infrastructure;
using Portal.Repository.Video;
using PortalEncoder;
using PortalProcessManager.Data;
using PortalProcessManager.Interface;
using Portal.Repository.Queue;
using PortalProcessManager.Process;
using Wrappers;

namespace PortalProcessManagerTest
{
    [TestClass]
    public class EncodeProcessTest
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
        public void EncodeSuccessfulTest()
        {
            //Arragnge
            var videoMssg = new VideoMessage();
            var videoList = new List<VideoData>();
            var screenshotList = new List<ScreenshotData>();

            var encodeProcess = new EncodeProcess(5, _encoder.Object, _videoRepository.Object, _mediaInfoReader.Object, _queueVideoRepository.Object, _fileSystem.Object);

            _encoder.Setup(m => m.EncodeVideo(It.IsAny<VideoMediaInfo>(), It.IsAny<string>(), It.IsAny<string>())).Returns(videoList);
            _encoder.Setup(m => m.EncodeScreenshot(It.IsAny<VideoMediaInfo>(), It.IsAny<string>(), It.IsAny<string>())).Returns(screenshotList);

            var downloadInfo = new DownloadInformation()
                                   {
                                       LocalPath = "local path",
                                       LocalFilePath = "local file path",
                                       QueueInformation = new QueueInformation()
                                                              {
                                                                  VideoMessage = videoMssg
                                                              }
                                   };

            //Act
            var encodeInfo = encodeProcess.ProcessMethod(downloadInfo, new CancellationToken());

            //Assert
            Assert.AreEqual(downloadInfo.QueueInformation.VideoMessage, videoMssg);
            Assert.AreEqual(downloadInfo.LocalFilePath, encodeInfo.DownloadInformation.LocalFilePath);
            Assert.AreEqual(downloadInfo.LocalPath, encodeInfo.DownloadInformation.LocalPath);
            Assert.AreEqual(videoList, encodeInfo.EncodeVideoList);
            Assert.AreEqual(screenshotList, encodeInfo.EncodeScreenshotList);
        }

        [TestMethod]
        public void EncodeNotExistVideoTest()
        {
            //Arragnge
            var videoMssg = new VideoMessage();

            var encodeProcess = new EncodeProcess(5, _encoder.Object, _videoRepository.Object, _mediaInfoReader.Object, _queueVideoRepository.Object, _fileSystem.Object);

            _videoRepository.Setup(m => m.ExistsEncodedVideo(It.IsAny<string>())).Returns(false);

            var downloadInfo = new DownloadInformation()
                                   {
                                       LocalPath = "local path",
                                       LocalFilePath = "local file path",
                                       QueueInformation = new QueueInformation()
                                                              {
                                                                  VideoMessage = videoMssg
                                                              }
                                   };

            //Act
            encodeProcess.ProcessMethod(downloadInfo, new CancellationToken());

            //Assert
            _videoRepository.Verify(m => m.SetEncodingState(It.IsAny<string>(), EncodingState.InProcess, EncodingStage.Encoding, null), Times.Once());
            _videoRepository.Verify(m => m.ExistsEncodedVideo(It.IsAny<string>()), Times.Once());
            _mediaInfoReader.Verify(m => m.GetInformation(It.IsAny<string>()), Times.Once());
            _videoRepository.Verify(m => m.FillMediaInfoTables(It.IsAny<Dictionary<Enum, object>>(), It.IsAny<string>()), Times.Once());
            _videoRepository.Verify(m => m.GetVideoMediaInfo(It.IsAny<Dictionary<Enum, object>>()), Times.Once());
            _encoder.Verify(m => m.EncodeVideo(It.IsAny<VideoMediaInfo>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            _encoder.Verify(m => m.EncodeScreenshot(It.IsAny<VideoMediaInfo>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void EncodeExistVideoTest()
        {
            //Arragnge
            var videoMsg = new VideoMessage();

            var encodeProcess = new EncodeProcess(5, _encoder.Object, _videoRepository.Object, _mediaInfoReader.Object, _queueVideoRepository.Object, _fileSystem.Object);

            _videoRepository.Setup(m => m.ExistsEncodedVideo(It.IsAny<string>())).Returns(true);

            var downloadInfo = new DownloadInformation()
                                   {
                                       LocalPath = "local path",
                                       LocalFilePath = "local file path",
                                       QueueInformation = new QueueInformation()
                                                              {
                                                                  VideoMessage = videoMsg
                                                              }
                                   };

            //Act
            encodeProcess.ProcessMethod(downloadInfo, new CancellationToken());

            //Assert
            _videoRepository.Verify(m => m.SetEncodingState(It.IsAny<string>(), EncodingState.InProcess, EncodingStage.Encoding, null), Times.Once());
            _videoRepository.Verify(m => m.ExistsEncodedVideo(It.IsAny<string>()), Times.Once());
            _mediaInfoReader.Verify(m => m.GetInformation(It.IsAny<string>()), Times.Never());
            _videoRepository.Verify(m => m.FillMediaInfoTables(It.IsAny<Dictionary<Enum, object>>(), It.IsAny<string>()), Times.Never());
            _videoRepository.Verify(m => m.GetVideoMediaInfo(It.IsAny<Dictionary<Enum, object>>()), Times.Never());
            _encoder.Verify(m => m.EncodeVideo(It.IsAny<VideoMediaInfo>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            _encoder.Verify(m => m.EncodeScreenshot(It.IsAny<VideoMediaInfo>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void BrunchOfDeleteTest()
        {
            //Arragnge
            var videoMsg = new VideoMessage() {Delete = true};

            var encodeProcess = new EncodeProcess(5, _encoder.Object, _videoRepository.Object, _mediaInfoReader.Object, _queueVideoRepository.Object, _fileSystem.Object);
            var downloadInfo = new DownloadInformation()
                                   {
                                       QueueInformation = new QueueInformation()
                                                              {
                                                                  VideoMessage = videoMsg
                                                              }
                                   };
            //Act
            var encodeInfo = encodeProcess.ProcessMethod(downloadInfo, new CancellationToken());

            //Assert
            Assert.AreEqual(downloadInfo, encodeInfo.DownloadInformation);

            _videoRepository.Verify(m => m.SetEncodingState(It.IsAny<string>(), EncodingState.InProcess, EncodingStage.Encoding, null), Times.Never());
            _mediaInfoReader.Verify(m => m.GetInformation(It.IsAny<string>()), Times.Never());
            _videoRepository.Verify(m => m.FillMediaInfoTables(It.IsAny<Dictionary<Enum, object>>(), It.IsAny<string>()), Times.Never());
            _videoRepository.Verify(m => m.GetVideoMediaInfo(It.IsAny<Dictionary<Enum, object>>()), Times.Never());
            _encoder.Verify(m => m.EncodeVideo(It.IsAny<VideoMediaInfo>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            _encoder.Verify(m => m.EncodeScreenshot(It.IsAny<VideoMediaInfo>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());

        }

        [TestMethod]
        public void EncodeExceptionHandlerTest()
        {
            //Arrange
            const string localPath = "local path";
            var videoMsg = new VideoMessage();
            var encodeProcess = new EncodeProcess(5, _encoder.Object, _videoRepository.Object, _mediaInfoReader.Object, _queueVideoRepository.Object, _fileSystem.Object);

             var downloadInfo = new DownloadInformation()
                                   {
                                       LocalPath=localPath,
                                       QueueInformation = new QueueInformation()
                                                              {
                                                                  VideoMessage = videoMsg
                                                              }
                                   };

            //Act
            encodeProcess.ExceptionHandler(new Exception(), downloadInfo);

            //Asert
            _videoRepository.Verify(m=>m.SetEncodingState(It.IsAny<string>(),EncodingState.Failed, EncodingStage.Encoding, null), Times.Once());
            _fileSystem.Verify(m=>m.DirectoryDelete(localPath), Times.Once());
            _queueVideoRepository.Verify(m=>m.DeleteMessage(videoMsg), Times.Once());
        }
    }
}
