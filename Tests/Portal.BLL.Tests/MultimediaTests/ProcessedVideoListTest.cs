using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.Builder;
using Portal.BLL.Concrete.Multimedia.Calculator;
using Portal.BLL.Concrete.Multimedia.Lists;
using Portal.Domain.ProcessedVideoContext;

namespace Portal.BLL.Tests.MultimediaTests
{
    [TestClass]
    public class ProcessedVideoListTest
    {
        [TestMethod]
        public void CreateProcessedVideosTest()
        {
            //Arrange
            const string newContainer = "newContainer";
            const string contentType = "contentType";

            var processedVideoBuilder = new Mock<IProcessedVideoBuilder>();
            var processedVideoList = new ProcessedVideoList(processedVideoBuilder.Object);

            var videoParam = new VideoAdjusterParam();
            var audioParam = new AudioAdjusterParam();
            var processedVideo1 = new DomainProcessedVideo();
            var processedVideo2 = new DomainProcessedVideo();

            var size1 = new Mock<IVideoSize>();
            var size2 = new Mock<IVideoSize>();
            var sizeList = new List<IVideoSize> {size1.Object, size2.Object};

            processedVideoBuilder.Setup(m => m.BuildProcessedVideo(videoParam, audioParam, newContainer, size1.Object, contentType)).Returns(processedVideo1);
            processedVideoBuilder.Setup(m => m.BuildProcessedVideo(videoParam, audioParam, newContainer, size2.Object, contentType)).Returns(processedVideo2);

            //Act
            IEnumerable<DomainProcessedVideo> list = processedVideoList.CreateProcessedVideos(videoParam, audioParam, newContainer, sizeList, contentType);

            //Assert
            Assert.AreEqual(sizeList.Count, list.Count());
            Assert.IsTrue(list.Any(p => p == processedVideo1));
            Assert.IsTrue(list.Any(p => p == processedVideo1));
        }
    }
}