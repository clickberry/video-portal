using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Factory;
using Portal.BackEnd.Encoder.Ffmpeg;
using Portal.BackEnd.Encoder.Interface;
using Wrappers;
using Wrappers.Implementation;

namespace Portal.BackEnd.Encoder.Test.FactoryTests
{
    [TestClass]
    public class EncodeCreatorBaseTest
    {
        [TestMethod]
        public void CreateDataReceivedHandlerTest()
        {
            //Arrange
            var ffmpegParser = new Mock<IFfmpegParser>();
            var creator = new TestingEncodeCreator();

            //Act
            var dataReceivedHandler = creator.CreateDataReceivedHandler(ffmpegParser.Object);

            //Assert
            Assert.IsInstanceOfType(dataReceivedHandler,typeof(DataReceivedHandler));
        }

        internal class TestingEncodeCreator:EncodeCreatorBase
        {
        }
    }
}