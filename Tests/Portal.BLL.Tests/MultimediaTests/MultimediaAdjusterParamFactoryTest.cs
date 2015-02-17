using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Multimedia.Factory;
using Portal.Domain.EncoderContext;
using Portal.Exceptions.Multimedia;
using TestExtension;

namespace Portal.BLL.Tests.MultimediaTests
{
    [TestClass]
    public class MultimediaAdjusterParamFactoryTest
    {
        private const int KeyFrameRate = 12;
        private Mock<IVideoMetadata> _videoMetadata;
        private Mock<IVideoMetadata> _videoMetadata0;
        private Mock<IVideoMetadata> _videoMetadata1;
        private Mock<IVideoMetadata> _videoMetadata2;

        [TestInitialize]
        public void Initialize()
        {
            _videoMetadata = new Mock<IVideoMetadata>();
            _videoMetadata0 = new Mock<IVideoMetadata>();
            _videoMetadata1 = new Mock<IVideoMetadata>();
            _videoMetadata2 = new Mock<IVideoMetadata>();

            _videoMetadata0.Setup(p => p.AudioStreamsCount).Returns(0);
            _videoMetadata0.Setup(p => p.VideoStreamsCount).Returns(0);
            _videoMetadata1.Setup(p => p.AudioStreamsCount).Returns(1);
            _videoMetadata1.Setup(p => p.VideoStreamsCount).Returns(1);
            _videoMetadata2.Setup(p => p.AudioStreamsCount).Returns(2);
            _videoMetadata2.Setup(p => p.VideoStreamsCount).Returns(2);

            _videoMetadata.Setup(p => p.GeneralFormat).Returns("mediaContainer");
            _videoMetadata.Setup(p => p.VideoFormat).Returns("videoCodec");
            _videoMetadata.Setup(p => p.VideoFormatProfile).Returns("videoProfile");
            _videoMetadata.Setup(p => p.VideoBitRate).Returns(43664);
            _videoMetadata.Setup(p => p.VideoFrameRate).Returns(23);
            _videoMetadata.Setup(p => p.VideoFrameRateMode).Returns("frameRateMode");
            _videoMetadata.Setup(p => p.VideoFormatSettingsGOP).Returns(String.Format("M=1, N={0}", KeyFrameRate));
            _videoMetadata.Setup(p => p.VideoRotation).Returns(90);
            _videoMetadata.Setup(p => p.AudioFormat).Returns("audioCodec");
            _videoMetadata.Setup(p => p.AudioBitRate).Returns(3421);
            _videoMetadata.Setup(p => p.AudioChannels).Returns(2);
            _videoMetadata.Setup(p => p.VideoStreamsCount).Returns(1);
            _videoMetadata.Setup(p => p.AudioStreamsCount).Returns(1);
            _videoMetadata.Setup(p => p.AudioSamplingRate).Returns(12343);
        }

        [TestMethod]
        public void CreateVideoParamTest()
        {
            //Arrange
            var factory = new MultimediaAdjusterParamFactory();
            
            //Act
            var videoAdjusterParam = factory.CreateVideoParam(_videoMetadata.Object);

            //Assert
            Assert.AreEqual(_videoMetadata.Object.GeneralFormat, videoAdjusterParam.MediaContainer);
            Assert.AreEqual(_videoMetadata.Object.VideoFormat, videoAdjusterParam.VideoCodec);
            Assert.AreEqual(_videoMetadata.Object.VideoFormatProfile, videoAdjusterParam.VideoProfile);
            Assert.AreEqual(_videoMetadata.Object.VideoBitRate, videoAdjusterParam.VideoBitrate);
            Assert.AreEqual(_videoMetadata.Object.VideoFrameRate, videoAdjusterParam.FrameRate);
            Assert.AreEqual(_videoMetadata.Object.VideoFrameRateMode, videoAdjusterParam.FrameRateMode);
            Assert.AreEqual(KeyFrameRate, videoAdjusterParam.KeyFrameRate);
            Assert.AreEqual(_videoMetadata.Object.VideoRotation, videoAdjusterParam.VideoRotation);
        }

        [TestMethod]
        public void CreateVideoParamGopIsNullTest()
        {
            //Arrange
            var factory = new MultimediaAdjusterParamFactory();

            _videoMetadata.Setup(p=>p.VideoFormatSettingsGOP).Returns((string) null);

            //Act
            var videoParam = factory.CreateVideoParam(_videoMetadata.Object);

            //Assert
            Assert.AreEqual(0, videoParam.KeyFrameRate);
        }

        [TestMethod]
        public void CreateVideoParamThrowAggregateExceptionTest()
        {
            //Arrange
            var factory = new MultimediaAdjusterParamFactory();

            //Act & Assert
            var exception0 = ExceptionAssert.Throws<AggregateException>(() => factory.CreateVideoParam(_videoMetadata0.Object));
            var exception2 = ExceptionAssert.Throws<AggregateException>(() => factory.CreateVideoParam(_videoMetadata2.Object));
            ExceptionAssert.NotThrows<AggregateException>(() => factory.CreateVideoParam(_videoMetadata1.Object));

            //Assert
            Assert.IsTrue(exception0.InnerExceptions.OfType<VideoFormatException>().Any(p=>p.ParamType==ParamType.VideoStreamCount));
            Assert.IsTrue(exception2.InnerExceptions.OfType<VideoFormatException>().Any(p => p.ParamType == ParamType.VideoStreamCount));
        }

        [TestMethod]
        public void CreateAudioParamTest()
        {
            //Arrange
            var factory = new MultimediaAdjusterParamFactory();

            //Act
            var audioParam = factory.CreateAudioParam(_videoMetadata.Object);

            //Assert
            Assert.AreEqual(_videoMetadata.Object.AudioFormat, audioParam.AudioCodec);
            Assert.AreEqual(_videoMetadata.Object.AudioBitRate, audioParam.AudioBitrate);
            Assert.AreEqual(_videoMetadata.Object.AudioChannels, audioParam.AudioChannels);
            Assert.AreEqual(_videoMetadata.Object.AudioSamplingRate, audioParam.AudioSampleRate);
            Assert.IsTrue(audioParam.IsExistAudioStream);
        }

        [TestMethod]
        public void CreateAudioParamThrowAggregateExceptionTest()
        {
            //Arrange
            var factory = new MultimediaAdjusterParamFactory();

            //Act & Assert
            var exception2 = ExceptionAssert.Throws<AggregateException>(() => factory.CreateAudioParam(_videoMetadata2.Object));
            ExceptionAssert.NotThrows<AggregateException>(() => factory.CreateAudioParam(_videoMetadata0.Object));
            ExceptionAssert.NotThrows<AggregateException>(() => factory.CreateAudioParam(_videoMetadata1.Object));

            //Assert
            Assert.IsTrue(exception2.InnerExceptions.OfType<VideoFormatException>().Any(p => p.ParamType == ParamType.AudioStreamCount));
        }
    }
}
