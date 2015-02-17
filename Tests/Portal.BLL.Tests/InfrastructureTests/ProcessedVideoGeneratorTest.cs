using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Infrastructure.ProcessedEntity;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.Calculator;
using Portal.BLL.Concrete.Multimedia.Factory;
using Portal.BLL.Concrete.Multimedia.Lists;
using Portal.Domain.BackendContext.Constant;
using Portal.Domain.EncoderContext;
using Portal.Domain.ProcessedVideoContext;

namespace Portal.BLL.Tests.InfrastructureTests
{
    [TestClass]
    public class ProcessedVideoGeneratorTest
    {
        [TestMethod]
        public void GenerateProcessedVedeoTest()
        {
            //Arrange
            var calculator = new Mock<IResolutionCalculator>();
            var paramFactory = new Mock<IMultimediaAdjusterParamFactory>();
            var processedVideoList = new Mock<IProcessedVideoList>();

            var processedVideoGenerator = new ProcessedVideoGenerator(calculator.Object, paramFactory.Object, processedVideoList.Object);

            var videoParam = new VideoAdjusterParam();
            var audioParam = new AudioAdjusterParam();

            var sizeList = new List<IVideoSize>();
            var mp4ProcessedVideos = new List<DomainProcessedVideo> {new DomainProcessedVideo()};
            var webmProcessedVideos = new List<DomainProcessedVideo> {new DomainProcessedVideo()};

            var metadata = new Mock<IVideoMetadata>();
            metadata.Setup(p => p.VideoWidth).Returns(2345);
            metadata.Setup(p => p.VideoHeight).Returns(345);

            calculator.Setup(m => m.Calculate(metadata.Object.VideoWidth, metadata.Object.VideoHeight)).Returns(sizeList);
            paramFactory.Setup(m => m.CreateVideoParam(metadata.Object)).Returns(videoParam);
            paramFactory.Setup(m => m.CreateAudioParam(metadata.Object)).Returns(audioParam);

            processedVideoList.Setup(m => m.CreateProcessedVideos(videoParam, audioParam, MetadataConstant.Mp4Container, sizeList, ContentType.Mp4Content)).Returns(mp4ProcessedVideos);
            processedVideoList.Setup(m => m.CreateProcessedVideos(videoParam, audioParam, MetadataConstant.WebmContainer, sizeList, ContentType.WebmContent)).Returns(webmProcessedVideos);

            //Act
            List<DomainProcessedVideo> list = processedVideoGenerator.Generate(metadata.Object);

            //Assert
            Assert.AreEqual(mp4ProcessedVideos.Count + webmProcessedVideos.Count, list.Count);
            Assert.IsTrue(mp4ProcessedVideos.All(p => list.Any(procVid => procVid == p)));
            Assert.IsTrue(webmProcessedVideos.All(p => list.Any(procVid => procVid == p)));
        }
    }
}