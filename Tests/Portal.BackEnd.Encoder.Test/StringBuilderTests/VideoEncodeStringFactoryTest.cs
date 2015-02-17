using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.BackEnd.Encoder.Ffmpeg;
using Portal.BackEnd.Encoder.StringBuilder;
using Portal.Domain.BackendContext.Constant;
using Portal.Domain.BackendContext.Entity;

namespace Portal.BackEnd.Encoder.Test.StringBuilderTests
{
    [TestClass]
    public class VideoEncodeStringFactoryTest
    {
        [TestMethod]
        public void GetVideoCodecLibTest()
        {
            //Arrange
            var videoEncodeStringFactory = new VideoEncodeStringFactory();

            //Act
            var mp4VideoCodec = videoEncodeStringFactory.GetVideoCodecLib(MetadataConstant.AvcCodec);
            var webmVideoCodec = videoEncodeStringFactory.GetVideoCodecLib(MetadataConstant.Vp8Codec);

            Assert.AreEqual(FfmpegConstant.AvcCodecLib, mp4VideoCodec);
            Assert.AreEqual(FfmpegConstant.Vp8CodecLib, webmVideoCodec);
        }

        [TestMethod]
        public void GetVideoCodecOptionTest()
        {
            //Arrange
            const string videoProfile = "videoProfile";

            var videoEncodeStringFactory = new VideoEncodeStringFactory();

            //Act
            var mp4Option = videoEncodeStringFactory.GetVideoCodecOption(MetadataConstant.AvcCodec, videoProfile);
            var webmOption = videoEncodeStringFactory.GetVideoCodecOption(MetadataConstant.Vp8Codec, videoProfile);

            Assert.AreEqual(String.Format("-profile:v {0}", videoProfile), mp4Option);
            Assert.AreEqual(String.Format("-quality good -cpu-used 5 -threads {0}", Environment.ProcessorCount - 1), webmOption);
        }
        
        [TestMethod]
        public void GetAudioCodecLibTest()
        {
            //Arrange
            var videoEncodeStringFactory = new VideoEncodeStringFactory();

            //Act
            var mp4AudioCodec = videoEncodeStringFactory.GetAudioCodecLib(MetadataConstant.AacCodec);
            var webmAudioCodec = videoEncodeStringFactory.GetAudioCodecLib(MetadataConstant.VorbisCodec);

            Assert.AreEqual(FfmpegConstant.AacCodecLib, mp4AudioCodec);
            Assert.AreEqual(FfmpegConstant.VorbisCodecLib, webmAudioCodec);
        }

        [TestMethod]
        public void GetContainerStringTest()
        {
            //Arrange
            var videoEncodeStringFactory = new VideoEncodeStringFactory();

            //Act
            var mp4String = videoEncodeStringFactory.GetContainerString(MetadataConstant.Mp4Container);
            var webmString = videoEncodeStringFactory.GetContainerString(MetadataConstant.WebmContainer);

            Assert.AreEqual(String.Format("-f {0}", FfmpegConstant.Mp4FfmpegContainer), mp4String);
            Assert.AreEqual(String.Format("-f {0}", FfmpegConstant.WebmFfmpegContainer), webmString);
        }

        [TestMethod]
        public void GetAudioStringTest()
        {
            //Arrange
            const string audioCodecLib = "audioCodec";
            const int audioBitrate = 325124;
            var videoEncodeStringFactory = new VideoEncodeStringFactory();

            //Act
            var audioString1 = videoEncodeStringFactory.GetAudioString(audioCodecLib, audioBitrate, true);
            var audioString2 = videoEncodeStringFactory.GetAudioString(audioCodecLib, audioBitrate, false);
            var audioString3 = videoEncodeStringFactory.GetAudioString(null, audioBitrate, false);

            Assert.AreEqual("-acodec copy", audioString1);
            Assert.AreEqual(String.Format("-acodec {0} -b:a {1}", audioCodecLib, audioBitrate), audioString2);
            Assert.AreEqual(String.Empty, audioString3);
        }
        
        [TestMethod]
        public void GetVideoStringTest()
        {
            //Arrange
            const string videoCodecLib = "videoCodecLib";
            const string videoCodecOption = "videoCodecOption";
            const string videoFilter = "videoFilter";

            const int videoBitrate = 325124;
            
            const double frameRate = 23.3;
            const int keyFrameRate = 10;
            const int width = 1243;
            const int height = 234;

            const string expectedString1 = "-vcodec copy";
            var expectedString2 = String.Format("-vcodec {0} -b:v {1} -r {2} -g {3} -s {4}x{5} {6} {7}",
                videoCodecLib,
                videoBitrate,
                frameRate,
                keyFrameRate,
                width,
                height,
                videoCodecOption,
                videoFilter);
            

            var videoEncodeStringFactory = new VideoEncodeStringFactory();

            //Act
            var videoString1 = videoEncodeStringFactory.GetVideoString(videoCodecLib, videoCodecOption, videoFilter, videoBitrate, frameRate, keyFrameRate, width, height, true);
            var videoString2 = videoEncodeStringFactory.GetVideoString(videoCodecLib, videoCodecOption, videoFilter, videoBitrate, frameRate, keyFrameRate, width, height, false);

            Assert.AreEqual(expectedString1, videoString1);
            Assert.AreEqual(expectedString2, videoString2);
        }
    }
}
