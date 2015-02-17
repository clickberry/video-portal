using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.Adjusters;
using Portal.BLL.Concrete.Multimedia.AdjustmentParameters;
using Portal.BLL.Concrete.Multimedia.Calculator;
using Portal.Exceptions.Multimedia;
using TestExtension;

namespace Portal.BLL.Tests.MultimediaTests
{
    [TestClass]
    public class AudioAdjusterTest
    {
        private AudioAdjuster _audioAdjuster;
        private Mock<IAdjustmentAudioMetadata> _adjustmentAudioParam;
        private AudioAdjusterParam _audioParam;

        [TestInitialize]
        public void Initialize()
        {
            _audioParam = new AudioAdjusterParam()
                              {
                                  AudioBitrate = 567,
                                  AudioCodec = "audioCodec",
                                  AudioChannels=2,
                                  IsExistAudioStream=true,
                                  AudioSampleRate = 123
                              };

            _adjustmentAudioParam = new Mock<IAdjustmentAudioMetadata>();
            _audioAdjuster = new AudioAdjuster(_adjustmentAudioParam.Object);
        }

        [TestMethod]
        public void AdjustAudioParamTest()
        {
            //Arrange
            const string mediaContainer = "mediaContainer";
            const string audioCodec = "adjAudioCodec";
            const int audioBitrate = 890;
            const int size = 3424;

            var videoSize = new Mock<IVideoSize>();
            videoSize.Setup(m => m.Square()).Returns(size);
            
            _adjustmentAudioParam.Setup(m => m.AdjustAudioCodec(mediaContainer, _audioParam.AudioCodec)).Returns(audioCodec);
            _adjustmentAudioParam.Setup(m => m.AdjustAudioBitrate(size, _audioParam.AudioChannels, _audioParam.AudioBitrate, _audioParam.AudioSampleRate)).Returns(audioBitrate);

            //Act
            var audioParam = _audioAdjuster.AdjustAudioParam(_audioParam, mediaContainer, videoSize.Object);

            //Assert
            Assert.AreEqual(audioCodec, audioParam.AudioCodec);
            Assert.AreEqual(audioBitrate,audioParam.AudioBitrate);
        }

        [TestMethod]
        public void AdjustAudioParamThrowAggregateExceptionTest()
        {
            //Arrange
            var videoSize = new Mock<IVideoSize>();

            _adjustmentAudioParam.Setup(m => m.AdjustAudioCodec(It.IsAny<string>(), It.IsAny<string>())).Throws(new VideoFormatException(ParamType.AudioCodec));
            _adjustmentAudioParam.Setup(m => m.AdjustAudioBitrate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new VideoFormatException(ParamType.AudioBitrate));

            //Act & Asseert
            var exception = ExceptionAssert.Throws<AggregateException>(() => _audioAdjuster.AdjustAudioParam(_audioParam, It.IsAny<string>(), videoSize.Object));

            //Assert
            Assert.IsTrue(exception.InnerExceptions.OfType<VideoFormatException>().Any(p => p.ParamType == ParamType.AudioCodec));
            Assert.IsTrue(exception.InnerExceptions.OfType<VideoFormatException>().Any(p => p.ParamType == ParamType.AudioBitrate));
        }

        [TestMethod]
        public void AdjustAudioParamWhenAudioStreamNotExistTest()
        {
            //Arrange
            const string mediaContainer = "mediaContainer";
            const string audioCodec = "adjAudioCodec";
            const int audioBitrate = 890;

            var videoSize = new Mock<IVideoSize>();

            _adjustmentAudioParam.Setup(m => m.AdjustAudioCodec(It.IsAny<string>(), It.IsAny<string>())).Returns(audioCodec);
            _adjustmentAudioParam.Setup(m => m.AdjustAudioBitrate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(audioBitrate);

            _audioParam.IsExistAudioStream = false;

            //Act
            var audioParam = _audioAdjuster.AdjustAudioParam(_audioParam, mediaContainer, videoSize.Object);

            //Assert
            Assert.IsNull(audioParam.AudioCodec);
            Assert.AreEqual(0, audioParam.AudioBitrate);
        }
    }
}