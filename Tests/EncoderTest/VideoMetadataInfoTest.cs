using System;
using System.Linq.Expressions;
using Encoder;
using Encoder.Exceptions;
using Encoder.Interfaces;
using MSTestExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EncoderTest
{
    [TestClass]
    public class VideoMetadataInfoTest
    {
        private Mock<IMediaInfo> _mockMediaInfo;
        private VideoMetadataInfo _metadataInfo;

        [TestInitialize]
        public void Initialize()
        {
            _mockMediaInfo=new Mock<IMediaInfo>();
            _metadataInfo=new VideoMetadataInfo(_mockMediaInfo.Object);
        }

        [TestMethod]
        public void MediaInfoOpenFileFailTest()
        {
            //Arrange
            var ii = -1;
            _mockMediaInfo.Setup(m => m.Open(It.IsAny<string>())).Returns<string>((s) => ii++);

            //Act & Assert
            CustomAssert.IsThrown<MediaFormatException>(() => _metadataInfo.GetMetadata(It.IsAny<string>()));
            CustomAssert.IsThrown<MediaFormatException>(() => _metadataInfo.GetMetadata(It.IsAny<string>()));
        }

        [TestMethod]
        public void MediaInfoOpenFileSuccessTest()
        {
            //Arrange
            var ii = 1;
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
            const int videoKeyFrame = 30;
            const double videoFps = 25.001;
            const int audioBps = 63997;
            var videoFormatProfile = String.Format("{0}@L{1}", h264Profile, h264Level);
            const long fileSize = 1293887332;
            const int duration = 13454643;
            const AudioChannel audioChannel = AudioChannel.Six;
            const string audioProfile = "Layer 3";

            const string filePath = "myFile";

            SetupAllMediaInfoParams(width, height, videoContainer, videoCodec, videoBps, videoKeyFrame, videoFps, audioCodec, audioBps, fileSize, duration, audioChannel,audioProfile, videoFormatProfile);
            _mockMediaInfo.Setup(m => m.Open(It.IsAny<string>())).Returns(1);

            //Act
            var metadata = _metadataInfo.GetMetadata(filePath);

            //Assert
            Assert.AreEqual(width, metadata.Width);
            Assert.AreEqual(height, metadata.Height);
            Assert.AreEqual(videoContainer, metadata.Container);
            Assert.AreEqual(videoCodec, metadata.VideoCodec);
            Assert.AreEqual(audioCodec, metadata.AudioCodec);
            Assert.AreEqual(videoBps, metadata.VideoBps);
            Assert.AreEqual(videoKeyFrame, metadata.VideoKeyFrame);
            Assert.AreEqual(videoFps, metadata.VideoFps);
            Assert.AreEqual(audioBps, metadata.AudioBps);
            Assert.AreEqual(h264Profile, metadata.VideoProfile);
            Assert.AreEqual(fileSize, metadata.FileSize);
            Assert.AreEqual(duration, metadata.Duration);
            Assert.AreEqual(audioChannel, metadata.AudioChannel);
            Assert.AreEqual(audioProfile, metadata.AudioProfile);
            Assert.AreEqual(filePath, metadata.FilePath);
        }

        [TestMethod]
        public void GetMetadataErroVideoProfileTest()
        {
            //Arrange
            const string filePath = "my file path";
            const string videoProfile = "profile";
            const string profileLevel = "level";
            var count = 0;
            var testValues = new string[] { null, videoProfile, String.Format("{0}{1}{2}", videoProfile, "@", profileLevel), String.Format("{0}{1}", videoProfile, "@L") };

            _mockMediaInfo.Setup((m) => m.Open(It.IsAny<string>())).Returns(1);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.VideoFormatProfile)).Returns<string, string>((s1,s2)=>
                                                                                                              {
                                                                                                                  var str = testValues[count];
                                                                                                                  count++;
                                                                                                                  return str;
                                                                                                              });

            //Act
            var metadata1 = _metadataInfo.GetMetadata(filePath);
            var metadata2 = _metadataInfo.GetMetadata(filePath);
            var metadata3 = _metadataInfo.GetMetadata(filePath);
            var metadata4 = _metadataInfo.GetMetadata(filePath);

            //Assert
            Assert.AreEqual(null, metadata1.VideoProfile);
            Assert.AreEqual(videoProfile, metadata2.VideoProfile);
            Assert.AreEqual(videoProfile, metadata3.VideoProfile);
            Assert.AreEqual(videoProfile, metadata4.VideoProfile);
        }
        
        private VideoMetadata GetVideoMetadata(int width, int height, string container, string videoCodec, int videoBps, int videoKeyFrame, double videoFps, string audioCodec, int audioBps, long fileSize, int duration, AudioChannel audioChannel,string audioProfile, string videoFormatProfile = null)
        {
            var metadata = new VideoMetadata()
                               {
                                   AudioBps = audioBps,
                                   AudioCodec = audioCodec,
                                   AudioChannel=audioChannel,
                                   AudioProfile=audioProfile,
                                   Container = container,
                                   Duration = duration,
                                   FileSize = fileSize,
                                   Height = height,
                                   VideoBps = videoBps,
                                   VideoCodec = videoCodec,
                                   VideoFps = videoFps,
                                   VideoKeyFrame = videoKeyFrame,
                                   VideoProfile = videoFormatProfile,
                                   Width = width
                               };

            return metadata;
        }

        private void SetupAllMediaInfoParams(int width, int height, string container, string videoCodec, int videoBps, int videoKeyFrame, double videoFps, string audioCodec, int audioBps, long fileSize, int duration, AudioChannel audioChannel,string audioProfile, string videoFormatProfile = null)
        {
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.VideoWidth)).Returns(width.ToString);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.VideHeight)).Returns(height.ToString);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.Container)).Returns(container);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.VideoCodec)).Returns(videoCodec);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.VideoBps)).Returns(videoBps.ToString);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.VideoKeyFrame)).Returns(videoKeyFrame.ToString);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.VideoFps)).Returns(videoFps.ToString);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.VideoFormatProfile)).Returns(videoFormatProfile);

            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.FileSize)).Returns(fileSize.ToString);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.Duration)).Returns(duration.ToString);

            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.AudioCodec)).Returns(audioCodec);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.AudioBps)).Returns(audioBps.ToString);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.AudioChannel)).Returns(((int)audioChannel).ToString);
            _mockMediaInfo.Setup(SetupMediaInfoParam(MediaInfoParams.AudioProfile)).Returns(audioProfile.ToString);
        }

        private Expression<Func<IMediaInfo, string>> SetupMediaInfoParam(string paramName)
        {
            return m => m.Option(MediaInfoParams.Option, It.Is<string>(s => s == paramName));
        }
    }
    
}
