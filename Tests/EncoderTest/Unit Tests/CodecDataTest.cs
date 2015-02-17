using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortalEncoder;

namespace EncoderTest
{
    [TestClass]
    public class CodecDataTest
    {
        [TestMethod]
        public void CreateCodecDataWithProfileTest()
        {
            //Arrange
            const string videoCodec = "videoCodec";
            const string videoProfile1 = "videoProfile1";
            const string videoProfile2 = "videoProfile2";
            const string libName = "libName";

            //Act
            var codecData = new CodecData(videoCodec, libName, videoProfile1, videoProfile1, videoProfile2);

            //Assert
            Assert.AreEqual(videoCodec, codecData.Codec);
            Assert.AreEqual(videoProfile1, codecData.DefaultProfile);
            Assert.AreEqual(libName, codecData.LibName);
            Assert.IsTrue(codecData.Profiles.Any(p => p == videoProfile1));
            Assert.IsTrue(codecData.Profiles.Any(p => p == videoProfile2));
        }

        [TestMethod]
        public void CreateCodecDataWithoutProfileTest()
        {
            //Arrange
            const string videoCodec = "videoCodec";
            const string libName = "libName";

            //Act
            var codecData = new CodecData(videoCodec, libName);

            //Assert
            Assert.AreEqual(videoCodec, codecData.Codec);
            Assert.AreEqual(libName, codecData.LibName);
            Assert.IsTrue(codecData.Profiles.Any(p => p == null));
        }
    }
}