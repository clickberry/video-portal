using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Ffmpeg;
using Portal.BackEnd.Encoder.Interface;
using Portal.Domain.BackendContext.Entity;

namespace Portal.BackEnd.Encoder.Test.FfmpegTests
{
    [TestClass]
    public class ScreenshotFfmpegParserTest
    {
        [TestMethod]
        public void ParseDurationTest()
        {
            var parser = new ScreenshotFfmpegParser();

            //Act
            var result = parser.ParseDuration(It.IsAny<string>());

            //Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void ParseEncodeTimeTest()
        {
            var parser = new ScreenshotFfmpegParser();

            //Act
            var result = parser.ParseEncodeTime(It.IsAny<string>());

            //Assert
            Assert.AreEqual(0, result);
        }
    }
}