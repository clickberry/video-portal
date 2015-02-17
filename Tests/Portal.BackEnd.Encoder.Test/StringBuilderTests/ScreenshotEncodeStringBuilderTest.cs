using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.StringBuilder;
using Portal.Domain.BackendContext.Entity;

namespace Portal.BackEnd.Encoder.Test.StringBuilderTests
{
    [TestClass]
    public class ScreenshotEncodeStringBuilderTest
    {
        private const string DestinationPath = "destinationPath";
        private const string InputVideoLocalFilePath = "video url";
        private const int TimeOffset = 7;
        private const double VideoRotation = 90.0;

        private ScreenshotEncodeData _encodeData;

        [TestInitialize]
        public void Initialize()
        {
            _encodeData = new ScreenshotEncodeData()
            {
                ScreenshotParam = new ScreenshotParam()
                {
                    TimeOffset = TimeOffset,
                    VideoRotation = VideoRotation
                }
            };
        }

        [TestMethod]
        public void GetScreenshotFfmpegStringTest()
        {
            //Arrange
            const string videoFilter = "videoFilter";
            const string imageOption = "imageOption";

            var expectedStr = String.Format(@"-i ""{0}"" -f image2 {1} -frames:v 1 {2} -y ""{3}""",
                                            InputVideoLocalFilePath,
                                            imageOption,
                                            videoFilter,
                                            DestinationPath);

            var videoEncodeStringFactory = new Mock<IScreenshotEncodeStringFactory>();
            var tempFileManager = new Mock<ITempFileManager>();

            videoEncodeStringFactory.Setup(m => m.GetVideoFilter((int)VideoRotation)).Returns(videoFilter);
            videoEncodeStringFactory.Setup(m => m.GetImageOption(TimeOffset)).Returns(imageOption);

            tempFileManager.Setup(m => m.GetOriginalTempFilePath()).Returns(InputVideoLocalFilePath);
            tempFileManager.Setup(m => m.GetEncodingTempFilePath()).Returns(DestinationPath);

            var stringBuilder = new ScreenshotEncodeStringBuilder(_encodeData,videoEncodeStringFactory.Object, tempFileManager.Object);

            //Act
            var actualStr = stringBuilder.GetFfmpegArguments();

            //Assert
            Assert.AreEqual(expectedStr, actualStr);
        }
    }
}