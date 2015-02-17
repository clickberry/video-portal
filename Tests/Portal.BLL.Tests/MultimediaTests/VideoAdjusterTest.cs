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
    public class VideoAdjusterTest
    {
        private VideoAdjuster _videoAdjuster;
        private VideoAdjusterParam _videoAdjusterParam;
        private Mock<IAdjustmentVideoMetadata> _adjustmentVideoParam;

        [TestInitialize]
        public void Initialize()
        {
            _videoAdjusterParam = new VideoAdjusterParam()
                {
                    MediaContainer = "mediaContainer",
                    VideoCodec = "videoCodec",
                    VideoBitrate = 12345,
                    VideoProfile = "videoProfile",
                    FrameRate = 25,
                    KeyFrameRate = 10,
                    FrameRateMode = "frameRateMode",
                    VideoRotation = 270
                };

            _adjustmentVideoParam = new Mock<IAdjustmentVideoMetadata>();
            _videoAdjuster = new VideoAdjuster(_adjustmentVideoParam.Object);
        }

        [TestMethod]
        public void AdjustVideoParamTest()
        {
            //Arrange
            const string newMediaContainer = "newMediaContainer";
            const string videoCodec = "adjVideoCodec";
            const int bitrate = 2345;
            const int adjWidth = 65;
            const int adjHeight = 98;
            const int rotWidth = 84326;
            const int rotHeight = 123457;
            const string profile = "profile";
            const double frameRate = 12;
            const double videoRotation = 270;
            const int keyFrameRate = 10;
            const int size = 2356;

            var videoSize = new Mock<IVideoSize>();
            videoSize.Setup(m => m.Square()).Returns(size);
            videoSize.Setup(p => p.Width).Returns(543);
            videoSize.Setup(p => p.Height).Returns(324);

            _adjustmentVideoParam.Setup(m => m.AdjustMediaContainer(_videoAdjusterParam.MediaContainer, newMediaContainer)).Returns(newMediaContainer);
            _adjustmentVideoParam.Setup(m => m.AdjustVideoCodec(newMediaContainer, _videoAdjusterParam.VideoCodec)).Returns(videoCodec);
            _adjustmentVideoParam.Setup(m => m.AdjustVideoBitrate(size, _videoAdjusterParam.VideoBitrate)).Returns(bitrate);
            _adjustmentVideoParam.Setup(m => m.AdjustVideoWidth(videoSize.Object.Width)).Returns(adjWidth);
            _adjustmentVideoParam.Setup(m => m.AdjustVideoHeight(videoSize.Object.Height)).Returns(adjHeight);
            _adjustmentVideoParam.Setup(m => m.AdjustVideoProfile(newMediaContainer, _videoAdjusterParam.VideoProfile)).Returns(profile);
            _adjustmentVideoParam.Setup(m => m.AdjustFrameRate(_videoAdjusterParam.FrameRate, _videoAdjusterParam.FrameRateMode)).Returns(frameRate);
            _adjustmentVideoParam.Setup(m => m.AdjustKeyFrameRate(_videoAdjusterParam.KeyFrameRate)).Returns(keyFrameRate);
            _adjustmentVideoParam.Setup(m => m.AdjustVideoRotateSize(adjWidth, adjHeight, videoRotation)).Returns(new VideoSize(rotWidth, rotHeight));

            //Act
            var videoParam = _videoAdjuster.AdjustVideoParam(_videoAdjusterParam, newMediaContainer, videoSize.Object);

            //Assert
            Assert.AreEqual(newMediaContainer, videoParam.MediaContainer);
            Assert.AreEqual(videoCodec, videoParam.VideoCodec);
            Assert.AreEqual(bitrate, videoParam.VideoBitrate);
            Assert.AreEqual(rotWidth, videoParam.VideoWidth);
            Assert.AreEqual(rotHeight, videoParam.VideoHeight);
            Assert.AreEqual(profile, videoParam.VideoProfile);
            Assert.AreEqual(frameRate, videoParam.FrameRate);
            Assert.AreEqual(keyFrameRate, videoParam.KeyFrameRate);
            Assert.AreEqual(_videoAdjusterParam.VideoRotation, videoParam.VideoRotation);
        }

        [TestMethod]
        public void AdjustVideoParamThrowAggregateException()
        {
            //Arrange
            const string mediaContainer = "mediaContainer";
            var videoSize = new Mock<IVideoSize>();

            _adjustmentVideoParam.Setup(m => m.AdjustMediaContainer(It.IsAny<string>(), It.IsAny<string>())).Throws(new VideoFormatException(ParamType.MediaContainer));
            _adjustmentVideoParam.Setup(m => m.AdjustVideoCodec(It.IsAny<string>(), It.IsAny<string>())).Throws(new VideoFormatException(ParamType.VideoCodec));
            _adjustmentVideoParam.Setup(m => m.AdjustVideoBitrate(It.IsAny<int>(), It.IsAny<int>())).Throws(new VideoFormatException(ParamType.VideoBitRate));
            _adjustmentVideoParam.Setup(m => m.AdjustVideoWidth(It.IsAny<int>())).Throws(new VideoFormatException(ParamType.Width));
            _adjustmentVideoParam.Setup(m => m.AdjustVideoHeight(It.IsAny<int>())).Throws(new VideoFormatException(ParamType.Height)); 
            _adjustmentVideoParam.Setup(m => m.AdjustFrameRate(It.IsAny<double>(), It.IsAny<string>())).Throws(new VideoFormatException(ParamType.FrameRate));

            //Act & Assert
            var exception = ExceptionAssert.Throws<AggregateException>(() => _videoAdjuster.AdjustVideoParam(_videoAdjusterParam, mediaContainer, videoSize.Object));

            //Assert
            Assert.IsTrue(exception.InnerExceptions.OfType<VideoFormatException>().Any(p => p.ParamType == ParamType.MediaContainer));
            Assert.IsTrue(exception.InnerExceptions.OfType<VideoFormatException>().Any(p => p.ParamType == ParamType.VideoCodec));
            Assert.IsTrue(exception.InnerExceptions.OfType<VideoFormatException>().Any(p => p.ParamType == ParamType.VideoBitRate));
            Assert.IsTrue(exception.InnerExceptions.OfType<VideoFormatException>().Any(p => p.ParamType == ParamType.Width));
            Assert.IsTrue(exception.InnerExceptions.OfType<VideoFormatException>().Any(p => p.ParamType == ParamType.Height));
            Assert.IsTrue(exception.InnerExceptions.OfType<VideoFormatException>().Any(p => p.ParamType == ParamType.FrameRate));
        }
    }
}
