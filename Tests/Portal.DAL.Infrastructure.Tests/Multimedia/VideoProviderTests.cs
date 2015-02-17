using System;
using System.Threading.Tasks;
using MediaInfoLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.DAL.Infrastructure.Multimedia;

namespace Portal.DAL.Infrastructure.Tests.Multimedia
{
    [TestClass]
    public class VideoProviderTests
    {
        [TestMethod]
        public async Task TestRetrivingInformationFromMediaFile()
        {
            // Arrange
            var mediaInfoReader = new VideoMetadataParser(new Lazy<Task<IMediaInfo>>(MediaInfo.Create));

            // Act
            var metadata = await mediaInfoReader.GetVideoMetadata(@"D:\TrunkJiraPortal\Tests\Portal.DAL.Infrastructure.Tests\Multimedia\video.mp4");

            // Assert
            Assert.IsNotNull(metadata);
            
            // Assert General
            Assert.AreEqual("MPEG-4", metadata.GeneralFormat);
            Assert.AreEqual("Base Media", metadata.GeneralFormatProfile);
            Assert.AreEqual("isom", metadata.GeneralCodecID);
            Assert.AreEqual(2074467L, metadata.GeneralFileSize);
            Assert.AreEqual(1741L, metadata.GeneralDuration);
            Assert.AreEqual(9532301, metadata.GeneralOverallBitRate);
            Assert.AreEqual(new DateTime(2012, 06, 26, 08, 06, 20), metadata.GeneralEncodedDate);
            Assert.AreEqual(new DateTime(2012, 06, 26, 08, 06, 20), metadata.GeneralTaggedDate);

            // Assert Video
            Assert.AreEqual(1, metadata.VideoStreamsCount);
            Assert.AreEqual("AVC", metadata.VideoFormat);
            Assert.AreEqual("Advanced Video Codec", metadata.VideoFormatInfo);
            Assert.AreEqual("Baseline@L4.0", metadata.VideoFormatProfile);
            Assert.AreEqual("No", metadata.VideoFormatSettingsCABAC);
            Assert.AreEqual(1, metadata.VideoFormatSettingsRefFrames);
            Assert.AreEqual("avc1", metadata.VideoCodecID);
            Assert.AreEqual("Advanced Video Coding", metadata.VideoCodecIDInfo);
            Assert.AreEqual(1415L, metadata.VideoDuration);
            Assert.AreEqual(11561789, metadata.VideoBitRate);
            Assert.AreEqual(1280, metadata.VideoWidth);
            Assert.AreEqual(720, metadata.VideoHeight);
            Assert.AreEqual("16:9", metadata.VideoDisplayAspectRatio);
            Assert.AreEqual("VFR", metadata.VideoFrameRateMode);
            Assert.AreEqual(24.031, metadata.VideoFrameRate);
            Assert.AreEqual(23.095, metadata.VideoFrameRateMinimum);
            Assert.AreEqual(25.028, metadata.VideoFrameRateMaximum);
            Assert.AreEqual("YUV", metadata.VideoColorSpace);
            Assert.AreEqual("4:2:0", metadata.VideoChromaSubsampling);
            Assert.AreEqual(8, metadata.VideoBitDepth);
            Assert.AreEqual("Progressive", metadata.VideoScanType);
            Assert.AreEqual(0.522, metadata.VideoBitsPixelByFrame);
            Assert.AreEqual(2044759L, metadata.VideoStreamSize);
            Assert.AreEqual("VideoHandle", metadata.VideoTitle);
            Assert.AreEqual("en", metadata.VideoLanguage);
            Assert.AreEqual(new DateTime(2012, 06, 26, 08, 06, 20), metadata.VideoEncodedDate);
            Assert.AreEqual(new DateTime(2012, 06, 26, 08, 06, 20), metadata.VideoTaggedDate);
            Assert.AreEqual(0, metadata.VideoRotation);

            // Assert Audio
            Assert.AreEqual(1, metadata.AudioStreamsCount);
            Assert.AreEqual("AAC", metadata.AudioFormat);
            Assert.AreEqual("Advanced Audio Codec", metadata.AudioFormatInfo);
            Assert.AreEqual("LC", metadata.AudioFormatProfile);
            Assert.AreEqual("40", metadata.AudioCodecID);
            Assert.AreEqual(1741L, metadata.AudioDuration);
            Assert.AreEqual("CBR", metadata.AudioBitRateMode);
            Assert.AreEqual(128032, metadata.AudioBitRate);
            Assert.AreEqual(96000, metadata.AudioBitRateNominal);
            Assert.AreEqual(2, metadata.AudioChannels);
            Assert.AreEqual("Front: L R", metadata.AudioChannelPositions);
            Assert.AreEqual(44100, metadata.AudioSamplingRate);
            Assert.AreEqual("Lossy", metadata.AudioCompressionMode);
            Assert.AreEqual(27863L, metadata.AudioStreamSize);
            Assert.AreEqual("SoundHandle", metadata.AudioTitle);
            Assert.AreEqual("en", metadata.AudioLanguage);
            Assert.AreEqual(new DateTime(2012, 06, 26, 08, 06, 20), metadata.AudioEncodedDate);
            Assert.AreEqual(new DateTime(2012, 06, 26, 08, 06, 20), metadata.AudioTaggedDate);
        }
    }
}
