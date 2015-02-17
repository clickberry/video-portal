using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.Calculator;
using Portal.BLL.Concrete.Multimedia.Comparator;
using Portal.Domain.BackendContext.Constant;
using Portal.Domain.BackendContext.Entity;

namespace Portal.BLL.Tests.MultimediaTests
{
    [TestClass]
    public class ComparatorTest
    {
        private Comparator _comparator;
        private VideoAdjusterParam _videoAdjusterParam;
        private VideoParam _videoParam;
        private AudioAdjusterParam _audioAdjusterParam;
        private AudioParam _audioParam;
        private VideoSize _videoSize;

        [TestInitialize]
        public void Initialize()
        {
            const string mediaContainer = "mediaContainer";
            const string videoCodec = "videoCodec";
            const string videoProfile = "videoProfile";
            const int videoBitrate = 10000;
            const int width = 150;
            const int height = 100;
            const double frameRate = 25;
            const int keyFrameRate = 10;

            const string audioCodec = "audioCodec";
            const int audioBitrate = 500;

            _comparator = new Comparator();

            _videoSize = new VideoSize(width, height);

            _videoAdjusterParam = new VideoAdjusterParam()
                              {
                                  MediaContainer = mediaContainer,
                                  VideoCodec = videoCodec,
                                  VideoProfile = videoProfile,
                                  VideoBitrate = videoBitrate,
                                  FrameRate = frameRate,
                                  KeyFrameRate = keyFrameRate
                              };

            _videoParam = new VideoParam()
                              {
                                  MediaContainer = mediaContainer,
                                  VideoCodec = videoCodec,
                                  VideoProfile = videoProfile,
                                  VideoBitrate = videoBitrate,
                                  VideoWidth = width,
                                  VideoHeight = height,
                                  FrameRate = frameRate,
                                  KeyFrameRate = keyFrameRate
                              };
            
            _audioAdjusterParam = new AudioAdjusterParam()
                               {
                                   AudioCodec = audioCodec,
                                   AudioBitrate = audioBitrate
                               };

            _audioParam = new AudioParam()
                               {
                                   AudioCodec = audioCodec,
                                   AudioBitrate = audioBitrate
                               };
        }

        [TestMethod]
        public void VideoParamCompareEqualsTest()
        {
            //Arrange
            const string newContainer = "newContainer";

            //Act
            var result = _comparator.VideoParamCompare(_videoAdjusterParam, _videoParam, newContainer, _videoSize);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void VideoParamCompareWithDifferentMediaContainerTest()
        {
            //Arrange
            const string newContainer = "newContainer";
            _videoAdjusterParam.MediaContainer = "mediaContainer1";
            _videoParam.MediaContainer = "mediaContainer2";

            //Act
            var result = _comparator.VideoParamCompare(_videoAdjusterParam, _videoParam, newContainer, _videoSize);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void VideoParamCompareWithDifferentVideoCodecTest()
        {
            //Arrange
            const string newContainer = "newContainer";
            _videoAdjusterParam.VideoCodec = "VideoCodec1";
            _videoParam.VideoCodec = "VideoCodec2";

            //Act
            var result = _comparator.VideoParamCompare(_videoAdjusterParam, _videoParam, newContainer, _videoSize);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void VideoParamCompareWithDifferentVideoProfileForMp4Test()
        {
            //Arrange
            _videoAdjusterParam.VideoProfile = "VideoProfile1";
            _videoParam.VideoProfile = "VideoProfile2";

            //Act
            var result = _comparator.VideoParamCompare(_videoAdjusterParam, _videoParam, MetadataConstant.Mp4Container, _videoSize);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void VideoParamCompareWithDifferentVideoProfileForWebMTest()
        {
            //Arrange
            _videoAdjusterParam.VideoProfile = "VideoProfile1";
            _videoParam.VideoProfile = "VideoProfile2";

            //Act
            var result = _comparator.VideoParamCompare(_videoAdjusterParam, _videoParam, MetadataConstant.WebmContainer, _videoSize);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void VideoParamCompareWithDifferentVideoBitrateTest()
        {
            //Arrange
            const string newContainer = "newContainer";
            _videoAdjusterParam.VideoBitrate = 123;
            _videoParam.VideoBitrate = 456;

            //Act
            var result = _comparator.VideoParamCompare(_videoAdjusterParam, _videoParam, newContainer, _videoSize);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void VideoParamCompareWithDifferentVideoWidthTest()
        {
            //Arrange
            const string newContainer = "newContainer";
            _videoSize = new VideoSize(234, _videoParam.VideoHeight);
            _videoParam.VideoWidth = 456;

            //Act
            var result = _comparator.VideoParamCompare(_videoAdjusterParam, _videoParam, newContainer, _videoSize);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void VideoParamCompareWithDifferentVideoHeightTest()
        {
            //Arrange
            const string newContainer = "newContainer";
            _videoSize=new VideoSize(_videoParam.VideoWidth, 543);
            _videoParam.VideoHeight = 456;

            //Act
            var result = _comparator.VideoParamCompare(_videoAdjusterParam, _videoParam, newContainer, _videoSize);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void VideoParamCompareWithDifferentVideoFrameRateTest()
        {
            //Arrange
            const string newContainer = "newContainer";
            _videoAdjusterParam.FrameRate = 123;
            _videoParam.FrameRate = 456;

            //Act
            var result = _comparator.VideoParamCompare(_videoAdjusterParam, _videoParam, newContainer, _videoSize);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void VideoParamCompareWithDifferentVideoKeyFrameRateTest()
        {
            //Arrange
            const string newContainer = "newContainer";
            _videoAdjusterParam.KeyFrameRate = 123;
            _videoParam.KeyFrameRate = 456;

            //Act
            var result = _comparator.VideoParamCompare(_videoAdjusterParam, _videoParam, newContainer, _videoSize);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AudioParamCompareEqualsTest()
        {
            //Act
            var result = _comparator.AudioParamCompare(_audioAdjusterParam, _audioParam);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AudioParamCompareWithDifferentAudioCodecTest()
        {
            //Arrange
            _audioAdjusterParam.AudioCodec = "AudioCodec1";
            _audioParam.AudioCodec = "AudioCodec2";

            //Act
            var result = _comparator.AudioParamCompare(_audioAdjusterParam, _audioParam);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AudioParamCompareWithDifferentAudioBitrateTest()
        {
            //Arrange
            _audioAdjusterParam.AudioBitrate = 123;
            _audioParam.AudioBitrate = 456;

            //Act
            var result = _comparator.AudioParamCompare(_audioAdjusterParam, _audioParam);

            //Assert
            Assert.IsFalse(result);
        }
    }
}
