using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Multimedia.Factory;
using Portal.DAL.Entities;
using Portal.Domain.EncoderContext;
using Portal.Exceptions.Multimedia;
using TestExtension;

namespace Portal.BLL.Tests.MultimediaTests
{
    [TestClass]
    public class ScreenshotAdjusterParamFactoryTest
    {
        private Mock<IVideoMetadata> _videoMetadata;

        [TestInitialize]
        public void Initialize()
        {
            _videoMetadata = new Mock<IVideoMetadata>();
            _videoMetadata.Setup(p => p.VideoWidth).Returns(643);
            _videoMetadata.Setup(p => p.VideoHeight).Returns(234);
            _videoMetadata.Setup(p => p.VideoDuration).Returns(154654);
            _videoMetadata.Setup(p => p.VideoStreamsCount).Returns(1);
            _videoMetadata.Setup(p => p.VideoRotation).Returns(180);
        }

        [TestMethod]
        public void CreateScreenshotAdjusterParamTest()
        {
            //Arrange
            var factory = new ScreenshotAdjusterParamFactory();

            //Act
            var screenshotAdjusterParam = factory.CreateScreenshotAdjusterParam(_videoMetadata.Object);

            //Assert
            Assert.AreEqual(_videoMetadata.Object.VideoWidth, screenshotAdjusterParam.ImageWidth);
            Assert.AreEqual(_videoMetadata.Object.VideoHeight, screenshotAdjusterParam.ImageHeight);
            Assert.AreEqual(154.654, screenshotAdjusterParam.Duration);
            Assert.AreEqual(_videoMetadata.Object.VideoRotation, screenshotAdjusterParam.VideoRotation);
        }

        [TestMethod]
        public void CreateScreenshotAdjusterParamThrowAggregateExceptionTest()
        {
            //Arrange
            var factory = new ScreenshotAdjusterParamFactory();
            var videoMetadata0 = new Mock<IVideoMetadata>();
            var videoMetadata1 = new Mock<IVideoMetadata>();
            var videoMetadata2 = new Mock<IVideoMetadata>();

            videoMetadata0.Setup(p=>p.VideoStreamsCount).Returns(0);
            videoMetadata1.Setup(p => p.VideoStreamsCount).Returns(1);
            videoMetadata2.Setup(p => p.VideoStreamsCount).Returns(2);

            //Act & Assert
            var exception0 = ExceptionAssert.Throws<AggregateException>(() => factory.CreateScreenshotAdjusterParam(videoMetadata0.Object));
            var exception2 = ExceptionAssert.Throws<AggregateException>(() => factory.CreateScreenshotAdjusterParam(videoMetadata2.Object));
            ExceptionAssert.NotThrows<AggregateException>(() => factory.CreateScreenshotAdjusterParam(videoMetadata1.Object));

            //Assert
            Assert.IsTrue(exception0.InnerExceptions.OfType<VideoFormatException>().Any(p => p.ParamType == ParamType.VideoStreamCount));
            Assert.IsTrue(exception2.InnerExceptions.OfType<VideoFormatException>().Any(p => p.ParamType == ParamType.VideoStreamCount));
        }
    }
}
