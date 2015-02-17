using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Factory;
using Portal.BackEnd.Encoder.Ffmpeg;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.MiddleEndClient;
using Portal.BackEnd.Encoder.Settings;
using Portal.BackEnd.Encoder.StringBuilder;
using Portal.Domain.BackendContext.Entity;
using Portal.SLL.Abstract;
using Wrappers;
using Wrappers.Interface;

namespace Portal.BackEnd.Encoder.Test.FactoryTests
{
    [TestClass]
    public class ScreenshotEncodeCreatorTest
    {
        [TestMethod]
        public void CreateFfmpegParserTest()
        {
            //Arrange
            var encodeData = new ScreenshotEncodeData();
            var screenshotEncodeCreator = new ScreenshotEncodeCreator(encodeData);
            var tempFileCreator = new Mock<ITempFileManager>();

            //Act
            var snapshotFfmpegString = screenshotEncodeCreator.CreateFfmpegParser();
            
            //Assert
            Assert.AreEqual(typeof(ScreenshotFfmpegParser), snapshotFfmpegString.GetType());
        }

        [TestMethod]
        public void CreateEncodeStringFactoryTest()
        {
            //Arrange
            var encodeData = new ScreenshotEncodeData();
            var screenshotEncodeCreator = new ScreenshotEncodeCreator(encodeData);

            //Act
            var encodeStringFactory = screenshotEncodeCreator.CreateEncodeStringFactory();

            //Assert
            Assert.AreEqual(typeof(ScreenshotEncodeStringFactory), encodeStringFactory.GetType());
        }

        [TestMethod]
        public void CreateEncodeStringBuilderTest()
        {
            //Arrange
            var encodeData = new ScreenshotEncodeData();
            var screenshotEncodeCreator = new ScreenshotEncodeCreator(encodeData);
            var encodeStringFactory = new Mock<IScreenshotEncodeStringFactory>();
            var tempFileManager = new Mock<ITempFileManager>();

            //Act
            var encodeStringBuilder = screenshotEncodeCreator.CreateEncodeStringBuilder(tempFileManager.Object, encodeStringFactory.Object);

            //Assert
            Assert.AreEqual(typeof(ScreenshotEncodeStringBuilder), encodeStringBuilder.GetType());
        }
    }
}