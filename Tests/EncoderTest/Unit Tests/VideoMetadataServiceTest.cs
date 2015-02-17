using System;
using System.Collections.Generic;
using System.Linq;
using EncoderTest.Tmp;
using MSTestExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Domain;
using PortalEncoder;
using PortalEncoder.Exceptions;

namespace EncoderTest
{
    [TestClass]
    public class VideoMetadataServiceTest
    {
        [TestMethod]
        public void AdjustVideoBpsForVideoSizeMoreConfigureMaxSizeTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);
            metadataConfigurator.MaxWidth = 1280;
            metadataConfigurator.MaxHeight = 720;

            var metadata1920X1080X8000 = new VideoMediaInfo
                {
                    VideoWidth = 1920,
                    VideoHeight = 1080,
                    VideoBitRate = metadataConfigurator.VideoBps1920X1080
                };
            var metadataService1 = new MetadataService(metadataConfigurator, metadata1920X1080X8000);

            //Act
            int adjBps1920X1080X8000 = metadataService1.AdjustVideoBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.VideoBps1280X720, adjBps1920X1080X8000);
        }

        [TestMethod]
        public void AdjustVideoBpsForVideoSizeLessConfigureMaxSizeTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata1920X1080X8000 = new VideoMediaInfo
                {
                    VideoWidth = 1920,
                    VideoHeight = 1080,
                    VideoBitRate = metadataConfigurator.VideoBps1920X1080
                };
            var metadataService1 = new MetadataService(metadataConfigurator, metadata1920X1080X8000);

            //Act
            int adjBps1920X1080X8000 = metadataService1.AdjustVideoBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.VideoBps1920X1080, adjBps1920X1080X8000);
        }

        [TestMethod]
        public void AdjustAudioBpsForVideoSizeMoreConfigureMaxSizeTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);
            metadataConfigurator.MaxWidth = 854;
            metadataConfigurator.MaxHeight = 480;

            var metadata128Bps = new VideoMediaInfo
                {
                    VideoWidth = 1920,
                    VideoHeight = 1080,
                    AudioChannels = (int) AudioChannel.One,
                    AudioBitRate = metadataConfigurator.AudioBps720P1Channel
                };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata128Bps);

            //Act
            int adjBps = metadataService1.AdjustAudioBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.AudioBps360P1Channel, adjBps);
        }

        [TestMethod]
        public void AdjustAudioBpsForVideoSizeLessConfigureMaxSizeTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata128Bps = new VideoMediaInfo
                {
                    VideoWidth = 1920,
                    VideoHeight = 1080,
                    AudioChannels = (int) AudioChannel.One,
                    AudioBitRate = metadataConfigurator.AudioBps720P1Channel
                };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata128Bps);

            //Act
            int adjBps = metadataService1.AdjustAudioBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.AudioBps720P1Channel, adjBps);
        }

        [TestMethod]
        public void AdjustWidthMoreConfigureMaxWidthTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);
            metadataConfigurator.MaxWidth = 100*metadataConfigurator.VideoSizeMul;

            var metadata = new VideoMediaInfo
                {
                    VideoWidth = metadataConfigurator.MaxWidth + metadataConfigurator.VideoSizeMul
                };
            var metadataService = new MetadataService(metadataConfigurator, metadata);

            //Act
            int adjustedwidth = metadataService.AdjustVideoWidth();

            //Assert
            Assert.AreEqual(metadataConfigurator.MaxWidth, adjustedwidth);
        }

        [TestMethod]
        public void AdjustHeightMoreConfigureMaxHeightTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);
            metadataConfigurator.MaxHeight = 100*metadataConfigurator.VideoSizeMul;

            var metadata = new VideoMediaInfo
                {
                    VideoHeight = metadataConfigurator.MaxHeight + metadataConfigurator.VideoSizeMul
                };
            var metadataService = new MetadataService(metadataConfigurator, metadata);

            //Act
            int adjustedHeight = metadataService.AdjustVideoHeight();

            //Assert
            Assert.AreEqual(metadataConfigurator.MaxHeight, adjustedHeight);
        }


        [TestMethod]
        public void AdjustWidthVideoSuccessTest()
        {
            //Arrange
            const int width = 35;
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata = new VideoMediaInfo
                {
                    VideoWidth = width*metadataConfigurator.VideoSizeMul
                };
            var metadataService = new MetadataService(metadataConfigurator, metadata);

            //Act
            int adjustedwidth = metadataService.AdjustVideoWidth();

            //Assert
            Assert.AreEqual(metadata.VideoWidth, adjustedwidth);
        }

        [TestMethod]
        public void AdjustWidtVideoFailTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata1 = new VideoMediaInfo
                {
                    VideoWidth = 15
                };

            var metadata2 = new VideoMediaInfo
                {
                    VideoWidth = -1
                };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata1);
            var metadataService2 = new MetadataService(metadataConfigurator, metadata2);

            //Act & Assert
            CustomAssert.IsThrown<MediaFormatException>(() => metadataService1.AdjustVideoWidth());
            CustomAssert.IsThrown<MediaFormatException>(() => metadataService2.AdjustVideoWidth());
        }

        [TestMethod]
        public void AdjustWidthMultiply16Test()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata1 = new VideoMediaInfo
                {
                    VideoWidth = 100
                };
            var metadata2 = new VideoMediaInfo
                {
                    VideoWidth = 89
                };
            var metadataService1 = new MetadataService(metadataConfigurator, metadata1);
            var metadataService2 = new MetadataService(metadataConfigurator, metadata2);

            //Act
            int adjustedwidth1 = metadataService1.AdjustVideoWidth();
            int adjustedwidth2 = metadataService2.AdjustVideoWidth();

            //Assert
            Assert.AreEqual(96, adjustedwidth1);
            Assert.AreEqual(80, adjustedwidth2);
        }

        [TestMethod]
        public void AdjustHeightVideoSuccessTest()
        {
            //Arrange
            const int height = 23;
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata = new VideoMediaInfo
                {
                    VideoHeight = height*metadataConfigurator.VideoSizeMul
                };
            var metadataService = new MetadataService(metadataConfigurator, metadata);

            //Act
            int adjustedHeight = metadataService.AdjustVideoHeight();

            //Assert
            Assert.AreEqual(metadata.VideoHeight, adjustedHeight);
        }

        [TestMethod]
        public void AdjustHeightVideoFailTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata1 = new VideoMediaInfo
                {
                    VideoHeight = 15
                };

            var metadata2 = new VideoMediaInfo
                {
                    VideoHeight = -1
                };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata1);
            var metadataService2 = new MetadataService(metadataConfigurator, metadata2);

            //Act & Assert
            CustomAssert.IsThrown<MediaFormatException>(() => metadataService1.AdjustVideoHeight());
            CustomAssert.IsThrown<MediaFormatException>(() => metadataService2.AdjustVideoHeight());
        }

        [TestMethod]
        public void AdjustHeighhMultiply16Test()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata1 = new VideoMediaInfo
                {
                    VideoHeight = 100
                };
            var metadata2 = new VideoMediaInfo
                {
                    VideoHeight = 89
                };
            var metadataService1 = new MetadataService(metadataConfigurator, metadata1);
            var metadataService2 = new MetadataService(metadataConfigurator, metadata2);

            //Act
            int adjustedwidth1 = metadataService1.AdjustVideoHeight();
            int adjustedwidth2 = metadataService2.AdjustVideoHeight();

            //Assert
            Assert.AreEqual(96, adjustedwidth1);
            Assert.AreEqual(80, adjustedwidth2);
        }

        [TestMethod]
        public void AdjustVideoFpsSuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata1 = new VideoMediaInfo
                {
                    VideoFrameRate = 29.970
                };

            var metadata2 = new VideoMediaInfo
                {
                    VideoFrameRate = metadataConfigurator.MinFps - 1
                };

            var metadata3 = new VideoMediaInfo
                {
                    VideoFrameRate = metadataConfigurator.MaxFps + 1
                };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata1);
            var metadataService2 = new MetadataService(metadataConfigurator, metadata2);
            var metadataService3 = new MetadataService(metadataConfigurator, metadata3);

            //Act
            double adjustedFps1 = metadataService1.AdjustVideoFps();

            //Act & Assert
            CustomAssert.IsThrown<MediaFormatException>(() => metadataService2.AdjustVideoFps());
            CustomAssert.IsThrown<MediaFormatException>(() => metadataService3.AdjustVideoFps());

            //Assert
            Assert.AreEqual(metadata1.VideoFrameRate, adjustedFps1);
        }

        [TestMethod]
        public void AdjustVideoBpsFor1920X1080SuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata1920X1080X8000 = new VideoMediaInfo
                {
                    VideoWidth = 1920,
                    VideoHeight = 1080,
                    VideoBitRate = metadataConfigurator.VideoBps1920X1080
                };
            var metadata1920X1080More8000 = new VideoMediaInfo
                {
                    VideoWidth = 1920,
                    VideoHeight = 1080,
                    VideoBitRate = metadataConfigurator.VideoBps1920X1080 + 1
                };
            var metadata1920X1080Less8000 = new VideoMediaInfo
                {
                    VideoWidth = 1920,
                    VideoHeight = 1080,
                    VideoBitRate = metadataConfigurator.VideoBps1920X1080 - 1
                };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata1920X1080X8000);
            var metadataService2 = new MetadataService(metadataConfigurator, metadata1920X1080More8000);
            var metadataService3 = new MetadataService(metadataConfigurator, metadata1920X1080Less8000);

            //Act
            int adjBps1920X1080X8000 = metadataService1.AdjustVideoBps();
            int adjBps1920X1080More8000 = metadataService2.AdjustVideoBps();
            int adjBps1920X1080Less8000 = metadataService3.AdjustVideoBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.VideoBps1920X1080, adjBps1920X1080X8000);
            Assert.AreEqual(metadataConfigurator.VideoBps1920X1080, adjBps1920X1080More8000);
            Assert.AreEqual(metadata1920X1080Less8000.VideoBitRate, adjBps1920X1080Less8000);
        }

        [TestMethod]
        public void AdjustVideoBpsFor1280X720SuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata1280X720X5000 = new VideoMediaInfo
                {
                    VideoWidth = 1280,
                    VideoHeight = 720,
                    VideoBitRate = metadataConfigurator.VideoBps1280X720
                };
            var metadata1280X720More5000 = new VideoMediaInfo
                {
                    VideoWidth = 1280,
                    VideoHeight = 720,
                    VideoBitRate = metadataConfigurator.VideoBps1280X720 + 1
                };
            var metadata1280X720Less5000 = new VideoMediaInfo
                {
                    VideoWidth = 1280,
                    VideoHeight = 720,
                    VideoBitRate = metadataConfigurator.VideoBps1280X720 - 1
                };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata1280X720X5000);
            var metadataService2 = new MetadataService(metadataConfigurator, metadata1280X720More5000);
            var metadataService3 = new MetadataService(metadataConfigurator, metadata1280X720Less5000);

            //Act
            int adjBps1280X720X5000 = metadataService1.AdjustVideoBps();
            int adjBps1280X720More5000 = metadataService2.AdjustVideoBps();
            int adjBps1280X720Less5000 = metadataService3.AdjustVideoBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.VideoBps1280X720, adjBps1280X720X5000);
            Assert.AreEqual(metadataConfigurator.VideoBps1280X720, adjBps1280X720More5000);
            Assert.AreEqual(metadata1280X720Less5000.VideoBitRate, adjBps1280X720Less5000);
        }

        [TestMethod]
        public void AdjustVideoBpsFor854X480SuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata854X480X2500 = new VideoMediaInfo
                {
                    VideoWidth = 854,
                    VideoHeight = 480,
                    VideoBitRate = metadataConfigurator.VideoBps854X480
                };
            var metadata854X480More2500 = new VideoMediaInfo
                {
                    VideoWidth = 854,
                    VideoHeight = 480,
                    VideoBitRate = metadataConfigurator.VideoBps854X480 + 1
                };
            var metadata854X480Less2500 = new VideoMediaInfo
                {
                    VideoWidth = 854,
                    VideoHeight = 480,
                    VideoBitRate = metadataConfigurator.VideoBps854X480 - 1
                };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata854X480X2500);
            var metadataService2 = new MetadataService(metadataConfigurator, metadata854X480More2500);
            var metadataService3 = new MetadataService(metadataConfigurator, metadata854X480Less2500);

            //Act
            int adjBps854X480X2500 = metadataService1.AdjustVideoBps();
            int adjBps854X480More2500 = metadataService2.AdjustVideoBps();
            int adjBps854X480Less2500 = metadataService3.AdjustVideoBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.VideoBps854X480, adjBps854X480X2500);
            Assert.AreEqual(metadataConfigurator.VideoBps854X480, adjBps854X480More2500);
            Assert.AreEqual(metadata854X480Less2500.VideoBitRate, adjBps854X480Less2500);
        }

        [TestMethod]
        public void AdjustVideoBpsFor640X360SuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata640X360X1000 = new VideoMediaInfo
                {
                    VideoWidth = 640,
                    VideoHeight = 360,
                    VideoBitRate = metadataConfigurator.VideoBps640X360
                };
            var metadata640X360More1000 = new VideoMediaInfo
                {
                    VideoWidth = 640,
                    VideoHeight = 360,
                    VideoBitRate = metadataConfigurator.VideoBps640X360 + 1
                };
            var metadata640X360Less1000 = new VideoMediaInfo
                {
                    VideoWidth = 640,
                    VideoHeight = 360,
                    VideoBitRate = metadataConfigurator.VideoBps640X360 - 1
                };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata640X360X1000);
            var metadataService2 = new MetadataService(metadataConfigurator, metadata640X360More1000);
            var metadataService3 = new MetadataService(metadataConfigurator, metadata640X360Less1000);

            //Act
            int adjBps640X360X1000 = metadataService1.AdjustVideoBps();
            int adjBps640X360More1000 = metadataService2.AdjustVideoBps();
            int adjBps640X360Less1000 = metadataService3.AdjustVideoBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.VideoBps640X360, adjBps640X360X1000);
            Assert.AreEqual(metadataConfigurator.VideoBps640X360, adjBps640X360More1000);
            Assert.AreEqual(metadata640X360Less1000.VideoBitRate, adjBps640X360Less1000);
        }

        [TestMethod]
        public void AdjustVideoBpsForLess640X360SuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata1000Bps = new VideoMediaInfo
                {
                    VideoWidth = 40,
                    VideoHeight = 60,
                    VideoBitRate = metadataConfigurator.VideoBps640X360
                };
            var metadataMore1000Bps = new VideoMediaInfo
                {
                    VideoWidth = 40,
                    VideoHeight = 60,
                    VideoBitRate = metadataConfigurator.VideoBps640X360 + 1
                };
            var metadataLess1000Bps = new VideoMediaInfo
                {
                    VideoWidth = 40,
                    VideoHeight = 60,
                    VideoBitRate = metadataConfigurator.VideoBps640X360 - 1
                };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata1000Bps);
            var metadataService2 = new MetadataService(metadataConfigurator, metadataMore1000Bps);
            var metadataService3 = new MetadataService(metadataConfigurator, metadataLess1000Bps);

            //Act
            int adjBps1000Bps = metadataService1.AdjustVideoBps();
            int adjBpsMore1000Bps = metadataService2.AdjustVideoBps();
            int adjBpsLess1000Bps = metadataService3.AdjustVideoBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.VideoBps640X360, adjBps1000Bps);
            Assert.AreEqual(metadataConfigurator.VideoBps640X360, adjBpsMore1000Bps);
            Assert.AreEqual(metadataLess1000Bps.VideoBitRate, adjBpsLess1000Bps);
        }

        [TestMethod]
        public void AdjustAudioBpsFor1920X1080And1280X720X1ChannelSuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata128Bps = new VideoMediaInfo
                {
                    VideoWidth = 1280,
                    VideoHeight = 720,
                    AudioChannels = (int) AudioChannel.One,
                    AudioBitRate = metadataConfigurator.AudioBps720P1Channel
                };
            var metadataMore128Bps = new VideoMediaInfo
                {
                    VideoWidth = 1280,
                    VideoHeight = 720,
                    AudioChannels = (int) AudioChannel.One,
                    AudioBitRate = metadataConfigurator.AudioBps720P1Channel + 1
                };
            var metadataLess128Bps = new VideoMediaInfo
                {
                    VideoWidth = 1280,
                    VideoHeight = 720,
                    AudioChannels = (int) AudioChannel.One,
                    AudioBitRate = metadataConfigurator.AudioBps720P1Channel - 1
                };
            var metadataService1 = new MetadataService(metadataConfigurator, metadata128Bps);
            var metadataService2 = new MetadataService(metadataConfigurator, metadataMore128Bps);
            var metadataService3 = new MetadataService(metadataConfigurator, metadataLess128Bps);

            //Act
            int adjBps128Bps = metadataService1.AdjustAudioBps();
            int adjBpsMore128Bps = metadataService2.AdjustAudioBps();
            int adjBpsLess128Bps = metadataService3.AdjustAudioBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.AudioBps720P1Channel, adjBps128Bps);
            Assert.AreEqual(metadataConfigurator.AudioBps720P1Channel, adjBpsMore128Bps);
            Assert.AreEqual(metadataLess128Bps.AudioBitRate, adjBpsLess128Bps);
        }

        [TestMethod]
        public void AdjustAudioBpsFor1920X1080And1280X720X2ChannelSuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata384Bps = new VideoMediaInfo
                {
                    VideoWidth = 1280,
                    VideoHeight = 720,
                    AudioChannels = (int) AudioChannel.Two,
                    AudioBitRate = metadataConfigurator.AudioBps720P2Channel
                };
            var metadataMore384Bps = new VideoMediaInfo
                {
                    VideoWidth = 1280,
                    VideoHeight = 720,
                    AudioChannels = (int) AudioChannel.Two,
                    AudioBitRate = metadataConfigurator.AudioBps720P2Channel + 1
                };
            var metadataLess384Bps = new VideoMediaInfo
                {
                    VideoWidth = 1280,
                    VideoHeight = 720,
                    AudioChannels = (int) AudioChannel.Two,
                    AudioBitRate = metadataConfigurator.AudioBps720P2Channel - 1
                };
            var metadataService1 = new MetadataService(metadataConfigurator, metadata384Bps);
            var metadataService2 = new MetadataService(metadataConfigurator, metadataMore384Bps);
            var metadataService3 = new MetadataService(metadataConfigurator, metadataLess384Bps);

            //Act
            int adjBps384Bps = metadataService1.AdjustAudioBps();
            int adjBpsMore384Bps = metadataService2.AdjustAudioBps();
            int adjBpsLess384Bps = metadataService3.AdjustAudioBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.AudioBps720P2Channel, adjBps384Bps);
            Assert.AreEqual(metadataConfigurator.AudioBps720P2Channel, adjBpsMore384Bps);
            Assert.AreEqual(metadataLess384Bps.AudioBitRate, adjBpsLess384Bps);
        }

        [TestMethod]
        public void AdjustAudioBpsFor1920X1080And1280X720X6ChannelSuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata512Bps = new VideoMediaInfo
                {
                    VideoWidth = 1280,
                    VideoHeight = 720,
                    AudioChannels = (int) AudioChannel.Six,
                    AudioBitRate = metadataConfigurator.AudioBps720P6Channel
                };
            var metadataMore512Bps = new VideoMediaInfo
                {
                    VideoWidth = 1280,
                    VideoHeight = 720,
                    AudioChannels = (int) AudioChannel.Six,
                    AudioBitRate = metadataConfigurator.AudioBps720P6Channel + 1
                };
            var metadataLess512Bps = new VideoMediaInfo
                {
                    VideoWidth = 1280,
                    VideoHeight = 720,
                    AudioChannels = (int) AudioChannel.Six,
                    AudioBitRate = metadataConfigurator.AudioBps720P6Channel - 1
                };
            var metadataService1 = new MetadataService(metadataConfigurator, metadata512Bps);
            var metadataService2 = new MetadataService(metadataConfigurator, metadataMore512Bps);
            var metadataService3 = new MetadataService(metadataConfigurator, metadataLess512Bps);

            //Act
            int adjBps512Bps = metadataService1.AdjustAudioBps();
            int adjBpsMore512Bps = metadataService2.AdjustAudioBps();
            int adjBpsLess512Bps = metadataService3.AdjustAudioBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.AudioBps720P6Channel, adjBps512Bps);
            Assert.AreEqual(metadataConfigurator.AudioBps720P6Channel, adjBpsMore512Bps);
            Assert.AreEqual(metadataLess512Bps.AudioBitRate, adjBpsLess512Bps);
        }

        [TestMethod]
        public void AdjustAudioBpsFor854X480And640X360AndLessX1ChannelSuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata64Bps = new VideoMediaInfo
                {
                    VideoWidth = 640,
                    VideoHeight = 360,
                    AudioChannels = (int) AudioChannel.One,
                    AudioBitRate = metadataConfigurator.AudioBps360P1Channel
                };
            var metadataMore64Bps = new VideoMediaInfo
                {
                    VideoWidth = 640,
                    VideoHeight = 360,
                    AudioChannels = (int) AudioChannel.One,
                    AudioBitRate = metadataConfigurator.AudioBps360P1Channel + 1
                };
            var metadataLess64Bps = new VideoMediaInfo
                {
                    VideoWidth = 640,
                    VideoHeight = 360,
                    AudioChannels = (int) AudioChannel.One,
                    AudioBitRate = metadataConfigurator.AudioBps360P1Channel - 1
                };
            var metadataService1 = new MetadataService(metadataConfigurator, metadata64Bps);
            var metadataService2 = new MetadataService(metadataConfigurator, metadataMore64Bps);
            var metadataService3 = new MetadataService(metadataConfigurator, metadataLess64Bps);

            //Act
            int adjBps64Bps = metadataService1.AdjustAudioBps();
            int adjBpsMore64Bps = metadataService2.AdjustAudioBps();
            int adjBpsLess64Bps = metadataService3.AdjustAudioBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.AudioBps360P1Channel, adjBps64Bps);
            Assert.AreEqual(metadataConfigurator.AudioBps360P1Channel, adjBpsMore64Bps);
            Assert.AreEqual(metadataLess64Bps.AudioBitRate, adjBpsLess64Bps);
        }

        [TestMethod]
        public void AdjustAudioBpsFor854X480And640X360AndLessX2ChannelSuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata64Bps = new VideoMediaInfo
                {
                    VideoWidth = 640,
                    VideoHeight = 360,
                    AudioChannels = (int) AudioChannel.Two,
                    AudioBitRate = metadataConfigurator.AudioBps360P2Channel
                };
            var metadataMore64Bps = new VideoMediaInfo
                {
                    VideoWidth = 640,
                    VideoHeight = 360,
                    AudioChannels = (int) AudioChannel.Two,
                    AudioBitRate = metadataConfigurator.AudioBps360P2Channel + 1
                };
            var metadataLess64Bps = new VideoMediaInfo
                {
                    VideoWidth = 640,
                    VideoHeight = 360,
                    AudioChannels = (int) AudioChannel.Two,
                    AudioBitRate = metadataConfigurator.AudioBps360P2Channel - 1
                };
            var metadataService1 = new MetadataService(metadataConfigurator, metadata64Bps);
            var metadataService2 = new MetadataService(metadataConfigurator, metadataMore64Bps);
            var metadataService3 = new MetadataService(metadataConfigurator, metadataLess64Bps);

            //Act
            int adjBps64Bps = metadataService1.AdjustAudioBps();
            int adjBpsMore64Bps = metadataService2.AdjustAudioBps();
            int adjBpsLess64Bps = metadataService3.AdjustAudioBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.AudioBps360P2Channel, adjBps64Bps);
            Assert.AreEqual(metadataConfigurator.AudioBps360P2Channel, adjBpsMore64Bps);
            Assert.AreEqual(metadataLess64Bps.AudioBitRate, adjBpsLess64Bps);
        }

        [TestMethod]
        public void AdjustAudioBpsFor854X480And640X360AndLessX6ChannelSuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata64Bps = new VideoMediaInfo
                {
                    VideoWidth = 640,
                    VideoHeight = 360,
                    AudioChannels = (int) AudioChannel.Six,
                    AudioBitRate = metadataConfigurator.AudioBps360P6Channel
                };
            var metadataMore64Bps = new VideoMediaInfo
                {
                    VideoWidth = 640,
                    VideoHeight = 360,
                    AudioChannels = (int) AudioChannel.Six,
                    AudioBitRate = metadataConfigurator.AudioBps360P6Channel + 1
                };
            var metadataLess64Bps = new VideoMediaInfo
                {
                    VideoWidth = 640,
                    VideoHeight = 360,
                    AudioChannels = (int) AudioChannel.Six,
                    AudioBitRate = metadataConfigurator.AudioBps360P6Channel - 1
                };
            var metadataService1 = new MetadataService(metadataConfigurator, metadata64Bps);
            var metadataService2 = new MetadataService(metadataConfigurator, metadataMore64Bps);
            var metadataService3 = new MetadataService(metadataConfigurator, metadataLess64Bps);

            //Act
            int adjBps64Bps = metadataService1.AdjustAudioBps();
            int adjBpsMore64Bps = metadataService2.AdjustAudioBps();
            int adjBpsLess64Bps = metadataService3.AdjustAudioBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.AudioBps360P6Channel, adjBps64Bps);
            Assert.AreEqual(metadataConfigurator.AudioBps360P6Channel, adjBpsMore64Bps);
            Assert.AreEqual(metadataLess64Bps.AudioBitRate, adjBpsLess64Bps);
        }

        [TestMethod]
        public void AdjustVideoKeyFrameSuccessTest()
        {
            //Arrange
            var serviceConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);
            int myKeyFrame = serviceConfigurator.MaxKeyFrame - 1;

            var metadataKeyFrame0 = new VideoMediaInfo
                {
                    VideoFormatSettingsGOP = String.Format("M=1, N={0}", serviceConfigurator.MinKeyFrame - 1)
                };
            var metadataKeyFrameMore60 = new VideoMediaInfo
                {
                    VideoFormatSettingsGOP = String.Format("M=1, N={0}", serviceConfigurator.MaxKeyFrame + 1)
                };
            var metadataKeyFrame = new VideoMediaInfo
                {
                    VideoFormatSettingsGOP = String.Format("M=1, N={0}", myKeyFrame)
                };

            var metadataService1 = new MetadataService(serviceConfigurator, metadataKeyFrame0);
            var metadataService2 = new MetadataService(serviceConfigurator, metadataKeyFrameMore60);
            var metadataService3 = new MetadataService(serviceConfigurator, metadataKeyFrame);

            //Act;
            int keyFrame0 = metadataService1.AdjustKeyFrame();
            int keyFrameMore60 = metadataService2.AdjustKeyFrame();
            int keyFrame = metadataService3.AdjustKeyFrame();

            //Assert
            Assert.AreEqual(serviceConfigurator.KeyFrame, keyFrame0);
            Assert.AreEqual(serviceConfigurator.KeyFrame, keyFrameMore60);
            Assert.AreEqual(myKeyFrame, keyFrame);
        }

        [TestMethod]
        public void AdjustContainerTest()
        {
            //Arrange
            const string myContainer = "myContainer";
            const string anotherContainer = "anotherContainer";
            var serviceConfigurator = new MetadataServiceConfigurator(myContainer, null, null, null, null);

            var metadata1 = new VideoMediaInfo
                {
                    GeneralFormat = myContainer
                };
            var metadata2 = new VideoMediaInfo
                {
                    GeneralFormat = anotherContainer
                };
            var metadataWithoutContainer = new VideoMediaInfo
                {
                    GeneralFormat = null
                };

            var metadataService1 = new MetadataService(serviceConfigurator, metadata1);
            var metadataService2 = new MetadataService(serviceConfigurator, metadata2);
            var metadataService3 = new MetadataService(serviceConfigurator, metadataWithoutContainer);

            //Act
            string container1 = metadataService1.AdjustContainer();
            string container2 = metadataService2.AdjustContainer();

            //Act & Assert
            CustomAssert.IsThrown<MediaFormatException>(() => metadataService3.AdjustContainer());

            //Assert
            Assert.AreEqual(myContainer, container1);
            Assert.AreEqual(myContainer, container2);
        }

        [TestMethod]
        public void AdjustVideoCodecTest()
        {
            //Arrange
            const string anotherCodecName = "anotherCodec";
            const string myCodecName = "myCodec";
            var myCodec = new CodecData(myCodecName, null);

            var serviceConfigurator = new MetadataServiceConfigurator(null, null, myCodec, null, null);

            var metadata1 = new VideoMediaInfo
                {
                    VideoFormat = myCodecName
                };
            var metadata2 = new VideoMediaInfo
                {
                    VideoFormat = anotherCodecName
                };
            var metadataWithoutCodec = new VideoMediaInfo
                {
                    VideoFormat = null
                };

            var metadataService1 = new MetadataService(serviceConfigurator, metadata1);
            var metadataService2 = new MetadataService(serviceConfigurator, metadata2);
            var metadataService3 = new MetadataService(serviceConfigurator, metadataWithoutCodec);

            //Act
            string codec1 = metadataService1.AdjustVideoCodec();
            string codec2 = metadataService2.AdjustVideoCodec();

            //Act & Assert
            CustomAssert.IsThrown<MediaFormatException>(() => metadataService3.AdjustContainer());

            //Assert
            Assert.AreEqual(myCodecName, codec1);
            Assert.AreEqual(myCodecName, codec2);
        }

        [TestMethod]
        public void AdjustVideoProfileTest()
        {
            //Arrange
            const string myCodec = "myCodec";
            const string anotherProfile = "anotherProfile";
            const string firstProfile = "firstProfile";
            const string secondProfile = "secondProfile";

            var videoCodec = new CodecData(myCodec, null, firstProfile, secondProfile);

            var serviceConfigurator = new MetadataServiceConfigurator(null, null, videoCodec, null, null);

            var metadata1 = new VideoMediaInfo
                {
                    VideoFormat = myCodec,
                    VideoFormatProfile = firstProfile
                };
            var metadata2 = new VideoMediaInfo
                {
                    VideoFormat = myCodec,
                    VideoFormatProfile = secondProfile
                };
            var metadata3 = new VideoMediaInfo
                {
                    VideoFormat = myCodec,
                    VideoFormatProfile = anotherProfile
                };
            var metadataWithoutCodec = new VideoMediaInfo
                {
                    VideoFormat = myCodec,
                    VideoFormatProfile = null
                };

            var metadataService1 = new MetadataService(serviceConfigurator, metadata1);
            var metadataService2 = new MetadataService(serviceConfigurator, metadata2);
            var metadataService3 = new MetadataService(serviceConfigurator, metadata3);
            var metadataService4 = new MetadataService(serviceConfigurator, metadataWithoutCodec);

            //Act
            string profile1 = metadataService1.AdjustVideoProfile();
            string profile2 = metadataService2.AdjustVideoProfile();
            string profile3 = metadataService3.AdjustVideoProfile();
            string profile4 = metadataService4.AdjustVideoProfile();

            //Assert
            Assert.AreEqual(firstProfile, profile1);
            Assert.AreEqual(secondProfile, profile2);
            Assert.AreEqual(firstProfile, profile3);
            Assert.AreEqual(firstProfile, profile4);
        }

        [TestMethod]
        public void AdjustAudioCodecTest()
        {
            //Arrange
            const string myCodec1 = "myCodec1";
            const string myCodec2 = "myCodec2";
            const string anotherCodec = "anotherCodec";

            var audioCodec = new CodecData(myCodec1, null);

            var serviceConfigurator = new MetadataServiceConfigurator(null, null, null, audioCodec, new CodecData(myCodec2, null));

            var metadata1 = new VideoMediaInfo
                {
                    AudioFormat = myCodec1
                };
            var metadata2 = new VideoMediaInfo
                {
                    AudioFormat = myCodec2
                };
            var metadata3 = new VideoMediaInfo
                {
                    AudioFormat = anotherCodec
                };
            var metadataWithoutCodec = new VideoMediaInfo
                {
                    AudioFormat = null
                };

            var metadataService1 = new MetadataService(serviceConfigurator, metadata1);
            var metadataService2 = new MetadataService(serviceConfigurator, metadata2);
            var metadataService3 = new MetadataService(serviceConfigurator, metadata3);
            var metadataService4 = new MetadataService(serviceConfigurator, metadataWithoutCodec);

            //Act
            string codec1 = metadataService1.AdjustAudioCodec();
            string codec2 = metadataService2.AdjustAudioCodec();
            string codec3 = metadataService3.AdjustAudioCodec();
            string codec4 = metadataService4.AdjustAudioCodec();

            //Assert
            Assert.AreEqual(myCodec1, codec1);
            Assert.AreEqual(myCodec2, codec2);
            Assert.AreEqual(myCodec1, codec3);
            Assert.AreEqual(null, codec4);
        }

        [TestMethod]
        public void AdjustAudioProfileTest()
        {
            //Arrange
            const string myCodec1 = "myCodec1";
            const string myCodec2 = "myCodec2";
            const string anotherProfile = "anotherProfile";
            const string firstProfile = "firstProfile";

            var audioCodec = new CodecData(myCodec1, null, firstProfile);

            var serviceConfigurator = new MetadataServiceConfigurator(null, null, null, audioCodec, new CodecData(myCodec2, null));

            var metadata1 = new VideoMediaInfo
                {
                    AudioFormat = myCodec1,
                    AudioFormatProfile = firstProfile
                };
            var metadata2 = new VideoMediaInfo
                {
                    AudioFormat = myCodec1,
                    AudioFormatProfile = anotherProfile
                };
            var metadata3 = new VideoMediaInfo
                {
                    AudioFormat = myCodec2,
                    AudioFormatProfile = anotherProfile
                };
            var metadataWithoutProfile = new VideoMediaInfo
                {
                    AudioFormat = myCodec1,
                    AudioFormatProfile = null
                };

            var metadataService1 = new MetadataService(serviceConfigurator, metadata1);
            var metadataService2 = new MetadataService(serviceConfigurator, metadata2);
            var metadataService3 = new MetadataService(serviceConfigurator, metadata3);
            var metadataService4 = new MetadataService(serviceConfigurator, metadataWithoutProfile);

            //Act
            string profile1 = metadataService1.AdjustAudioProfile();
            string profile2 = metadataService2.AdjustAudioProfile();
            string profile3 = metadataService3.AdjustAudioProfile();
            string profile4 = metadataService4.AdjustAudioProfile();

            //Assert
            Assert.AreEqual(firstProfile, profile1);
            Assert.AreEqual(firstProfile, profile2);
            Assert.AreEqual(anotherProfile, profile3);
            Assert.AreEqual(firstProfile, profile4);
        }

        [TestMethod]
        public void AdjustScreenshotTimeTest()
        {
            //Arrange
            var serviceConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata1 = new VideoMediaInfo
                {
                    VideoDuration = (int) serviceConfigurator.ScreenshotTime.TotalMilliseconds - 1
                };
            var metadata2 = new VideoMediaInfo
                {
                    VideoDuration = (int) serviceConfigurator.ScreenshotTime.TotalMilliseconds + 1
                };
            var metadataService1 = new MetadataService(serviceConfigurator, metadata1);
            var metadataService2 = new MetadataService(serviceConfigurator, metadata2);

            //Act
            int time1 = metadataService1.AdjustScreenshotTime();
            int time2 = metadataService2.AdjustScreenshotTime();

            //Assert
            Assert.AreEqual(0, time1);
            Assert.AreEqual(serviceConfigurator.ScreenshotTime.Seconds, time2);
        }

        [TestMethod]
        public void CheckCorrectVideoValueListTest()
        {
            //Arrange
            MetadataServiceConfigurator serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();

            var metadata = new VideoMediaInfo
                {
                    AudioBitRate = serviceConfigurator.AudioBps720P1Channel,
                    AudioChannels = (int) AudioChannel.One,
                    AudioFormat = serviceConfigurator.AudioCodec.Codec,
                    AudioFormatProfile = serviceConfigurator.AudioCodec.DefaultProfile,
                    VideoBitRate = serviceConfigurator.VideoBps1920X1080,
                    VideoFormat = serviceConfigurator.VideoCodec.Codec,
                    VideoFormatProfile = serviceConfigurator.VideoCodec.DefaultProfile,
                    VideoFrameRate = serviceConfigurator.MaxFps - 1,
                    VideoFormatSettingsGOP = String.Format("M=1, N={0}", serviceConfigurator.KeyFrame),
                    VideoWidth = 4096,
                    VideoHeight = 2304
                };

            var metadataService = new MetadataService(serviceConfigurator, metadata);

            //Act
            metadataService.AdjustVideoWidth();
            metadataService.AdjustVideoHeight();
            metadataService.AdjustKeyFrame();
            metadataService.AdjustVideoBps();
            metadataService.AdjustVideoCodec();
            metadataService.AdjustVideoFps();
            metadataService.AdjustVideoProfile();

            //Assert
            Assert.IsTrue(metadataService.CorrectVideoValueList.All((b) => b.Value));
        }

        [TestMethod]
        public void CheckCorrectAudioValueListTest()
        {
            //Arrange
            MetadataServiceConfigurator serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();

            var metadata = new VideoMediaInfo
                {
                    AudioBitRate = serviceConfigurator.AudioBps720P1Channel,
                    AudioChannels = (int) AudioChannel.One,
                    AudioFormat = "AAC",
                    AudioFormatProfile = "LC",
                    VideoWidth = 4096,
                    VideoHeight = 2304
                };

            var metadataService = new MetadataService(serviceConfigurator, metadata);

            //Act
            metadataService.AdjustAudioBps();
            metadataService.AdjustAudioCodec();
            metadataService.AdjustAudioProfile();

            //Assert
            Assert.IsTrue(metadataService.CorrectAudioValueList.All((b) => b.Value));
        }

        [TestMethod]
        public void CheckCorrectContainerTest()
        {
            //Arrange
            MetadataServiceConfigurator serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();

            var metadata = new VideoMediaInfo
                {
                    GeneralFormat = serviceConfigurator.Container
                };

            var metadataService = new MetadataService(serviceConfigurator, metadata);

            //Act
            metadataService.AdjustContainer();

            //Assert
            Assert.IsTrue(metadataService.CorrectContainer);
        }

        [TestMethod]
        public void CorectVideoValueListFillingTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            //Act
            var metadataService = new MetadataService(metadataConfigurator, null);

            //Assert
            Assert.IsTrue(metadataService.CorrectVideoValueList.Contains(new KeyValuePair<MetadataValue, bool>(MetadataValue.Width, false)));
            Assert.IsTrue(metadataService.CorrectVideoValueList.Contains(new KeyValuePair<MetadataValue, bool>(MetadataValue.Height, false)));
            Assert.IsTrue(metadataService.CorrectVideoValueList.Contains(new KeyValuePair<MetadataValue, bool>(MetadataValue.VideoBps, false)));
            Assert.IsTrue(metadataService.CorrectVideoValueList.Contains(new KeyValuePair<MetadataValue, bool>(MetadataValue.VideoCodec, false)));
            Assert.IsTrue(metadataService.CorrectVideoValueList.Contains(new KeyValuePair<MetadataValue, bool>(MetadataValue.VideoFps, false)));
            Assert.IsTrue(metadataService.CorrectVideoValueList.Contains(new KeyValuePair<MetadataValue, bool>(MetadataValue.VideoKeyFrame, false)));
            Assert.IsTrue(metadataService.CorrectVideoValueList.Contains(new KeyValuePair<MetadataValue, bool>(MetadataValue.VideoProfile, false)));
        }

        [TestMethod]
        public void CorectAudioValueListFillingTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            //Act
            var metadataService = new MetadataService(metadataConfigurator, null);

            //Assert
            Assert.IsTrue(metadataService.CorrectAudioValueList.Contains(new KeyValuePair<MetadataValue, bool>(MetadataValue.AudioBps, false)));
            Assert.IsTrue(metadataService.CorrectAudioValueList.Contains(new KeyValuePair<MetadataValue, bool>(MetadataValue.AudioCodec, false)));
            Assert.IsTrue(metadataService.CorrectAudioValueList.Contains(new KeyValuePair<MetadataValue, bool>(MetadataValue.AudioProfile, false)));
        }
    }
}