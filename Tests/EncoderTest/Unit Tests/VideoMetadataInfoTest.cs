using System;
using System.Linq.Expressions;
using IntegrationTestInfrastructure.Encoder;
using MSTestExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.Domain;
using PortalEncoder;
using PortalEncoder.Exceptions;

namespace EncoderTest
{
    [TestClass]
    public class VideoMetadataInfoTest
    {
        private VideoMetadataInfo _metadataInfo;
        private Mock<IMediaInfo> _mockMediaInfo;

        [TestInitialize]
        public void Initialize()
        {
            _mockMediaInfo = new Mock<IMediaInfo>();
            _metadataInfo = new VideoMetadataInfo(_mockMediaInfo.Object);
        }

        [TestMethod]
        public void MediaInfoOpenFileFailTest()
        {
            //Arrange
            int ii = -1;
            _mockMediaInfo.Setup(m => m.Open(It.IsAny<string>())).Returns<string>((s) => ii++);

            //Act & Assert
            CustomAssert.IsThrown<MediaFormatException>(() => _metadataInfo.GetMetadata(It.IsAny<string>()));
            CustomAssert.IsThrown<MediaFormatException>(() => _metadataInfo.GetMetadata(It.IsAny<string>()));
        }

        [TestMethod]
        public void MediaInfoOpenFileSuccessTest()
        {
            //Arrange
            int ii = 1;
            _mockMediaInfo.Setup(m => m.Open(It.IsAny<string>())).Returns<string>((s) => ii++);

            //Act & Assert
            CustomAssert.IsNotThrown(() => _metadataInfo.GetMetadata(It.IsAny<string>()));
            CustomAssert.IsNotThrown(() => _metadataInfo.GetMetadata(It.IsAny<string>()));
        }

        [TestMethod]
        public void GetMetadataTest()
        {
            //Arrange
            const string h264Profile = "Main";
            const string h264Level = "3.1";

            const int width = 100;
            const int height = 100;
            const string videoContainer = "MP4";
            const string videoCodec = "AVC";
            const string audioCodec = "MPEG Audio";
            const int videoBps = 63997;
            const string videoKeyFrame = "M=1, N=30";
            const double videoFps = 25.001;
            const int audioBps = 63997;
            string videoFormatProfile = String.Format("{0}@L{1}", h264Profile, h264Level);
            const long fileSize = 1293887332;
            const int duration = 13454643;
            const AudioChannel audioChannel = AudioChannel.Six;
            const string audioProfile = "Layer 3";

            const string filePath = "myFile";

            SetupAllMediaInfoParams(width, height, videoContainer, videoCodec, videoBps, videoKeyFrame, videoFps, audioCodec, audioBps, fileSize, duration, audioChannel, audioProfile,
                                    videoFormatProfile);
            _mockMediaInfo.Setup(m => m.Open(It.IsAny<string>())).Returns(1);

            //Act
            VideoMediaInfo metadata = _metadataInfo.GetMetadata(filePath);

            //Assert
            Assert.AreEqual(width, metadata.VideoWidth);
            Assert.AreEqual(height, metadata.VideoHeight);
            Assert.AreEqual(videoContainer, metadata.GeneralFormat);
            Assert.AreEqual(videoCodec, metadata.VideoFormat);
            Assert.AreEqual(audioCodec, metadata.AudioFormat);
            Assert.AreEqual(videoBps, metadata.VideoBitRate);
            Assert.AreEqual(videoKeyFrame, metadata.VideoFormatSettingsGOP);
            Assert.AreEqual(videoFps, metadata.VideoFrameRate);
            Assert.AreEqual(audioBps, metadata.AudioBitRate);
            Assert.AreEqual(h264Profile, metadata.VideoFormatProfile);
            Assert.AreEqual(fileSize, metadata.GeneralFileSize);
            Assert.AreEqual(duration, metadata.VideoDuration);
            Assert.AreEqual((int) audioChannel, metadata.AudioChannels);
            Assert.AreEqual(audioProfile, metadata.AudioFormatProfile);
        }

        [TestMethod]
        public void GetMetadataErroVideoProfileTest()
        {
            //Arrange
            const string filePath = "my file path";
            const string videoProfile = "profile";
            const string profileLevel = "level";
            int count = 0;
            var testValues = new[] {null, videoProfile, String.Format("{0}{1}{2}", videoProfile, "@", profileLevel), String.Format("{0}{1}", videoProfile, "@L")};

            _mockMediaInfo.Setup((m) => m.Open(It.IsAny<string>())).Returns(1);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.VideoFormatProfile)).Returns<string, string>((s1, s2) =>
                {
                    string str = testValues[count];
                    count++;
                    return str;
                });

            //Act
            VideoMediaInfo metadata1 = _metadataInfo.GetMetadata(filePath);
            VideoMediaInfo metadata2 = _metadataInfo.GetMetadata(filePath);
            VideoMediaInfo metadata3 = _metadataInfo.GetMetadata(filePath);
            VideoMediaInfo metadata4 = _metadataInfo.GetMetadata(filePath);

            //Assert
            Assert.AreEqual(null, metadata1.VideoFormatProfile);
            Assert.AreEqual(videoProfile, metadata2.VideoFormatProfile);
            Assert.AreEqual(videoProfile, metadata3.VideoFormatProfile);
            Assert.AreEqual(videoProfile, metadata4.VideoFormatProfile);
        }

        private VideoMediaInfo GetVideoMetadata(int width, int height, string container, string videoCodec, int videoBps, string videoKeyFrame, double videoFps, string audioCodec, int audioBps,
                                                long fileSize, int duration, AudioChannel audioChannel, string audioProfile, string videoFormatProfile = null)
        {
            var metadata = new VideoMediaInfo
                {
                    AudioBitRate = audioBps,
                    AudioFormat = audioCodec,
                    AudioChannels = (int) audioChannel,
                    AudioFormatProfile = audioProfile,
                    GeneralFormat = container,
                    VideoDuration = duration,
                    GeneralFileSize = fileSize,
                    VideoHeight = height,
                    VideoBitRate = videoBps,
                    VideoFormat = videoCodec,
                    VideoFrameRate = videoFps,
                    VideoFormatSettingsGOP = videoKeyFrame,
                    VideoFormatProfile = videoFormatProfile,
                    VideoWidth = width
                };

            return metadata;
        }

        private void SetupAllMediaInfoParams(int width, int height, string container, string videoCodec, int videoBps, string videoKeyFrame, double videoFps, string audioCodec, int audioBps,
                                             long fileSize, int duration, AudioChannel audioChannel, string audioProfile, string videoFormatProfile = null)
        {
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.VideoWidth)).Returns(width.ToString);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.VideHeight)).Returns(height.ToString);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.Container)).Returns(container);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.VideoCodec)).Returns(videoCodec);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.VideoBps)).Returns(videoBps.ToString);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.VideoKeyFrame)).Returns(videoKeyFrame);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.VideoFps)).Returns(videoFps.ToString);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.VideoFormatProfile)).Returns(videoFormatProfile);

            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.FileSize)).Returns(fileSize.ToString);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.Duration)).Returns(duration.ToString);

            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.AudioCodec)).Returns(audioCodec);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.AudioBps)).Returns(audioBps.ToString);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.AudioChannel)).Returns(((int) audioChannel).ToString);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.AudioProfile)).Returns(audioProfile.ToString);
        }

        private Expression<Func<IMediaInfo, string>> SetupMediaInfoParam(string paramName)
        {
            return m => m.Option(MediaInfoParams.Option, It.Is<string>(s => s == paramName));
        }
    }
}