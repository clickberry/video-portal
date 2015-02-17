using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.BackEnd.Encoder.Factory;
using Portal.Domain.BackendContext.Entity;
using Portal.Domain.BackendContext.Enum;

namespace Portal.BackEnd.Encoder.Test.FactoryTests
{
    [TestClass]
    public class EncodeCreatorFactoryTest
    {
        [TestMethod]
        public void CreateVideoEncodeCreatorTest()
        {
            //Arrange
            var encodeData = new VideoEncodeData();
            var creatorFactory = new EncodeCreatorFactory();

            //Act
            var creator = creatorFactory.Create(encodeData);

            //Assert
            Assert.IsInstanceOfType(creator, typeof(VideoEncodeCreator));
        }

        [TestMethod]
        public void CreateScreenshotEncodeCreatorTest()
        {
            //Arrange
            var encodeData = new ScreenshotEncodeData();
            var creatorFactory = new EncodeCreatorFactory();

            //Act
            var creator = creatorFactory.Create(encodeData);

            //Assert
            Assert.IsInstanceOfType(creator, typeof(ScreenshotEncodeCreator));
        }
    }
}
