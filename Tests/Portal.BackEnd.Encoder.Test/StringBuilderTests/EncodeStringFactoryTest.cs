using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.BackEnd.Encoder.StringBuilder;

namespace Portal.BackEnd.Encoder.Test.StringBuilderTests
{
    [TestClass]
    public class EncodeStringFactoryTest
    {
        [TestMethod]
        public void GetVideoFilterTest()
        {
            //Arrange
            var videoEncodeStringFactory = new EncodeStringFactoryBaseStub();

            //Act
            var videoFilter1 = videoEncodeStringFactory.GetVideoFilter(0);
            var videoFilter2 = videoEncodeStringFactory.GetVideoFilter(90);
            var videoFilter3 = videoEncodeStringFactory.GetVideoFilter(270);
            var videoFilter4 = videoEncodeStringFactory.GetVideoFilter(180);

            Assert.AreEqual(String.Empty, videoFilter1);
            Assert.AreEqual("-vf transpose=1 ", videoFilter2);
            Assert.AreEqual("-vf transpose=2 ", videoFilter3);
            Assert.AreEqual("-vf vflip,hflip ", videoFilter4);
        }


        private class EncodeStringFactoryBaseStub : EncodeStringFactoryBase
        {
        }
    }
}
