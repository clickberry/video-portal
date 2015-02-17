using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.BackEnd.Encoder.Factory;
using Wrappers;

namespace Portal.BackEnd.Encoder.Test.FactoryTests
{
    [TestClass]
    public class TokenSourceFactoryTest
    {
        [TestMethod]
        public void CreateTokenSourceTest()
        {
            //Arrange
            var factory = new TokenSourceFactory();

            //Act
            var tokenSource = factory.CreateTokenSource();

            //Assert
            Assert.IsInstanceOfType(tokenSource, typeof(CancellationTokenSourceWrapper));
        }
    }
}