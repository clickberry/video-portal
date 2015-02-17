using System;
using System.Collections.Generic;
using System.Linq;
using Encoder;
using Encoder.Exceptions;
using EncoderTest.Tmp;
using MSTestExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EncoderTest
{
    [TestClass]
    public class VideoMetadataServiceTest
    {
        [TestMethod]
        public void AdjustWidthVideoSuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata = new VideoMetadata()
                               {
                                   Width = 100
                               };
            var metadataService = new MetadataService(metadataConfigurator, metadata);

            //Act
            var adjustedwidth = metadataService.AdjustVideoWidth();

            //Assert
            Assert.AreEqual(metadata.Width, adjustedwidth);
        }

        [TestMethod]
        public void AdjustWidtVideoFailTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata1 = new VideoMetadata()
                                {
                                    Width = 0
                                };

            var metadata2 = new VideoMetadata()
                                {
                                    Width = -1
                                };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata1);
            var metadataService2 = new MetadataService(metadataConfigurator, metadata2);

            //Act & Assert
            CustomAssert.IsThrown<MediaFormatException>(() => metadataService1.AdjustVideoWidth());
            CustomAssert.IsThrown<MediaFormatException>(() => metadataService2.AdjustVideoWidth());
        }

        [TestMethod]
        public void AdjustHeightVideoSuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata = new VideoMetadata()
            {
                Height = 100
            };
            var metadataService = new MetadataService(metadataConfigurator, metadata);

            //Act
            var adjustedHeight = metadataService.AdjustVideoHeight();

            //Assert
            Assert.AreEqual(metadata.Height, adjustedHeight);
        }

        [TestMethod]
        public void AdjustHeightVideoFailTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata1 = new VideoMetadata()
            {
                Height = 0
            };

            var metadata2 = new VideoMetadata()
            {
                Height = -1
            };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata1);
            var metadataService2 = new MetadataService(metadataConfigurator, metadata2);

            //Act & Assert
            CustomAssert.IsThrown<MediaFormatException>(() => metadataService1.AdjustVideoHeight());
            CustomAssert.IsThrown<MediaFormatException>(() => metadataService2.AdjustVideoHeight());
        }

        [TestMethod]
        public void AdjustVideoFpsSuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata1 = new VideoMetadata()
                                {
                                    VideoFps = 29.970
                                };

            var metadata2 = new VideoMetadata()
                                {
                                    VideoFps = metadataConfigurator.MinFps - 1
                                };

            var metadata3 = new VideoMetadata()
                                {
                                    VideoFps = metadataConfigurator.MaxFps + 1
                                };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata1);
            var metadataService2 = new MetadataService(metadataConfigurator, metadata2);
            var metadataService3 = new MetadataService(metadataConfigurator, metadata3);

            //Act
            var adjustedFps1 = metadataService1.AdjustVideoFps();
            
            //Act & Assert
            CustomAssert.IsThrown<MediaFormatException>(() => metadataService2.AdjustVideoFps());
            CustomAssert.IsThrown<MediaFormatException>(() =>metadataService3.AdjustVideoFps());

            //Assert
            Assert.AreEqual(metadata1.VideoFps, adjustedFps1);
        }

        [TestMethod]
        public void AdjustVideoBpsFor1920X1080SuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata1920X1080X8000 = new VideoMetadata()
            {
                Width = 1920,
                Height = 1080,
                VideoBps = metadataConfigurator.VideoBps1920X1080
            };
            var metadata1920X1080More8000 = new VideoMetadata()
            {
                Width = 1920,
                Height = 1080,
                VideoBps = metadataConfigurator.VideoBps1920X1080 + 1
            };
            var metadata1920X1080Less8000 = new VideoMetadata()
            {
                Width = 1920,
                Height = 1080,
                VideoBps = metadataConfigurator.VideoBps1920X1080 - 1
            };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata1920X1080X8000);
            var metadataService2 = new MetadataService(metadataConfigurator, metadata1920X1080More8000);
            var metadataService3 = new MetadataService(metadataConfigurator, metadata1920X1080Less8000);

            //Act
            var adjBps1920X1080X8000 = metadataService1.AdjustVideoBps();
            var adjBps1920X1080More8000 = metadataService2.AdjustVideoBps();
            var adjBps1920X1080Less8000 = metadataService3.AdjustVideoBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.VideoBps1920X1080, adjBps1920X1080X8000);
            Assert.AreEqual(metadataConfigurator.VideoBps1920X1080, adjBps1920X1080More8000);
            Assert.AreEqual(metadata1920X1080Less8000.VideoBps, adjBps1920X1080Less8000);
        }

        [TestMethod]
        public void AdjustVideoBpsFor1280X720SuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata1280X720X5000 = new VideoMetadata()
            {
                Width = 1280,
                Height = 720,
                VideoBps = metadataConfigurator.VideoBps1280X720
            };
            var metadata1280X720More5000 = new VideoMetadata()
            {
                Width = 1280,
                Height = 720,
                VideoBps = metadataConfigurator.VideoBps1280X720 + 1
            };
            var metadata1280X720Less5000 = new VideoMetadata()
            {
                Width = 1280,
                Height = 720,
                VideoBps = metadataConfigurator.VideoBps1280X720 - 1
            };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata1280X720X5000);
            var metadataService2 = new MetadataService(metadataConfigurator, metadata1280X720More5000);
            var metadataService3 = new MetadataService(metadataConfigurator, metadata1280X720Less5000);

            //Act
            var adjBps1280X720X5000 = metadataService1.AdjustVideoBps();
            var adjBps1280X720More5000 = metadataService2.AdjustVideoBps();
            var adjBps1280X720Less5000 = metadataService3.AdjustVideoBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.VideoBps1280X720, adjBps1280X720X5000);
            Assert.AreEqual(metadataConfigurator.VideoBps1280X720, adjBps1280X720More5000);
            Assert.AreEqual(metadata1280X720Less5000.VideoBps, adjBps1280X720Less5000);
        }

        [TestMethod]
        public void AdjustVideoBpsFor854X480SuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata854X480X2500 = new VideoMetadata()
            {
                Width = 854,
                Height = 480,
                VideoBps = metadataConfigurator.VideoBps854X480
            };
            var metadata854X480More2500 = new VideoMetadata()
            {
                Width = 854,
                Height = 480,
                VideoBps = metadataConfigurator.VideoBps854X480 + 1
            };
            var metadata854X480Less2500 = new VideoMetadata()
            {
                Width = 854,
                Height = 480,
                VideoBps = metadataConfigurator.VideoBps854X480 - 1
            };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata854X480X2500);
            var metadataService2 = new MetadataService(metadataConfigurator, metadata854X480More2500);
            var metadataService3 = new MetadataService(metadataConfigurator, metadata854X480Less2500);

            //Act
            var adjBps854X480X2500 = metadataService1.AdjustVideoBps();
            var adjBps854X480More2500 = metadataService2.AdjustVideoBps();
            var adjBps854X480Less2500 = metadataService3.AdjustVideoBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.VideoBps854X480, adjBps854X480X2500);
            Assert.AreEqual(metadataConfigurator.VideoBps854X480, adjBps854X480More2500);
            Assert.AreEqual(metadata854X480Less2500.VideoBps, adjBps854X480Less2500);
        }

        [TestMethod]
        public void AdjustVideoBpsFor640X360SuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata640X360X1000 = new VideoMetadata()
            {
                Width = 640,
                Height = 360,
                VideoBps = metadataConfigurator.VideoBps640X360
            };
            var metadata640X360More1000 = new VideoMetadata()
            {
                Width = 640,
                Height = 360,
                VideoBps = metadataConfigurator.VideoBps640X360 + 1
            };
            var metadata640X360Less1000 = new VideoMetadata()
            {
                Width = 640,
                Height = 360,
                VideoBps = metadataConfigurator.VideoBps640X360 - 1
            };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata640X360X1000);
            var metadataService2 = new MetadataService(metadataConfigurator, metadata640X360More1000);
            var metadataService3 = new MetadataService(metadataConfigurator, metadata640X360Less1000);

            //Act
            var adjBps640X360X1000 = metadataService1.AdjustVideoBps();
            var adjBps640X360More1000 = metadataService2.AdjustVideoBps();
            var adjBps640X360Less1000 = metadataService3.AdjustVideoBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.VideoBps640X360, adjBps640X360X1000);
            Assert.AreEqual(metadataConfigurator.VideoBps640X360, adjBps640X360More1000);
            Assert.AreEqual(metadata640X360Less1000.VideoBps, adjBps640X360Less1000);
        }

        [TestMethod]
        public void AdjustVideoBpsForLess640X360SuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata1000Bps = new VideoMetadata()
            {
                Width = 40,
                Height = 60,
                VideoBps = metadataConfigurator.VideoBps640X360
            };
            var metadataMore1000Bps = new VideoMetadata()
            {
                Width = 40,
                Height = 60,
                VideoBps = metadataConfigurator.VideoBps640X360 + 1
            };
            var metadataLess1000Bps = new VideoMetadata()
            {
                Width = 40,
                Height = 60,
                VideoBps = metadataConfigurator.VideoBps640X360 - 1
            };

            var metadataService1 = new MetadataService(metadataConfigurator, metadata1000Bps);
            var metadataService2 = new MetadataService(metadataConfigurator, metadataMore1000Bps);
            var metadataService3 = new MetadataService(metadataConfigurator, metadataLess1000Bps);

            //Act
            var adjBps1000Bps = metadataService1.AdjustVideoBps();
            var adjBpsMore1000Bps = metadataService2.AdjustVideoBps();
            var adjBpsLess1000Bps = metadataService3.AdjustVideoBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.VideoBps640X360, adjBps1000Bps);
            Assert.AreEqual(metadataConfigurator.VideoBps640X360, adjBpsMore1000Bps);
            Assert.AreEqual(metadataLess1000Bps.VideoBps, adjBpsLess1000Bps);
        }

        [TestMethod]
        public void AdjustAudioBpsFor1920X1080And1280X720X1ChannelSuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata128Bps = new VideoMetadata()
                                      {
                                          Width = 1280,
                                          Height = 720,
                                          AudioChannel = AudioChannel.One,
                                          AudioBps = metadataConfigurator.AudioBps720P1Channel
                                      };
            var metadataMore128Bps = new VideoMetadata()
                                         {
                                             Width = 1280,
                                             Height = 720,
                                             AudioChannel = AudioChannel.One,
                                             AudioBps = metadataConfigurator.AudioBps720P1Channel + 1
                                         };
            var metadataLess128Bps = new VideoMetadata()
                                         {
                                             Width = 1280,
                                             Height = 720,
                                             AudioChannel = AudioChannel.One,
                                             AudioBps = metadataConfigurator.AudioBps720P1Channel - 1
                                         };
            var metadataService1 = new MetadataService(metadataConfigurator, metadata128Bps);
            var metadataService2 = new MetadataService(metadataConfigurator, metadataMore128Bps);
            var metadataService3 = new MetadataService(metadataConfigurator, metadataLess128Bps);

            //Act
            var adjBps128Bps = metadataService1.AdjustAudioBps();
            var adjBpsMore128Bps = metadataService2.AdjustAudioBps();
            var adjBpsLess128Bps = metadataService3.AdjustAudioBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.AudioBps720P1Channel, adjBps128Bps);
            Assert.AreEqual(metadataConfigurator.AudioBps720P1Channel, adjBpsMore128Bps);
            Assert.AreEqual(metadataLess128Bps.AudioBps, adjBpsLess128Bps);
        }

        [TestMethod]
        public void AdjustAudioBpsFor1920X1080And1280X720X2ChannelSuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata384Bps = new VideoMetadata()
            {
                Width = 1280,
                Height = 720,
                AudioChannel = AudioChannel.Two,
                AudioBps = metadataConfigurator.AudioBps720P2Channel
            };
            var metadataMore384Bps = new VideoMetadata()
            {
                Width = 1280,
                Height = 720,
                AudioChannel = AudioChannel.Two,
                AudioBps = metadataConfigurator.AudioBps720P2Channel + 1
            };
            var metadataLess384Bps = new VideoMetadata()
            {
                Width = 1280,
                Height = 720,
                AudioChannel = AudioChannel.Two,
                AudioBps = metadataConfigurator.AudioBps720P2Channel - 1
            };
            var metadataService1 = new MetadataService(metadataConfigurator, metadata384Bps);
            var metadataService2 = new MetadataService(metadataConfigurator, metadataMore384Bps);
            var metadataService3 = new MetadataService(metadataConfigurator, metadataLess384Bps);

            //Act
            var adjBps384Bps = metadataService1.AdjustAudioBps();
            var adjBpsMore384Bps = metadataService2.AdjustAudioBps();
            var adjBpsLess384Bps = metadataService3.AdjustAudioBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.AudioBps720P2Channel, adjBps384Bps);
            Assert.AreEqual(metadataConfigurator.AudioBps720P2Channel, adjBpsMore384Bps);
            Assert.AreEqual(metadataLess384Bps.AudioBps, adjBpsLess384Bps);
        }

        [TestMethod]
        public void AdjustAudioBpsFor1920X1080And1280X720X6ChannelSuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata512Bps = new VideoMetadata()
            {
                Width = 1280,
                Height = 720,
                AudioChannel = AudioChannel.Six,
                AudioBps = metadataConfigurator.AudioBps720P6Channel
            };
            var metadataMore512Bps = new VideoMetadata()
            {
                Width = 1280,
                Height = 720,
                AudioChannel = AudioChannel.Six,
                AudioBps = metadataConfigurator.AudioBps720P6Channel + 1
            };
            var metadataLess512Bps = new VideoMetadata()
            {
                Width = 1280,
                Height = 720,
                AudioChannel = AudioChannel.Six,
                AudioBps = metadataConfigurator.AudioBps720P6Channel - 1
            };
            var metadataService1 = new MetadataService(metadataConfigurator, metadata512Bps);
            var metadataService2 = new MetadataService(metadataConfigurator, metadataMore512Bps);
            var metadataService3 = new MetadataService(metadataConfigurator, metadataLess512Bps);

            //Act
            var adjBps512Bps = metadataService1.AdjustAudioBps();
            var adjBpsMore512Bps = metadataService2.AdjustAudioBps();
            var adjBpsLess512Bps = metadataService3.AdjustAudioBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.AudioBps720P6Channel, adjBps512Bps);
            Assert.AreEqual(metadataConfigurator.AudioBps720P6Channel, adjBpsMore512Bps);
            Assert.AreEqual(metadataLess512Bps.AudioBps, adjBpsLess512Bps);
        }

        [TestMethod]
        public void AdjustAudioBpsFor854X480And640X360AndLessX1ChannelSuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata64Bps = new VideoMetadata()
            {
                Width = 640,
                Height = 360,
                AudioChannel = AudioChannel.One,
                AudioBps = metadataConfigurator.AudioBps360P1Channel
            };
            var metadataMore64Bps = new VideoMetadata()
            {
                Width = 640,
                Height = 360,
                AudioChannel = AudioChannel.One,
                AudioBps = metadataConfigurator.AudioBps360P1Channel + 1
            };
            var metadataLess64Bps = new VideoMetadata()
            {
                Width = 640,
                Height = 360,
                AudioChannel = AudioChannel.One,
                AudioBps = metadataConfigurator.AudioBps360P1Channel - 1
            };
            var metadataService1 = new MetadataService(metadataConfigurator, metadata64Bps);
            var metadataService2 = new MetadataService(metadataConfigurator, metadataMore64Bps);
            var metadataService3 = new MetadataService(metadataConfigurator, metadataLess64Bps);

            //Act
            var adjBps64Bps = metadataService1.AdjustAudioBps();
            var adjBpsMore64Bps = metadataService2.AdjustAudioBps();
            var adjBpsLess64Bps = metadataService3.AdjustAudioBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.AudioBps360P1Channel, adjBps64Bps);
            Assert.AreEqual(metadataConfigurator.AudioBps360P1Channel, adjBpsMore64Bps);
            Assert.AreEqual(metadataLess64Bps.AudioBps, adjBpsLess64Bps);
        }

        [TestMethod]
        public void AdjustAudioBpsFor854X480And640X360AndLessX2ChannelSuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata64Bps = new VideoMetadata()
            {
                Width = 640,
                Height = 360,
                AudioChannel = AudioChannel.Two,
                AudioBps = metadataConfigurator.AudioBps360P2Channel
            };
            var metadataMore64Bps = new VideoMetadata()
            {
                Width = 640,
                Height = 360,
                AudioChannel = AudioChannel.Two,
                AudioBps = metadataConfigurator.AudioBps360P2Channel + 1
            };
            var metadataLess64Bps = new VideoMetadata()
            {
                Width = 640,
                Height = 360,
                AudioChannel = AudioChannel.Two,
                AudioBps = metadataConfigurator.AudioBps360P2Channel - 1
            };
            var metadataService1 = new MetadataService(metadataConfigurator, metadata64Bps);
            var metadataService2 = new MetadataService(metadataConfigurator, metadataMore64Bps);
            var metadataService3 = new MetadataService(metadataConfigurator, metadataLess64Bps);

            //Act
            var adjBps64Bps = metadataService1.AdjustAudioBps();
            var adjBpsMore64Bps = metadataService2.AdjustAudioBps();
            var adjBpsLess64Bps = metadataService3.AdjustAudioBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.AudioBps360P2Channel, adjBps64Bps);
            Assert.AreEqual(metadataConfigurator.AudioBps360P2Channel, adjBpsMore64Bps);
            Assert.AreEqual(metadataLess64Bps.AudioBps, adjBpsLess64Bps);
        }

        [TestMethod]
        public void AdjustAudioBpsFor854X480And640X360AndLessX6ChannelSuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadata64Bps = new VideoMetadata()
            {
                Width = 640,
                Height = 360,
                AudioChannel = AudioChannel.Six,
                AudioBps = metadataConfigurator.AudioBps360P6Channel
            };
            var metadataMore64Bps = new VideoMetadata()
            {
                Width = 640,
                Height = 360,
                AudioChannel = AudioChannel.Six,
                AudioBps = metadataConfigurator.AudioBps360P6Channel + 1
            };
            var metadataLess64Bps = new VideoMetadata()
            {
                Width = 640,
                Height = 360,
                AudioChannel = AudioChannel.Six,
                AudioBps = metadataConfigurator.AudioBps360P6Channel - 1
            };
            var metadataService1 = new MetadataService(metadataConfigurator, metadata64Bps);
            var metadataService2 = new MetadataService(metadataConfigurator, metadataMore64Bps);
            var metadataService3 = new MetadataService(metadataConfigurator, metadataLess64Bps);

            //Act
            var adjBps64Bps = metadataService1.AdjustAudioBps();
            var adjBpsMore64Bps = metadataService2.AdjustAudioBps();
            var adjBpsLess64Bps = metadataService3.AdjustAudioBps();

            //Assert
            Assert.AreEqual(metadataConfigurator.AudioBps360P6Channel, adjBps64Bps);
            Assert.AreEqual(metadataConfigurator.AudioBps360P6Channel, adjBpsMore64Bps);
            Assert.AreEqual(metadataLess64Bps.AudioBps, adjBpsLess64Bps);
        }

        [TestMethod]
        public void AdjustVideoKeyFrameSuccessTest()
        {
            //Arrange
            var metadataConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            var metadataKeyFrame0 = new VideoMetadata()
            {
                VideoKeyFrame = metadataConfigurator.MinKeyFrame - 1
            };
            var metadataKeyFrameMore60 = new VideoMetadata()
            {
                VideoKeyFrame = metadataConfigurator.MaxKeyFrame + 1
            };
            var metadataKeyFrame = new VideoMetadata()
            {
                VideoKeyFrame = 30
            };

            var metadataService1 = new MetadataService(metadataConfigurator, metadataKeyFrame0);
            var metadataService2 = new MetadataService(metadataConfigurator, metadataKeyFrameMore60);
            var metadataService3 = new MetadataService(metadataConfigurator, metadataKeyFrame);

            //Act;
            var keyFrame0 = metadataService1.AdjustKeyFrame();
            var keyFrameMore60 = metadataService2.AdjustKeyFrame();
            var keyFrame = metadataService3.AdjustKeyFrame();

            //Assert
            Assert.AreEqual(metadataConfigurator.KeyFrame, keyFrame0);
            Assert.AreEqual(metadataConfigurator.KeyFrame, keyFrameMore60);
            Assert.AreEqual(metadataKeyFrame.VideoKeyFrame, keyFrame);
        }

        [TestMethod]
        public void AdjustContainerTest()
        {
            //Arrange
            const string myContainer = "myContainer";
            const string anotherContainer = "anotherContainer";
            var serviceConfigurator = new MetadataServiceConfigurator(myContainer, null, null, null, null);

            var metadata1 = new VideoMetadata()
            {
                Container = myContainer
            };
            var metadata2 = new VideoMetadata()
            {
                Container = anotherContainer
            };
            var metadataWithoutContainer = new VideoMetadata()
            {
                Container = null
            };

            var metadataService1 = new MetadataService(serviceConfigurator, metadata1);
            var metadataService2 = new MetadataService(serviceConfigurator, metadata2);
            var metadataService3 = new MetadataService(serviceConfigurator, metadataWithoutContainer);

            //Act
            var container1 = metadataService1.AdjustContainer();
            var container2 = metadataService2.AdjustContainer();

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

            var metadata1 = new VideoMetadata()
            {
                VideoCodec = myCodecName
            };
            var metadata2 = new VideoMetadata()
            {
                VideoCodec = anotherCodecName
            };
            var metadataWithoutCodec = new VideoMetadata()
            {
                VideoCodec = null
            };

            var metadataService1 = new MetadataService(serviceConfigurator, metadata1);
            var metadataService2 = new MetadataService(serviceConfigurator, metadata2);
            var metadataService3 = new MetadataService(serviceConfigurator, metadataWithoutCodec);

            //Act
            var codec1 = metadataService1.AdjustVideoCodec();
            var codec2 = metadataService2.AdjustVideoCodec();

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

            var metadata1 = new VideoMetadata()
            {
                VideoCodec = myCodec,
                VideoProfile = firstProfile
            };
            var metadata2 = new VideoMetadata()
            {
                VideoCodec = myCodec,
                VideoProfile = secondProfile
            };
            var metadata3 = new VideoMetadata()
            {
                VideoCodec = myCodec,
                VideoProfile = anotherProfile
            };
            var metadataWithoutCodec = new VideoMetadata()
            {
                VideoCodec = myCodec,
                VideoProfile = null
            };

            var metadataService1 = new MetadataService(serviceConfigurator, metadata1);
            var metadataService2 = new MetadataService(serviceConfigurator, metadata2);
            var metadataService3 = new MetadataService(serviceConfigurator, metadata3);
            var metadataService4 = new MetadataService(serviceConfigurator, metadataWithoutCodec);

            //Act
            var profile1 = metadataService1.AdjustVideoProfile();
            var profile2 = metadataService2.AdjustVideoProfile();
            var profile3 = metadataService3.AdjustVideoProfile();
            var profile4 = metadataService4.AdjustVideoProfile();

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

            var supportedAudioCodecs = new List<CodecData>()
                                           {
                                               audioCodec,
                                               new CodecData(myCodec2, null)
                                           };

            var serviceConfigurator = new MetadataServiceConfigurator(null, null, null, audioCodec, supportedAudioCodecs);

            var metadata1 = new VideoMetadata()
            {
                AudioCodec = myCodec1
            };
            var metadata2 = new VideoMetadata()
            {
                AudioCodec = myCodec2
            };
            var metadata3 = new VideoMetadata()
            {
                AudioCodec = anotherCodec
            };
            var metadataWithoutCodec = new VideoMetadata()
            {
                AudioCodec = null
            };

            var metadataService1 = new MetadataService(serviceConfigurator, metadata1);
            var metadataService2 = new MetadataService(serviceConfigurator, metadata2);
            var metadataService3 = new MetadataService(serviceConfigurator, metadata3);
            var metadataService4 = new MetadataService(serviceConfigurator, metadataWithoutCodec);

            //Act
            var codec1 = metadataService1.AdjustAudioCodec();
            var codec2 = metadataService2.AdjustAudioCodec();
            var codec3 = metadataService3.AdjustAudioCodec();
            var codec4 = metadataService4.AdjustAudioCodec();

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

            var supportedAudioCodecs = new List<CodecData>()
                                           {
                                               audioCodec,
                                               new CodecData(myCodec2,null)
                                           };

            var serviceConfigurator = new MetadataServiceConfigurator(null, null, null, audioCodec, supportedAudioCodecs);

            var metadata1 = new VideoMetadata()
            {
                AudioCodec = myCodec1,
                AudioProfile = firstProfile
            };
            var metadata2 = new VideoMetadata()
            {
                AudioCodec = myCodec1,
                AudioProfile = anotherProfile
            };
            var metadata3 = new VideoMetadata()
            {
                AudioCodec = myCodec2,
                AudioProfile = anotherProfile
            };
            var metadataWithoutProfile = new VideoMetadata()
            {
                AudioCodec = myCodec1,
                AudioProfile = null
            };

            var metadataService1 = new MetadataService(serviceConfigurator, metadata1);
            var metadataService2 = new MetadataService(serviceConfigurator, metadata2);
            var metadataService3 = new MetadataService(serviceConfigurator, metadata3);
            var metadataService4 = new MetadataService(serviceConfigurator, metadataWithoutProfile);

            //Act
            var profile1 = metadataService1.AdjustAudioProfile();
            var profile2 = metadataService2.AdjustAudioProfile();
            var profile3 = metadataService3.AdjustAudioProfile();
            var profile4 = metadataService4.AdjustAudioProfile();

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
            
            var metadata1 = new VideoMetadata()
                                {
                                    Duration = (int)serviceConfigurator.ScreenshotTime.TotalMilliseconds - 1
                                };
            var metadata2 = new VideoMetadata()
                                {
                                    Duration = (int)serviceConfigurator.ScreenshotTime.TotalMilliseconds + 1
                                };
            var metadataService1 = new MetadataService(serviceConfigurator, metadata1);
            var metadataService2 = new MetadataService(serviceConfigurator, metadata2);

            //Act
            var time1 = metadataService1.AdjustScreenshotTime();
            var time2 = metadataService2.AdjustScreenshotTime();

            //Assert
            Assert.AreEqual(0, time1);
            Assert.AreEqual((int)serviceConfigurator.ScreenshotTime.Seconds, time2);
        }

        [TestMethod]
        public void CheckCorrectVideoValueListTest()
        {
            //Arrange
            var serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();

            var metadata = new VideoMetadata()
            {
                AudioBps = serviceConfigurator.AudioBps720P1Channel,
                AudioChannel = AudioChannel.One,
                AudioCodec = serviceConfigurator.AudioCodec.Codec,
                AudioProfile = serviceConfigurator.AudioCodec.DefaultProfile,
                VideoBps = serviceConfigurator.VideoBps1920X1080,
                VideoCodec = serviceConfigurator.VideoCodec.Codec,
                VideoProfile = serviceConfigurator.VideoCodec.DefaultProfile,
                VideoFps = serviceConfigurator.MaxFps - 1,
                VideoKeyFrame = serviceConfigurator.KeyFrame,
                Width = 4096,
                Height = 2304
            };

            var metadataService = new MetadataService(serviceConfigurator, metadata);

            //Act
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
            var serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();

            var metadata = new VideoMetadata()
            {
                AudioBps = serviceConfigurator.AudioBps720P1Channel,
                AudioChannel = AudioChannel.One,
                AudioCodec = "AAC",
                AudioProfile = "LC",
                VideoBps = serviceConfigurator.VideoBps1920X1080,
                VideoCodec = "AVC",
                VideoProfile = "Baseline",
                VideoFps = serviceConfigurator.MaxFps - 1,
                VideoKeyFrame = serviceConfigurator.KeyFrame,
                Width = 4096,
                Height = 2304
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
            var serviceConfigurator = Factory.CreateMp4MetadataServiceConfigurator();

            var metadata = new VideoMetadata()
            {
                Container = serviceConfigurator.Container
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
