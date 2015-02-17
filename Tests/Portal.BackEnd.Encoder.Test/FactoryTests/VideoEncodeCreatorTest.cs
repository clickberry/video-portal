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
using Wrappers.Interface;

namespace Portal.BackEnd.Encoder.Test.FactoryTests
{
    [TestClass]
    public class VideoEncodeCreatorTest
    {
        [TestMethod]
        public void CreateFfmpegParserTest()
        {
            //Arrange
            var encodeData = new VideoEncodeData();
            var videoCreator = new VideoEncodeCreator(encodeData);

            //Act
            var videoFfmpegString = videoCreator.CreateFfmpegParser();

            //Assert
            Assert.AreEqual(typeof (VideoFfmpegParser), videoFfmpegString.GetType());
        }

        [TestMethod]
        public void CreateEncodeStringFactoryTest()
        {
            //Arrange
            var encodeData = new VideoEncodeData();
            var videoCreator = new VideoEncodeCreator(encodeData);

            //Act
            var encodeStringFactory = videoCreator.CreateEncodeStringFactory();

            //Assert
            Assert.AreEqual(typeof(VideoEncodeStringFactory), encodeStringFactory.GetType());
        }

        [TestMethod]
        public void CreateEncodeStringBuilderTest()
        {
            //Arrange
            var encodeData = new VideoEncodeData();
            var videoCreator = new VideoEncodeCreator(encodeData);
            var encodeStringFactory = new Mock<IVideoEncodeStringFactory>();
            var tempFileManager = new Mock<ITempFileManager>();

            //Act
            var encodeStringBuilder = videoCreator.CreateEncodeStringBuilder(tempFileManager.Object, encodeStringFactory.Object);

            //Assert
            Assert.AreEqual(typeof(VideoEncodeStringBuilder), encodeStringBuilder.GetType());
        }
    }
}