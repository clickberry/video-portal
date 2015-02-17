using System;
using System.IO;
using EncoderTest.Tmp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Domain;
using PortalEncoder;

namespace EncoderTest
{
    [TestClass]
    public class FfmpegServiceTest
    {
        [TestMethod]
        public void EncodedVideoMetadataTest()
        {
            //Arrange
            const string sourceFilePath = "source file path";
            const string destinationPath = "my path";
            const string destinationFileName = "my file";

            MetadataServiceConfigurator serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();
            var metadata = new VideoMediaInfo
                {
                    GeneralFormat = "MPEG-PS",
                    AudioBitRate = serviceConfigurator.AudioBps720P1Channel + 1,
                    AudioChannels = (int) AudioChannel.One,
                    AudioFormat = "AC-3",
                    AudioFormatProfile = null,
                    VideoBitRate = serviceConfigurator.VideoBps1920X1080 + 1,
                    VideoFormat = "MPEG Video",
                    VideoFrameRate = serviceConfigurator.MaxFps - 1,
                    VideoFormatSettingsGOP = String.Format("M=1, N={0}", serviceConfigurator.MaxKeyFrame + 1),
                    VideoFormatProfile = "High",
                    VideoWidth = 4096,
                    VideoHeight = 2304,
                };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var ffmpegService = new FfmpegService(metadataService, sourceFilePath, destinationPath, destinationFileName);

            //Act
            ffmpegService.GetStringForEncoder();

            //Assert
            Assert.AreEqual(ffmpegService.EncodedVideoData.Container, serviceConfigurator.Container);
            Assert.AreEqual(ffmpegService.EncodedVideoData.Width, metadata.VideoWidth);
            Assert.AreEqual(ffmpegService.EncodedVideoData.Height, metadata.VideoHeight);
            Assert.AreEqual(ffmpegService.EncodedVideoData.VideoCodec, serviceConfigurator.VideoCodec.Codec);
            Assert.AreEqual(ffmpegService.EncodedVideoData.AudioCodec, serviceConfigurator.AudioCodec.Codec);
        }

        [TestMethod]
        public void GetMp4StringFromIncorrectMetadataTest()
        {
            //Arrange
            const string sourceFilePath = "source file path";
            const string destinationPath = "my path";
            const string destinationFileName = "my file";

            string destinationFilePath = Path.Combine(destinationPath, destinationFileName);
            MetadataServiceConfigurator serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();
            var metadata = new VideoMediaInfo
                {
                    GeneralFormat = "MPEG-PS",
                    AudioBitRate = serviceConfigurator.AudioBps720P1Channel + 1,
                    AudioChannels = (int) AudioChannel.One,
                    AudioFormat = "AC-3",
                    AudioFormatProfile = null,
                    VideoBitRate = serviceConfigurator.VideoBps1920X1080 + 1,
                    VideoFormat = "MPEG Video",
                    VideoFrameRate = serviceConfigurator.MaxFps - 1,
                    VideoFormatSettingsGOP = String.Format("M=1, N={0}", serviceConfigurator.MaxKeyFrame + 1),
                    VideoFormatProfile = "High",
                    VideoWidth = 4096,
                    VideoHeight = 2304,
                };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegService(metadataService, sourceFilePath, destinationPath, destinationFileName);

            string ffmpegStr = "-i \"{0}\" -f {1} -vcodec {2} -b:v {3} -r {4} -g {5} -s {6}x{7} -profile {8} -acodec {9} -b:a {10} -quality good -cpu-used 0 -threads {11} -y \"{12}.{13}\"";
            ffmpegStr = String.Format(ffmpegStr,
                                      sourceFilePath,
                                      metadataService.ContainerForFfmpeg,
                                      metadataService.VideoCodecLib,
                                      serviceConfigurator.VideoBps1920X1080,
                                      serviceConfigurator.MaxFps - 1,
                                      serviceConfigurator.KeyFrame,
                                      metadata.VideoWidth,
                                      metadata.VideoHeight,
                                      serviceConfigurator.VideoCodec.DefaultProfile,
                                      metadataService.AudioCodecLib,
                                      serviceConfigurator.AudioBps720P1Channel,
                                      Environment.ProcessorCount,
                                      destinationFilePath,
                                      serviceConfigurator.FfmpegContainer);

            //Act
            string str = stringBuilder.GetStringForEncoder();

            //Assert
            Assert.AreEqual(ffmpegStr, str, true);
        }

        [TestMethod]
        public void GetWebMStringFromIncorrectMetadataTest()
        {
            //Arrange
            const string sourceFilePath = "source file path";
            const string destinationPath = "my path";
            const string destinationFileName = "my file";

            string destinationFilePath = Path.Combine(destinationPath, destinationFileName);
            MetadataServiceConfigurator serviceConfigurator = Factory.CreateWebMMetadataServiceConfigurator();
            var metadata = new VideoMediaInfo
                {
                    GeneralFormat = "MPEG-PS",
                    AudioBitRate = serviceConfigurator.AudioBps720P1Channel + 1,
                    AudioChannels = (int) AudioChannel.One,
                    AudioFormat = "AC-3",
                    AudioFormatProfile = null,
                    VideoBitRate = serviceConfigurator.VideoBps1920X1080 + 1,
                    VideoFormat = "MPEG Video",
                    VideoFrameRate = serviceConfigurator.MaxFps - 1,
                    VideoFormatSettingsGOP = String.Format("M=1, N={0}", serviceConfigurator.MaxKeyFrame + 1),
                    VideoFormatProfile = "High",
                    VideoWidth = 4096,
                    VideoHeight = 2304,
                };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegService(metadataService, sourceFilePath, destinationPath, destinationFileName);

            string ffmpegStr = "-i \"{0}\" -f {1} -vcodec {2} -b:v {3} -r {4} -g {5} -s {6}x{7} -acodec {8} -b:a {9} -quality good -cpu-used 0 -threads {10} -y \"{11}.{12}\"";
            ffmpegStr = String.Format(ffmpegStr,
                                      sourceFilePath,
                                      metadataService.ContainerForFfmpeg,
                                      metadataService.VideoCodecLib,
                                      serviceConfigurator.VideoBps1920X1080,
                                      serviceConfigurator.MaxFps - 1,
                                      serviceConfigurator.KeyFrame,
                                      metadata.VideoWidth,
                                      metadata.VideoHeight,
                                      metadataService.AudioCodecLib,
                                      serviceConfigurator.AudioBps720P1Channel,
                                      Environment.ProcessorCount,
                                      destinationFilePath,
                                      serviceConfigurator.FfmpegContainer);

            //Act
            string str = stringBuilder.GetStringForEncoder();

            //Assert
            Assert.AreEqual(ffmpegStr, str, true);
        }

        [TestMethod]
        public void GetMp4StringFromCorrectMetadataTest()
        {
            //Arrange
            const string sourceFilePath = "source file path";
            const string destinationPath = "my path";
            const string destinationFileName = "my file";

            string destinationFilePath = Path.Combine(destinationPath, destinationFileName);
            MetadataServiceConfigurator serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();
            var metadata = new VideoMediaInfo
                {
                    GeneralFormat = "MPEG-4",
                    AudioBitRate = 152000,
                    AudioChannels = (int) AudioChannel.Two,
                    AudioFormat = "AAC",
                    AudioFormatProfile = "LC",
                    VideoBitRate = 2500000,
                    VideoFormat = "AVC",
                    VideoFrameRate = 24,
                    VideoFormatSettingsGOP = String.Format("M=1, N={0}", serviceConfigurator.MaxKeyFrame),
                    VideoFormatProfile = "Baseline",
                    VideoWidth = 4096,
                    VideoHeight = 2304,
                };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegService(metadataService, sourceFilePath, destinationPath, destinationFileName);

            string ffmpegStr = "-i \"{0}\" -f mp4 -vcodec copy -acodec copy -quality good -cpu-used 0 -threads {1} -y \"{2}.{3}\"";
            ffmpegStr = String.Format(ffmpegStr,
                                      sourceFilePath,
                                      Environment.ProcessorCount,
                                      destinationFilePath,
                                      serviceConfigurator.FfmpegContainer);

            //Act
            string str = stringBuilder.GetStringForEncoder();

            //Assert
            Assert.AreEqual(ffmpegStr, str, true);
        }

        [TestMethod]
        public void GetWebMStringFromCorrectMetadataTest()
        {
            //Arrange
            const string sourceFilePath = "source file path";
            const string destinationPath = "my path";
            const string destinationFileName = "my file";

            string destinationFilePath = Path.Combine(destinationPath, destinationFileName);
            MetadataServiceConfigurator serviceConfigurator = Factory.CreateWebMMetadataServiceConfigurator();
            var metadata = new VideoMediaInfo
                {
                    GeneralFormat = "WebM",
                    AudioBitRate = 152000,
                    AudioChannels = (int) AudioChannel.Two,
                    AudioFormat = "Vorbis",
                    AudioFormatProfile = null,
                    VideoBitRate = 2500000,
                    VideoFormat = "VP8",
                    VideoFrameRate = 24,
                    VideoFormatSettingsGOP = String.Format("M=1, N={0}", serviceConfigurator.MaxKeyFrame),
                    VideoFormatProfile = null,
                    VideoWidth = 4096,
                    VideoHeight = 2304
                };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegService(metadataService, sourceFilePath, destinationPath, destinationFileName);

            string ffmpegStr = "-i \"{0}\" -f webm -vcodec copy -acodec copy -quality good -cpu-used 0 -threads {1} -y \"{2}.{3}\"";
            ffmpegStr = String.Format(ffmpegStr,
                                      sourceFilePath,
                                      Environment.ProcessorCount,
                                      destinationFilePath,
                                      serviceConfigurator.FfmpegContainer);

            //Act
            string str = stringBuilder.GetStringForEncoder();

            //Assert
            Assert.AreEqual(ffmpegStr, str, true);
        }

        [TestMethod]
        public void GetMp4StringFromMetadataWithMp3Test()
        {
            //Arrange
            const string sourceFilePath = "source file path";
            const string destinationPath = "my path";
            const string destinationFileName = "my file";

            string destinationFilePath = Path.Combine(destinationPath, destinationFileName);
            MetadataServiceConfigurator serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();
            var metadata = new VideoMediaInfo
                {
                    GeneralFormat = "MPEG-4",
                    AudioBitRate = 152000,
                    AudioChannels = (int) AudioChannel.Two,
                    AudioFormat = "MPEG Audio",
                    AudioFormatProfile = "Layer 3",
                    VideoBitRate = 2500000,
                    VideoFormat = "AVC",
                    VideoFrameRate = 24,
                    VideoFormatSettingsGOP = String.Format("M=1, N={0}", serviceConfigurator.MaxKeyFrame),
                    VideoFormatProfile = "Baseline",
                    VideoWidth = 4096,
                    VideoHeight = 2304
                };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegService(metadataService, sourceFilePath, destinationPath, destinationFileName);

            string ffmpegStr = "-i \"{0}\" -f mp4 -vcodec copy -acodec copy -quality good -cpu-used 0 -threads {1} -y \"{2}.{3}\"";
            ffmpegStr = String.Format(ffmpegStr,
                                      sourceFilePath,
                                      Environment.ProcessorCount,
                                      destinationFilePath,
                                      serviceConfigurator.FfmpegContainer);

            //Act
            string str = stringBuilder.GetStringForEncoder();

            //Assert
            Assert.AreEqual(ffmpegStr, str, true);
        }

        [TestMethod]
        public void GetMp4StringFromMetadataWithMp2Test()
        {
            //Arrange
            const string sourceFilePath = "source file path";
            const string destinationPath = "my path";
            const string destinationFileName = "my file";

            string destinationFilePath = Path.Combine(destinationPath, destinationFileName);
            MetadataServiceConfigurator serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();
            var metadata = new VideoMediaInfo
                {
                    GeneralFormat = "MPEG-4",
                    AudioBitRate = 152000,
                    AudioChannels = (int) AudioChannel.Two,
                    AudioFormat = "MPEG Audio",
                    AudioFormatProfile = "Layer 2",
                    VideoBitRate = 2500000,
                    VideoFormat = "AVC",
                    VideoFrameRate = 24,
                    VideoFormatSettingsGOP = String.Format("M=1, N={0}", serviceConfigurator.MaxKeyFrame),
                    VideoFormatProfile = "Baseline",
                    VideoWidth = 4096,
                    VideoHeight = 2304
                };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegService(metadataService, sourceFilePath, destinationPath, destinationFileName);

            string ffmpegStr = "-i \"{0}\" -f mp4 -vcodec copy -acodec {1} -b:a {2} -quality good -cpu-used 0 -threads {3} -y \"{4}.{5}\"";
            ffmpegStr = String.Format(ffmpegStr,
                                      sourceFilePath,
                                      metadataService.AudioCodecLib,
                                      metadata.AudioBitRate,
                                      Environment.ProcessorCount,
                                      destinationFilePath,
                                      serviceConfigurator.FfmpegContainer);

            //Act
            string str = stringBuilder.GetStringForEncoder();

            //Assert
            Assert.AreEqual(ffmpegStr, str, true);
        }

        [TestMethod]
        public void GetMp4StringWithoutAudioTest()
        {
            //Arrange
            const string sourceFilePath = "source file path";
            const string destinationPath = "my path";
            const string destinationFileName = "my file";

            string destinationFilePath = Path.Combine(destinationPath, destinationFileName);
            MetadataServiceConfigurator serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();
            var metadata = new VideoMediaInfo
                {
                    GeneralFormat = "MPEG-4",
                    AudioBitRate = 0,
                    AudioChannels = (int) AudioChannel.None,
                    AudioFormat = null,
                    AudioFormatProfile = null,
                    VideoBitRate = 2500000,
                    VideoFormat = "AVC",
                    VideoFrameRate = 24,
                    VideoFormatSettingsGOP = String.Format("M=1, N={0}", serviceConfigurator.MaxKeyFrame),
                    VideoFormatProfile = "Baseline",
                    VideoWidth = 4096,
                    VideoHeight = 2304
                };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegService(metadataService, sourceFilePath, destinationPath, destinationFileName);

            string ffmpegStr = "-i \"{0}\" -f mp4 -vcodec copy -quality good -cpu-used 0 -threads {1} -y \"{2}.{3}\"";
            ffmpegStr = String.Format(ffmpegStr,
                                      sourceFilePath,
                                      Environment.ProcessorCount,
                                      destinationFilePath,
                                      serviceConfigurator.FfmpegContainer);

            //Act
            string str = stringBuilder.GetStringForEncoder();

            //Assert
            Assert.AreEqual(ffmpegStr, str, true);
        }

        [TestMethod]
        public void GetMjpegStringWithNumFrameLessDurationTest()
        {
            //Arrange
            const string sourceFilePath = "source file path";
            const string destinationPath = "my path";
            const string destinationFileName = "my file";

            string destinationFilePath = Path.Combine(destinationPath, destinationFileName);
            MetadataServiceConfigurator serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();

            var metadata = new VideoMediaInfo
                {
                    VideoDuration = 2000,
                    VideoWidth = 4096,
                    VideoHeight = 2304
                };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegService(metadataService, sourceFilePath, destinationPath, destinationFileName);

            string ffmpegStr = "-i \"{0}\" -f image2 -ss {1} -frames:v 1 -y \"{2}.jpg\"";
            ffmpegStr = String.Format(ffmpegStr,
                                      sourceFilePath,
                                      (int) serviceConfigurator.ScreenshotTime.TotalSeconds,
                                      destinationFilePath);

            //Act
            string str = stringBuilder.GetStringForScreenshot();

            //Assert
            Assert.AreEqual(ffmpegStr, str);
        }

        [TestMethod]
        public void GetMjpegStringWithNumFrameMoreDurationTest()
        {
            //Arrange
            const string sourceFilePath = "source file path";
            const string destinationPath = "my path";
            const string destinationFileName = "my file";

            string destinationFilePath = Path.Combine(destinationPath, destinationFileName);
            MetadataServiceConfigurator serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();

            var metadata = new VideoMediaInfo
                {
                    VideoDuration = (int) serviceConfigurator.ScreenshotTime.TotalMilliseconds - 1,
                    VideoWidth = 4096,
                    VideoHeight = 2304
                };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegService(metadataService, sourceFilePath, destinationPath, destinationFileName);

            string ffmpegStr = "-i \"{0}\" -f image2 -ss {1} -frames:v 1 -y \"{2}.jpg\"";
            ffmpegStr = String.Format(ffmpegStr,
                                      sourceFilePath,
                                      0,
                                      destinationFilePath);

            //Act
            string str = stringBuilder.GetStringForScreenshot();

            //Assert
            Assert.AreEqual(ffmpegStr, str);
        }
    }
}