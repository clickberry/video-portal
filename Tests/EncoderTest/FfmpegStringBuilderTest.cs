using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Encoder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EncoderTest.Tmp;

namespace EncoderTest
{
    [TestClass]
    public class FfmpegStringBuilderTest
    {
        [TestMethod]
        public void GetMp4StringFromIncorrectMetadataTest()
        {
            //Arrange
            const string destinationFilePath = "destination Path";
            var serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();
            var metadata = new VideoMetadata()
                               {
                                   Container = "MPEG-PS",
                                   AudioBps = serviceConfigurator.AudioBps720P1Channel+1,
                                   AudioChannel = AudioChannel.One,
                                   AudioCodec = "AC-3",
                                   AudioProfile = null,
                                   VideoBps = serviceConfigurator.VideoBps1920X1080+1,
                                   VideoCodec = "MPEG Video",
                                   VideoFps = serviceConfigurator.MaxFps - 1,
                                   VideoKeyFrame = serviceConfigurator.MaxKeyFrame+1,
                                   VideoProfile = "High",
                                   Width = 4096,
                                   Height = 2304,
                                   FilePath = "source file path"
                               };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegStringBuilder(metadataService, destinationFilePath);

            var ffmpegStr = "-i \"{0}\" -f {1} -vcodec {2} -b:v {3} -r {4} -g {5} -profile {6} -acodec {7} -b:a {8} -y \"{9}.{10}\"";
            ffmpegStr = String.Format(ffmpegStr,
                metadata.FilePath,
                metadataService.ContainerForFfmpeg,
                metadataService.VideoCodecLib,
                 serviceConfigurator.VideoBps1920X1080,
                 serviceConfigurator.MaxFps - 1,
                serviceConfigurator.KeyFrame,
                serviceConfigurator.VideoCodec.DefaultProfile,
                metadataService.AudioCodecLib,
                serviceConfigurator.AudioBps720P1Channel,
                destinationFilePath,
                serviceConfigurator.FfmpegContainer);
            
            //Act
            var str = stringBuilder.GetStringForEncoder();

            //Assert
            Assert.AreEqual(ffmpegStr, str, true);
        }

        [TestMethod]
        public void GetWebMStringFromIncorrectMetadataTest()
        {
            //Arrange
            const string destinationFilePath = "destination Path";
            var serviceConfigurator = Factory.CreateWebMMetadataServiceConfigurator();
            var metadata = new VideoMetadata()
                               {
                                   Container = "MPEG-PS",
                                   AudioBps = serviceConfigurator.AudioBps720P1Channel + 1,
                                   AudioChannel = AudioChannel.One,
                                   AudioCodec = "AC-3",
                                   AudioProfile = null,
                                   VideoBps = serviceConfigurator.VideoBps1920X1080 + 1,
                                   VideoCodec = "MPEG Video",
                                   VideoFps = serviceConfigurator.MaxFps - 1,
                                   VideoKeyFrame = serviceConfigurator.MaxKeyFrame + 1,
                                   VideoProfile = "High",
                                   Width = 4096,
                                   Height = 2304,
                                   FilePath = "source file path"
                               };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegStringBuilder(metadataService, destinationFilePath);

            var ffmpegStr = "-i \"{0}\" -f {1} -vcodec {2} -b:v {3} -r {4} -g {5} -acodec {6} -b:a {7} -y \"{8}.{9}\"";
            ffmpegStr = String.Format(ffmpegStr,
                metadata.FilePath,
                metadataService.ContainerForFfmpeg,
                metadataService.VideoCodecLib,
                 serviceConfigurator.VideoBps1920X1080,
                 serviceConfigurator.MaxFps - 1,
                serviceConfigurator.KeyFrame,
                metadataService.AudioCodecLib,
                serviceConfigurator.AudioBps720P1Channel,
                destinationFilePath,
                serviceConfigurator.FfmpegContainer);

            //Act
            var str = stringBuilder.GetStringForEncoder();

            //Assert
            Assert.AreEqual(ffmpegStr, str, true);
        }

        [TestMethod]
        public void GetMp4StringFromCorrectMetadataTest()
        {
            //Arrange
            const string destinationFilePath = "destination Path";
            var serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();
            var metadata = new VideoMetadata()
                               {
                                   Container = "MPEG-4",
                                   AudioBps = 152000,
                                   AudioChannel = AudioChannel.Two,
                                   AudioCodec = "AAC",
                                   AudioProfile = "LC",
                                   VideoBps = 2500000,
                                   VideoCodec = "AVC",
                                   VideoFps = 24,
                                   VideoKeyFrame = 2,
                                   VideoProfile = "Baseline",
                                   Width = 4096,
                                   Height = 2304,
                                   FilePath = "source file path"
                               };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegStringBuilder(metadataService, destinationFilePath);

            var ffmpegStr = "-i \"{0}\" -f mp4 -vcodec copy -acodec copy -y \"{1}.{2}\"";
            ffmpegStr = String.Format(ffmpegStr,
                metadata.FilePath,
                destinationFilePath,
                serviceConfigurator.FfmpegContainer);

            //Act
            var str = stringBuilder.GetStringForEncoder();

            //Assert
            Assert.AreEqual(ffmpegStr, str, true);
        }

        [TestMethod]
        public void GetWebMStringFromCorrectMetadataTest()
        {
            //Arrange
            const string destinationFilePath = "destination Path";
            var serviceConfigurator = Factory.CreateWebMMetadataServiceConfigurator();
            var metadata = new VideoMetadata()
                               {
                                   Container = "WebM",
                                   AudioBps = 152000,
                                   AudioChannel = AudioChannel.Two,
                                   AudioCodec = "Vorbis",
                                   AudioProfile = null,
                                   VideoBps = 2500000,
                                   VideoCodec = "VP8",
                                   VideoFps = 24,
                                   VideoKeyFrame = 2,
                                   VideoProfile = null,
                                   Width = 4096,
                                   Height = 2304,
                                   FilePath = "source file path"
                               };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegStringBuilder(metadataService, destinationFilePath);

            var ffmpegStr = "-i \"{0}\" -f webm -vcodec copy -acodec copy -y \"{1}.{2}\"";
            ffmpegStr = String.Format(ffmpegStr,
                metadata.FilePath,
                destinationFilePath,
                serviceConfigurator.FfmpegContainer);

            //Act
            var str = stringBuilder.GetStringForEncoder();

            //Assert
            Assert.AreEqual(ffmpegStr, str, true);
        }
        
        [TestMethod]
        public void GetMp4StringFromMetadataWithMp3Test()
        {
            //Arrange
            const string destinationFilePath = "destination Path";
            var serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();
            var metadata = new VideoMetadata()
                               {
                                   Container = "MPEG-4",
                                   AudioBps = 152000,
                                   AudioChannel = AudioChannel.Two,
                                   AudioCodec = "MPEG Audio",
                                   AudioProfile = "Layer 3",
                                   VideoBps = 2500000,
                                   VideoCodec = "AVC",
                                   VideoFps = 24,
                                   VideoKeyFrame = 2,
                                   VideoProfile = "Baseline",
                                   Width = 4096,
                                   Height = 2304,
                                   FilePath = "source file path"
                               };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegStringBuilder(metadataService, destinationFilePath);

            var ffmpegStr = "-i \"{0}\" -f mp4 -vcodec copy -acodec copy -y \"{1}.{2}\"";
            ffmpegStr = String.Format(ffmpegStr,
                 metadata.FilePath,
                 destinationFilePath,
                serviceConfigurator.FfmpegContainer);

            //Act
            var str = stringBuilder.GetStringForEncoder();

            //Assert
            Assert.AreEqual(ffmpegStr, str, true);
        }

        [TestMethod]
        public void GetMp4StringFromMetadataWithMp2Test()
        {
            //Arrange
            const string destinationFilePath = "destination Path";
            var serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();
            var metadata = new VideoMetadata()
                               {
                                   Container = "MPEG-4",
                                   AudioBps = 152000,
                                   AudioChannel = AudioChannel.Two,
                                   AudioCodec = "MPEG Audio",
                                   AudioProfile = "Layer 2",
                                   VideoBps = 2500000,
                                   VideoCodec = "AVC",
                                   VideoFps = 24,
                                   VideoKeyFrame = 2,
                                   VideoProfile = "Baseline",
                                   Width = 4096,
                                   Height = 2304,
                                   FilePath = "source file path"
                               };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegStringBuilder(metadataService, destinationFilePath);

            var ffmpegStr = "-i \"{0}\" -f mp4 -vcodec copy -acodec {1} -b:a {2} -y \"{3}.{4}\"";
            ffmpegStr = String.Format(ffmpegStr,
                metadata.FilePath,
                metadataService.AudioCodecLib,
                metadata.AudioBps,
                destinationFilePath,
                serviceConfigurator.FfmpegContainer);

            //Act
            var str = stringBuilder.GetStringForEncoder();

            //Assert
            Assert.AreEqual(ffmpegStr, str, true);
        }

        [TestMethod]
        public void GetMp4StringWithoutAudioTest()
        {
            //Arrange
            const string destinationFilePath = "destination Path";
            var serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();
            var metadata = new VideoMetadata()
                               {
                                   Container = "MPEG-4",
                                   AudioBps = 0,
                                   AudioChannel = AudioChannel.None,
                                   AudioCodec = null,
                                   AudioProfile = null,
                                   VideoBps = 2500000,
                                   VideoCodec = "AVC",
                                   VideoFps = 24,
                                   VideoKeyFrame = 2,
                                   VideoProfile = "Baseline",
                                   Width = 4096,
                                   Height = 2304,
                                   FilePath = "source file path"
                               };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegStringBuilder(metadataService, destinationFilePath);

            var ffmpegStr = "-i \"{0}\" -f mp4 -vcodec copy -y \"{1}.{2}\"";
            ffmpegStr = String.Format(ffmpegStr,
                metadata.FilePath,
                destinationFilePath,
                serviceConfigurator.FfmpegContainer);

            //Act
            var str = stringBuilder.GetStringForEncoder();

            //Assert
            Assert.AreEqual(ffmpegStr, str, true);
        }

        [TestMethod]
        public void GetMjpegStringWithNumFrameLessDurationTest()
        {
            //Arrange
            const string destinationFilePath = "destination Path";
            const int numFrame = 10;
            var serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();
            
            var metadata = new VideoMetadata()
            {
                Duration=2000,
                Width = 4096,
                Height = 2304,
                FilePath = "source file path"
            };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegStringBuilder(metadataService, destinationFilePath);
            var ffmpegStr = "-i \"{0}\" -f image2 -ss {1} -frames:v 1 -y \"{2}.jpg\"";
            ffmpegStr = String.Format(ffmpegStr,
               metadata.FilePath,
               (int)serviceConfigurator.ScreenshotTime.TotalSeconds,
               destinationFilePath);

            //Act
            var str = stringBuilder.GetStringForScreenshot();

            //Assert
            Assert.AreEqual(ffmpegStr, str);
        }

        [TestMethod]
        public void GetMjpegStringWithNumFrameMoreDurationTest()
        {
            //Arrange
            const string destinationFilePath = "destination Path";
            const int numFrame = 10;
            var serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();

            var metadata = new VideoMetadata()
            {
                Duration = (int)serviceConfigurator.ScreenshotTime.TotalMilliseconds - 1,
                Width = 4096,
                Height = 2304,
                FilePath = "source file path"
            };

            var metadataService = new MetadataService(serviceConfigurator, metadata);
            var stringBuilder = new FfmpegStringBuilder(metadataService, destinationFilePath);
            var ffmpegStr = "-i \"{0}\" -f image2 -ss {1} -frames:v 1 -y \"{2}.jpg\"";
            ffmpegStr = String.Format(ffmpegStr,
               metadata.FilePath,
               0,
               destinationFilePath);

            //Act
            var str = stringBuilder.GetStringForScreenshot();

            //Assert
            Assert.AreEqual(ffmpegStr, str);
        }
    }

   
}
