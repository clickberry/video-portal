using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.BackEnd.Encoder.StringBuilder;

namespace Portal.BackEnd.Encoder.Test.StringBuilderTests
{
    [TestClass]
    public class ScreenshotEncodeStringFactoryTest
    {
        [TestMethod]
        public void GetImageOptionTest()
        {
            //Arrange
            const double timeOffset = 12.34;
            var screenshotEncodeStringFactory = new ScreenshotEncodeStringFactory();

            //Act
            var imageOption = screenshotEncodeStringFactory.GetImageOption(timeOffset);

            //Assert
            Assert.AreEqual(String.Format("-ss {0}", timeOffset), imageOption);
        }
    }
}
