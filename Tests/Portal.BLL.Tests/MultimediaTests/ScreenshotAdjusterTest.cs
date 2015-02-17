using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.Adjusters;
using Portal.BLL.Concrete.Multimedia.AdjustmentParameters;
using Portal.Exceptions.Multimedia;
using TestExtension;

namespace Portal.BLL.Tests.MultimediaTests
{
    [TestClass]
    public class ScreenshotAdjusterTest
    {
        private Mock<IAdjustmentScreenshotMetadata> _adjustmentScreenshotParam;
        private ScreenshotAdjuster _screenshotAdjuster;

        [TestInitialize]
        public void Initialize()
        {
            _adjustmentScreenshotParam = new Mock<IAdjustmentScreenshotMetadata>();
            _screenshotAdjuster = new ScreenshotAdjuster(_adjustmentScreenshotParam.Object);
        }

        [TestMethod]
        public void AdjustScreenshotParam()
        {
            //Arrange
            const int videoWidth = 234;
            const int adjustWidth = 548;
            const int videoHeight = 346;
            const int adjustHeight = 548;
            const double duration = 21.575;
            const double adjustOffsetTime = 9;
            const double videoRotation = 90;

            var screenshotAdjusterParam = new ScreenshotAdjusterParam()
                                              {
                                                  ImageWidth=videoWidth,
                                                  ImageHeight=videoHeight,
                                                  Duration=duration,
                                                  VideoRotation = videoRotation
                                              };

            _adjustmentScreenshotParam.Setup(m => m.AdjustScreenshotWidth(videoWidth)).Returns(adjustWidth);
            _adjustmentScreenshotParam.Setup(m => m.AdjustScreenshotHeight(videoHeight)).Returns(adjustHeight);
            _adjustmentScreenshotParam.Setup(m => m.AdjustScreenshotTimeOffset(duration)).Returns(adjustOffsetTime);

            //Act
            var adjustScreenshotParam = _screenshotAdjuster.AdjustScreenshotParam(screenshotAdjusterParam);

            //Assert
            Assert.AreEqual(adjustWidth, adjustScreenshotParam.ImageWidth);
            Assert.AreEqual(adjustHeight, adjustScreenshotParam.ImageHeight);
            Assert.AreEqual(adjustOffsetTime, adjustScreenshotParam.TimeOffset);
            Assert.AreEqual(videoRotation, adjustScreenshotParam.VideoRotation);
        }

        [TestMethod]
        public void CreateScreenshotParamThrowAggregateExceptionTest()
        {
            //Arrange
            var screenshotAdjusterParam = new ScreenshotAdjusterParam();
            _adjustmentScreenshotParam.Setup(m => m.AdjustScreenshotWidth(It.IsAny<int>())).Throws(new VideoFormatException(ParamType.Width));
            _adjustmentScreenshotParam.Setup(m => m.AdjustScreenshotHeight(It.IsAny<int>())).Throws(new VideoFormatException(ParamType.Height));

            //Act & Assert
            var exception = ExceptionAssert.Throws<AggregateException>(()=>_screenshotAdjuster.AdjustScreenshotParam(screenshotAdjusterParam));

            //Assert
            Assert.IsTrue(exception.InnerExceptions.OfType<VideoFormatException>().Any(p => p.ParamType == ParamType.Width));
            Assert.IsTrue(exception.InnerExceptions.OfType<VideoFormatException>().Any(p => p.ParamType == ParamType.Height));
        }
    }
}
