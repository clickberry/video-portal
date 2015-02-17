using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Ffmpeg;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.StringBuilder;
using Portal.Domain.BackendContext.Constant;
using Portal.Domain.BackendContext.Entity;

namespace Portal.BackEnd.Encoder.Test.StringBuilderTests
{
    [TestClass]
    public class VideoEncodeStringBuilderTest
    {
        private const string DestinationPath = "destinationPath";
        private const string InputVideoLocalFilePath = "video url";
        private const string VideoProfile = "videoProfile";
        private const double FrameRate = 12.34;
        private const string MediaContatiner = "mediaContatiner";
        private const string VideoCodec = "videoCodec";
        private const double VideoRotation = 270.0;
        private const int VideoBitrate = 1000000;
        private const int Width = 123;
        private const int Height = 345;
        private const int KeyFrame = 10;

        private const string AudioCodec = "audioCodec";
        private const int AudioBitrate = 2352;

        private const bool IsAudioCopy = true;
        private const bool IsVideoCopy = false;

        private VideoEncodeData _ecodeData;

        [TestInitialize]
        public void Initialize()
        {
            _ecodeData = new VideoEncodeData()
            {
                AudioParam = new AudioParam()
                {
                    AudioCodec = AudioCodec,
                    AudioBitrate = AudioBitrate
                },
                VideoParam = new VideoParam()
                {
                    MediaContainer = MediaContatiner,
                    VideoCodec = VideoCodec,
                    VideoProfile = VideoProfile,
                    VideoBitrate = VideoBitrate,
                    FrameRate = FrameRate,
                    KeyFrameRate = KeyFrame,
                    VideoWidth = Width,
                    VideoHeight = Height,
                    VideoRotation = VideoRotation
                },
                IsAudioCopy = IsAudioCopy,
                IsVideoCopy = IsVideoCopy
            };
        }


        [TestMethod]
        public void GetVideoFfmpegStringTest()
        {
            //Arrange
            const string containerString = "containerString";
            const string videoCodecLib = "videoCodecLib";
            const string videoCodecOption = "videoCodecOption";
            const string videoFilter = "videoFilter";
            const string audioCodecLib = "audioCodecLib";

            const string videoString = "videoString";
            const string audioString = "audioString";
           
            var videoEncodeStringFactory = new Mock<IVideoEncodeStringFactory>();
            var tempFileManager = new Mock<ITempFileManager>();

            var expectedStr = String.Format(@"-i ""{0}"" {1} {2} {3} -y ""{4}""",
                                           InputVideoLocalFilePath,
                                           containerString,
                                           videoString,
                                           audioString,
                                           DestinationPath);

            videoEncodeStringFactory.Setup(m => m.GetAudioCodecLib(AudioCodec)).Returns(audioCodecLib);
            videoEncodeStringFactory.Setup(m => m.GetAudioString(audioCodecLib, AudioBitrate, IsAudioCopy)).Returns(audioString);
            videoEncodeStringFactory.Setup(m => m.GetContainerString(MediaContatiner)).Returns(containerString);
            videoEncodeStringFactory.Setup(m => m.GetVideoCodecLib(VideoCodec)).Returns(videoCodecLib);
            videoEncodeStringFactory.Setup(m => m.GetVideoCodecOption(VideoCodec, VideoProfile)).Returns(videoCodecOption);
            videoEncodeStringFactory.Setup(m => m.GetVideoFilter((int)VideoRotation)).Returns(videoFilter);
            videoEncodeStringFactory.Setup(m => m.GetVideoString(videoCodecLib, videoCodecOption, videoFilter, VideoBitrate, FrameRate, KeyFrame, Width, Height, IsVideoCopy)).Returns(videoString);
            
            tempFileManager.Setup(m => m.GetOriginalTempFilePath()).Returns(InputVideoLocalFilePath);
            tempFileManager.Setup(m => m.GetEncodingTempFilePath()).Returns(DestinationPath);
            
            var stringBuilder = new VideoEncodeStringBuilder(_ecodeData, videoEncodeStringFactory.Object, tempFileManager.Object);

            //Act
            var actualStr = stringBuilder.GetFfmpegArguments();

            //Assert
            Assert.AreEqual(expectedStr, actualStr);
        }
    }
}


