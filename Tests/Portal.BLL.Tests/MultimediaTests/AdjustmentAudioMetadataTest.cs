using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.BLL.Concrete.Multimedia.AdjustmentParameters;
using Portal.BLL.Concrete.Multimedia.Constraints;
using Portal.Domain.BackendContext.Constant;
using Portal.Exceptions.Multimedia;
using TestExtension;

namespace Portal.BLL.Tests.MultimediaTests
{
    [TestClass]
    public class AdjustmentAudioMetadataTest
    {
        private AudioConstraints _constraint;
        private AdjustmentAudioMetadata _adjustmentAudioMetadata;

        [TestInitialize]
        public void Initialize()
        {
            _constraint = new AudioConstraints();
            _adjustmentAudioMetadata = new AdjustmentAudioMetadata(_constraint);
        }

        [TestMethod]
        public void AdjustAudioCodecForMp4Test()
        {
            //Arrange
            const string anyCodec = "anyCodec";

            //Act
            var adjustCodec = _adjustmentAudioMetadata.AdjustAudioCodec(MetadataConstant.Mp4Container, anyCodec);

            //Assert
            Assert.AreEqual(MetadataConstant.AacCodec, adjustCodec);
        }

        [TestMethod]
        public void AdjustAudioCodecForWebmTest()
        {
            //Arrange
            const string anyCodec = "anyCodec";

            //Act
            var adjustCodec = _adjustmentAudioMetadata.AdjustAudioCodec(MetadataConstant.WebmContainer, anyCodec);

            //Assert
            Assert.AreEqual(MetadataConstant.VorbisCodec, adjustCodec);
        }

        [TestMethod]
        public void AdjustAudioCodecThrowExceptionTest()
        {
            //Act & Assert
            var exception = ExceptionAssert.Throws<VideoFormatException>(()=>_adjustmentAudioMetadata.AdjustAudioCodec(MetadataConstant.WebmContainer, null));

            //Assert
            Assert.AreEqual(ParamType.AudioCodec, exception.ParamType);
        }

        [TestMethod]
        public void AdjustAudioBitrateForMoreOrEqual720POneChannelTest()
        {
            //Arrange
            const uint sizeDelta = 26;
            const uint bitrateDelta = 1207;
            const int samplerate = 12345;

            var size = (int)(_constraint.Size720P + sizeDelta);
            var bitrateMoreOrEqualMax = (int)(_constraint.MaxAudioBitrate720POneChannel + bitrateDelta);
            var bitrateLessMax = (int)(_constraint.MaxAudioBitrate720POneChannel - bitrateDelta - 1);

            //Act
            var adjustBitrate1 = _adjustmentAudioMetadata.AdjustAudioBitrate(size, 1, bitrateMoreOrEqualMax, samplerate);
            var adjustBitrate2 = _adjustmentAudioMetadata.AdjustAudioBitrate(size, 1, bitrateLessMax, samplerate);

            //Assert
            Assert.AreEqual(_constraint.MaxAudioBitrate720POneChannel, adjustBitrate1);
            Assert.AreEqual(bitrateLessMax, adjustBitrate2);
        }

        [TestMethod]
        public void AdjustAudioBitrateForMoreOrEqual720PTwoChannelTest()
        {
            //Arrange
            const uint sizeDelta = 76;
            const uint bitrateDelta = 887;
            const int samplerate = 12345;

            var size = (int)(_constraint.Size720P + sizeDelta);
            var bitrateMoreOrEqualMax = (int)(_constraint.MaxAudioBitrate720PTwoChannel + bitrateDelta);
            var bitrateLessMax = (int)(_constraint.MaxAudioBitrate720PTwoChannel - bitrateDelta - 1);

            //Act
            var adjustBitrate1 = _adjustmentAudioMetadata.AdjustAudioBitrate(size, 2, bitrateMoreOrEqualMax, samplerate);
            var adjustBitrate2 = _adjustmentAudioMetadata.AdjustAudioBitrate(size, 2, bitrateLessMax, samplerate);

            //Assert
            Assert.AreEqual(_constraint.MaxAudioBitrate720PTwoChannel, adjustBitrate1);
            Assert.AreEqual(bitrateLessMax, adjustBitrate2);
        }

        [TestMethod]
        public void AdjustAudioBitrateForMoreOrEqual720PSixChannelTest()
        {
            //Arrange
            const uint sizeDelta = 287;
            const uint bitrateDelta = 487;
            const int samplerate = 12345;

            var size = (int)(_constraint.Size720P + sizeDelta);
            var bitrateMoreOrEqualMax = (int)(_constraint.MaxAudioBitrate720PSixChannel + bitrateDelta);
            var bitrateLessMax = (int)(_constraint.MaxAudioBitrate720PSixChannel - bitrateDelta - 1);

            //Act
            var adjustBitrate1 = _adjustmentAudioMetadata.AdjustAudioBitrate(size, 6, bitrateMoreOrEqualMax, samplerate);
            var adjustBitrate2 = _adjustmentAudioMetadata.AdjustAudioBitrate(size, 6, bitrateLessMax, samplerate);

            //Assert
            Assert.AreEqual(_constraint.MaxAudioBitrate720PSixChannel, adjustBitrate1);
            Assert.AreEqual(bitrateLessMax, adjustBitrate2);
        }

        [TestMethod]
        public void AdjustAudioBitrateForLessThan720POneChannelTest()
        {
            //Arrange
            const uint sizeDelta = 26;
            const uint bitrateDelta = 1207;
            const int samplerate = 12345;

            var size = (int)(_constraint.Size720P - sizeDelta - 1);
            var bitrateMoreOrEqualMax = (int)(_constraint.DefaultAudioBitrateOneChannel + bitrateDelta);
            var bitrateLessMax = (int)(_constraint.DefaultAudioBitrateOneChannel - bitrateDelta - 1);

            //Act
            var adjustBitrate1 = _adjustmentAudioMetadata.AdjustAudioBitrate(size, 1, bitrateMoreOrEqualMax, samplerate);
            var adjustBitrate2 = _adjustmentAudioMetadata.AdjustAudioBitrate(size, 1, bitrateLessMax, samplerate);

            //Assert
            Assert.AreEqual(_constraint.DefaultAudioBitrateOneChannel, adjustBitrate1);
            Assert.AreEqual(bitrateLessMax, adjustBitrate2);
        }

        [TestMethod]
        public void AdjustAudioBitrateForLessThan720PTwoChannelTest()
        {
            //Arrange
            const uint sizeDelta = 76;
            const uint bitrateDelta = 887;
            const int samplerate = 12345;

            var size = (int)(_constraint.Size720P - sizeDelta - 1);
            var bitrateMoreOrEqualMax = (int)(_constraint.DefaultAudioBitrateTwoChannel + bitrateDelta);
            var bitrateLessMax = (int)(_constraint.DefaultAudioBitrateTwoChannel - bitrateDelta - 1);

            //Act
            var adjustBitrate1 = _adjustmentAudioMetadata.AdjustAudioBitrate(size, 2, bitrateMoreOrEqualMax, samplerate);
            var adjustBitrate2 = _adjustmentAudioMetadata.AdjustAudioBitrate(size, 2, bitrateLessMax, samplerate);

            //Assert
            Assert.AreEqual(_constraint.DefaultAudioBitrateTwoChannel, adjustBitrate1);
            Assert.AreEqual(bitrateLessMax, adjustBitrate2);
        }

        [TestMethod]
        public void AdjustAudioBitrateForLessThan720PSixChannelTest()
        {
            //Arrange
            const uint sizeDelta = 0;
            const uint bitrateDelta = 10;
            const int samplerate = 12345;

            var size = (int)(_constraint.Size720P - sizeDelta - 1);
            var bitrateMoreOrEqualMax = (int)(_constraint.DefaultAudioBitrateSixChannel + bitrateDelta);
            var bitrateLessMax = (int)(_constraint.DefaultAudioBitrateSixChannel - bitrateDelta - 1);

            //Act
            var adjustBitrate1 = _adjustmentAudioMetadata.AdjustAudioBitrate(size, 6, bitrateMoreOrEqualMax, samplerate);
            var adjustBitrate2 = _adjustmentAudioMetadata.AdjustAudioBitrate(size, 6, bitrateLessMax, samplerate);

            //Assert
            Assert.AreEqual(_constraint.DefaultAudioBitrateSixChannel, adjustBitrate1);
            Assert.AreEqual(bitrateLessMax, adjustBitrate2);
        }

        [TestMethod]
        public void AdjustAudioBitrateThrowExceptionTest()
        {
            //Arrange
            const int bitrate = 0;
            const int badChannel = 11;//not 1, 2 or 6
            var badBitrate = Math.Abs(bitrate) * -1;
            const int samplerate = 1234;

            //Act & Assert
            var bitrateException = ExceptionAssert.Throws<VideoFormatException>(() => _adjustmentAudioMetadata.AdjustAudioBitrate(123, 1, badBitrate, samplerate));
            var cannelException = ExceptionAssert.Throws<VideoFormatException>(() => _adjustmentAudioMetadata.AdjustAudioBitrate(123, badChannel, 65600, samplerate));

            //Assert
            Assert.AreEqual(ParamType.AudioBitrate, bitrateException.ParamType);
            Assert.AreEqual(ParamType.AudioChannel, cannelException.ParamType);
        }

        [TestMethod]
        public void AdjustAudioBitrateLessThanSampleRateTest()
        { 
            //Arrange
            const int size = 654;
            const int samplerate = 12345;
            const int channels = 2;
            const int bitrate = samplerate * channels - 1;
            
            //Act
            var bitrateException = ExceptionAssert.Throws<VideoFormatException>(() => _adjustmentAudioMetadata.AdjustAudioBitrate(size, channels, bitrate, samplerate));

            //Assert
            Assert.AreEqual(ParamType.AudioBitrate, bitrateException.ParamType);
        }
    }
}
