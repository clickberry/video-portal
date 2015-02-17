using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Mappers.Statistics;
using Wrappers.Implementation;

namespace Portal.Mappers.Tests.StatisticsTests
{
    [TestClass]
    public class SegmentioClientFactoryTest
    {
        [TestMethod]
        public void CreateSegmentioClientTest()
        {
            //Arrange
            const string secretKey = "secretKey";

            var factory = new SegmentioClientFactory();

            //Act
            var client = factory.CreateClient(secretKey);

            //Assert
            Assert.IsInstanceOfType(client,typeof(SegmentioClient));
        }

        [TestMethod]
        public void CreateEmptySegmentioClientTest()
        {
            //Arrange
            var factory = new SegmentioClientFactory();

            //Act
            var client1 = factory.CreateClient(null);
            var client2 = factory.CreateClient("");

            //Assert
            Assert.IsInstanceOfType(client1, typeof(EmptySegmentioClient));
            Assert.IsInstanceOfType(client2, typeof(EmptySegmentioClient));
        }
    }
}