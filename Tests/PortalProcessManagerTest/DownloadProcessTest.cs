using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.Domain;
using Portal.Repository.Video;
using PortalProcessManager.Data;
using PortalProcessManager.Process;
using Wrappers;

namespace PortalProcessManagerTest
{
    [TestClass]
    public class DownloadProcessTest
    {
        private Mock<IVideoRepository> _videoiRepository;
        private Mock<IFileSystemWrapper> _fileSystemWrapper;

        [TestInitialize]
        public void Initialize()
        {
            _videoiRepository = new Mock<IVideoRepository>();
            _fileSystemWrapper = new Mock<IFileSystemWrapper>();
        }
        [TestMethod]
        public void DownloadSuccessfulTest()
        {
            //Arrange
            string localFilePath = null;

            _videoiRepository.Setup(m => m.DownloadOriginalVideo(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>((hash, filePath) => localFilePath = filePath);
            
            _fileSystemWrapper.Setup(m => m.GetTempPath()).Returns("my temp path");
            _fileSystemWrapper.Setup(m => m.PathCombine(It.IsAny<string>(), It.IsAny<string>())).Returns<string,string>(Path.Combine);
            
            var queueInformation = new QueueInformation()
                                       {
                                           VideoMessage = new VideoMessage()
                                       };
            

            var downloadProcess = new DownloadProcess(5, _videoiRepository.Object, _fileSystemWrapper.Object);
            
            //Act
            var downloadInfo =  downloadProcess.ProcessMethod(queueInformation, new CancellationToken());

            //Assert
            Assert.AreEqual(queueInformation.VideoMessage, downloadInfo.QueueInformation.VideoMessage);
            Assert.AreEqual(localFilePath, downloadInfo.LocalFilePath);
            Assert.AreEqual(Path.GetDirectoryName(localFilePath), downloadInfo.LocalPath);
        }

        [TestMethod]
        public void DownloadNotExistVideoTest()
        {
            //Arrange
            string localFilePath = null;
            
            _fileSystemWrapper.Setup(m => m.GetTempPath()).Returns("my temp path");
            _fileSystemWrapper.Setup(m => m.PathCombine(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>(Path.Combine);

            _videoiRepository.Setup(m => m.ExistsEncodedVideo(It.IsAny<string>())).Returns(false);
            _videoiRepository.Setup(m => m.DownloadOriginalVideo(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>((hash, filePath) => localFilePath = filePath);

            var queueInformation = new QueueInformation()
            {
                VideoMessage = new VideoMessage()
            };
            
            var downloadProcess = new DownloadProcess(5, _videoiRepository.Object, _fileSystemWrapper.Object);

            //Act
            downloadProcess.ProcessMethod(queueInformation, new CancellationToken());

            //Assert
            _videoiRepository.Verify(m => m.SetEncodingState(It.IsAny<string>(), EncodingState.InProcess, EncodingStage.Downloading, null), Times.Once());
            _videoiRepository.Verify(m=>m.ExistsEncodedVideo(It.IsAny<string>()), Times.Once());
            _videoiRepository.Verify(m => m.DownloadOriginalVideo(It.IsAny<string>(), localFilePath), Times.Once());
        }

        [TestMethod]
        public void DownloadExistVideoTest()
        {
            //Arrange
            var downloadProcess = new DownloadProcess(5, _videoiRepository.Object, _fileSystemWrapper.Object);

            _videoiRepository.Setup(m => m.ExistsEncodedVideo(It.IsAny<string>())).Returns(true);

            var queueInformation = new QueueInformation()
            {
                VideoMessage = new VideoMessage()
            };
            
            //Act
            downloadProcess.ProcessMethod(queueInformation, new CancellationToken());

            //Assert
            _videoiRepository.Verify(m => m.SetEncodingState(It.IsAny<string>(), EncodingState.InProcess, EncodingStage.Downloading, null), Times.Once());
            _videoiRepository.Verify(m => m.ExistsEncodedVideo(It.IsAny<string>()), Times.Once());
            _videoiRepository.Verify(m => m.DownloadOriginalVideo(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void BrunchOfDeleteTest()
        {
            //Arragnge
            var downloadProcess = new DownloadProcess(5, _videoiRepository.Object, _fileSystemWrapper.Object);

            var queueInformation = new QueueInformation()
                                       {
                                           VideoMessage = new VideoMessage()
                                                              {
                                                                  Delete = true
                                                              }
                                       };
            
            //Act
            var downloadInfo = downloadProcess.ProcessMethod(queueInformation, new CancellationToken());

            //Assert
            _videoiRepository.Verify(m => m.SetEncodingState(It.IsAny<string>(), EncodingState.InProcess, EncodingStage.Downloading, null), Times.Never());
            _videoiRepository.Verify(m => m.DownloadOriginalVideo(It.IsAny<string>(), It.IsAny<string>()), Times.Never());

            Assert.AreEqual(queueInformation.VideoMessage, downloadInfo.QueueInformation.VideoMessage);
        }

        [TestMethod]
        public void DownloadExceptionHandlerTest()
        {
            //Arrange
            var downloadProcess = new DownloadProcess(5, _videoiRepository.Object, _fileSystemWrapper.Object);

            var queueInformation = new QueueInformation()
                                       {
                                           VideoMessage = new VideoMessage()
                                                              {
                                                                  Delete = true
                                                              }
                                       };

            _fileSystemWrapper.Setup(m => m.DirectoryExists(It.IsAny<string>())).Returns(true);

            //Act
            downloadProcess.ExceptionHandler(new Exception(), queueInformation);
            
            //Asert
            _videoiRepository.Verify(m => m.SetEncodingState(It.IsAny<string>(), EncodingState.Failed, EncodingStage.Downloading, null), Times.Once());

            _fileSystemWrapper.Verify(m => m.DirectoryExists(It.IsAny<string>()), Times.Once());
            _fileSystemWrapper.Verify(m => m.DirectoryDelete(It.IsAny<string>()), Times.Once());
        }
    }
}
