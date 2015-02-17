using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Repository.Azure.Infrastructure;
using Portal.Repository.Project;

namespace Portal.Repository.Tests.Infrastructure
{
    [TestClass]
    public class MediaInfoReaderTest
    {
        [TestMethod]
        public void TestRetrivingInformationFromMediaFile()
        {
            // Arrange
            var mediaInfoReader = new MediaInfoReader();

            // Act
            IDictionary<Enum, object> result = mediaInfoReader.GetInformation("video.mp4");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);

            // Assert General
            Assert.AreEqual("MPEG-4", result[VideoParameterType.GeneralFormat]);
            Assert.AreEqual("Base Media", result[VideoParameterType.GeneralFormatProfile]);
            Assert.AreEqual("isom", result[VideoParameterType.GeneralCodecID]);
            Assert.AreEqual(2074467L, result[VideoParameterType.GeneralFileSize]);
            Assert.AreEqual(1741L, result[VideoParameterType.GeneralDuration]);
            Assert.AreEqual(9532301, result[VideoParameterType.GeneralOverallBitRate]);
            Assert.AreEqual(new DateTime(2012, 06, 26, 08, 06, 20), result[VideoParameterType.GeneralEncodedDate]);
            Assert.AreEqual(new DateTime(2012, 06, 26, 08, 06, 20), result[VideoParameterType.GeneralTaggedDate]);

            // Assert Video
            Assert.AreEqual(1, result[VideoParameterType.VideoID]);
            Assert.AreEqual("AVC", result[VideoParameterType.VideoFormat]);
            Assert.AreEqual("Advanced Video Codec", result[VideoParameterType.VideoFormatInfo]);
            Assert.AreEqual("Baseline@L4.0", result[VideoParameterType.VideoFormatProfile]);
            Assert.AreEqual("No", result[VideoParameterType.VideoFormatSettingsCABAC]);
            Assert.AreEqual(1, result[VideoParameterType.VideoFormatSettingsRefFrames]);
            Assert.AreEqual("avc1", result[VideoParameterType.VideoCodecID]);
            Assert.AreEqual("Advanced Video Coding", result[VideoParameterType.VideoCodecIDInfo]);
            Assert.AreEqual(1415L, result[VideoParameterType.VideoDuration]);
            Assert.AreEqual(11561789, result[VideoParameterType.VideoBitRate]);
            Assert.AreEqual(1280, result[VideoParameterType.VideoWidth]);
            Assert.AreEqual(720, result[VideoParameterType.VideoHeight]);
            Assert.AreEqual("16:9", result[VideoParameterType.VideoDisplayAspectRatio]);
            Assert.AreEqual("VFR", result[VideoParameterType.VideoFrameRateMode]);
            Assert.AreEqual(24.031, result[VideoParameterType.VideoFrameRate]);
            Assert.AreEqual(23.095, result[VideoParameterType.VideoFrameRateMinimum]);
            Assert.AreEqual(25.028, result[VideoParameterType.VideoFrameRateMaximum]);
            Assert.AreEqual("YUV", result[VideoParameterType.VideoColorSpace]);
            Assert.AreEqual("4:2:0", result[VideoParameterType.VideoChromaSubsampling]);
            Assert.AreEqual(8, result[VideoParameterType.VideoBitDepth]);
            Assert.AreEqual("Progressive", result[VideoParameterType.VideoScanType]);
            Assert.AreEqual(0.522, result[VideoParameterType.VideoBitsPixelByFrame]);
            Assert.AreEqual(2044759L, result[VideoParameterType.VideoStreamSize]);
            Assert.AreEqual("VideoHandle", result[VideoParameterType.VideoTitle]);
            Assert.AreEqual("en", result[VideoParameterType.VideoLanguage]);
            Assert.AreEqual(new DateTime(2012, 06, 26, 08, 06, 20), result[VideoParameterType.VideoEncodedDate]);
            Assert.AreEqual(new DateTime(2012, 06, 26, 08, 06, 20), result[VideoParameterType.VideoTaggedDate]);

            // Assert Audio
            Assert.AreEqual(2, result[VideoParameterType.AudioID]);
            Assert.AreEqual("AAC", result[VideoParameterType.AudioFormat]);
            Assert.AreEqual("Advanced Audio Codec", result[VideoParameterType.AudioFormatInfo]);
            Assert.AreEqual("LC", result[VideoParameterType.AudioFormatProfile]);
            Assert.AreEqual("40", result[VideoParameterType.AudioCodecID]);
            Assert.AreEqual(1741L, result[VideoParameterType.AudioDuration]);
            Assert.AreEqual("CBR", result[VideoParameterType.AudioBitRateMode]);
            Assert.AreEqual(128032, result[VideoParameterType.AudioBitRate]);
            Assert.AreEqual(96000, result[VideoParameterType.AudioBitRateNominal]);
            Assert.AreEqual(2, result[VideoParameterType.AudioChannels]);
            Assert.AreEqual("Front: L R", result[VideoParameterType.AudioChannelPositions]);
            Assert.AreEqual(44100, result[VideoParameterType.AudioSamplingRate]);
            Assert.AreEqual("Lossy", result[VideoParameterType.AudioCompressionMode]);
            Assert.AreEqual(27863L, result[VideoParameterType.AudioStreamSize]);
            Assert.AreEqual("SoundHandle", result[VideoParameterType.AudioTitle]);
            Assert.AreEqual("en", result[VideoParameterType.AudioLanguage]);
            Assert.AreEqual(new DateTime(2012, 06, 26, 08, 06, 20), result[VideoParameterType.AudioEncodedDate]);
            Assert.AreEqual(new DateTime(2012, 06, 26, 08, 06, 20), result[VideoParameterType.AudioTaggedDate]);
        }
    }
}