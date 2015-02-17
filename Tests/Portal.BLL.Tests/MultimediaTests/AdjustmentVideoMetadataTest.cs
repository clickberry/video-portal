using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.BLL.Concrete.Multimedia.AdjustmentParameters;
using Portal.BLL.Concrete.Multimedia.Constraints;
using Portal.Domain.BackendContext.Constant;
using Portal.Exceptions.Multimedia;
using TestExtension;

namespace Portal.BLL.Tests.MultimediaTests
{
    [TestClass]
    public class AdjustmentVideoMetadataTest
    {
        private VideoConstraints _constraint;
        private AdjustmentVideoMetadata _adjustmentVideo;

        [TestInitialize]
        public void Initialize()
        {
            _constraint = new VideoConstraints();
            _adjustmentVideo = new AdjustmentVideoMetadata(_constraint);
        }

        [TestMethod]
        public void AdjustWidthSuccessTest()
        {
            //Assert
            const int val = 20;
            var width = _constraint.Mul * val;

            //Act
            var newWidth = _adjustmentVideo.AdjustVideoWidth(width);

            //Assert
            Assert.AreEqual(width,newWidth);
        }

        [TestMethod]
        public void AdjustWidthMultiplyMulConstraintTest()
        {
            //Arrange
            const int val = 33;
            var width = _constraint.Mul * val-1;

            //Act
            var newWidth = _adjustmentVideo.AdjustVideoWidth(width);

            //Assert
            Assert.AreEqual(_constraint.Mul*(val-1),newWidth);
        }

        [TestMethod]
        public void AdjustWidthMoreMaxWidthTest()
        {
            //Arrange
            var width = _constraint.MaxWidth + 1;

            //Act & Assert
            var exception = ExceptionAssert.Throws<VideoFormatException>(() => _adjustmentVideo.AdjustVideoWidth(width));

            //Assert
            Assert.AreEqual(ParamType.Width, exception.ParamType);
        }

        [TestMethod]
        public void AdjustWidthLessWidthTest()
        {
            //Arrange
            var width = _constraint.MinWidth - 1;

            //Act & Assert
            var exception = ExceptionAssert.Throws<VideoFormatException>(() => _adjustmentVideo.AdjustVideoWidth(width));

            //Assert
            Assert.AreEqual(ParamType.Width, exception.ParamType);
        }
        
        [TestMethod]
        public void AdjustHeightSuccessTest()
        {
            //Assert
            const int val = 20;
            var height = _constraint.Mul * val;

            //Act
            var adjustHeight = _adjustmentVideo.AdjustVideoHeight(height);

            //Assert
            Assert.AreEqual(height, adjustHeight);
        }

        [TestMethod]
        public void AdjustHeightMultiplyMulConstraintTest()
        {
            //Arrange
            const int val = 33;
            var height = _constraint.Mul * val - 1;

            //Act
            var adjustHeight = _adjustmentVideo.AdjustVideoHeight(height);

            //Assert
            Assert.AreEqual(_constraint.Mul * (val - 1), adjustHeight);
        }

        [TestMethod]
        public void AdjustHeightMoreMaxWidthTest()
        {
            //Arrange
            var height = _constraint.MaxHeight + 1;

            //Act & Assert
            var exception = ExceptionAssert.Throws<VideoFormatException>(() => _adjustmentVideo.AdjustVideoHeight(height));

            //Assert
            Assert.AreEqual(ParamType.Height, exception.ParamType);
        }

        [TestMethod]
        public void AdjustHeightLessWidthTest()
        {
            //Arrange
            var height = _constraint.MinHeight - 1;

            //Act & Assert
            var exception = ExceptionAssert.Throws<VideoFormatException>(() => _adjustmentVideo.AdjustVideoHeight(height));

            //Assert
            Assert.AreEqual(ParamType.Height, exception.ParamType);
        }

        [TestMethod]
        public void AdjustVideoBitrateForMoreOrEqual1080PTest()
        {
            //Arrange
            const uint sizeDelta = 26;
            const uint bitrateDelta = 346787;

            var size = (int)(_constraint.Size1080P + sizeDelta);
            var bitrate1 = (int)(_constraint.MaxBitrate1080P + bitrateDelta);
            var bitrate2 = (int)(_constraint.MaxBitrate1080P - bitrateDelta - 1);

            //Act
            var adjustBitrate1 = _adjustmentVideo.AdjustVideoBitrate(size, bitrate1);
            var adjustBitrate2 = _adjustmentVideo.AdjustVideoBitrate(size, bitrate2);

            //Assert
            Assert.AreEqual(_constraint.MaxBitrate1080P, adjustBitrate1);
            Assert.AreEqual(bitrate2, adjustBitrate2);
        }

        [TestMethod]
        public void AdjustVideoBitrateForMoreOrEqual720PTest()
        {
            //Arrange
            const uint sizeDelta = 446468;
            const uint bitrateDelta = 4054550;

            var size = (int)(_constraint.Size720P + sizeDelta);
            var bitrate1 = (int)(_constraint.MaxBitrate720P + bitrateDelta);
            var bitrate2 = (int)(_constraint.MaxBitrate720P - bitrateDelta - 1);

            //Act
            var adjustBitrate1 = _adjustmentVideo.AdjustVideoBitrate(size, bitrate1);
            var adjustBitrate2 = _adjustmentVideo.AdjustVideoBitrate(size, bitrate2);

            //Assert
            Assert.AreEqual(_constraint.MaxBitrate720P, adjustBitrate1);
            Assert.AreEqual(bitrate2, adjustBitrate2);
        }

        [TestMethod]
        public void AdjustVideoBitrateForMoreOrEqual480PTest()
        {
            //Arrange
            const uint sizeDelta = 446468;
            const uint bitrateDelta = 0;

            var size = (int)(_constraint.Size480P + sizeDelta);
            var bitrate1 = (int)(_constraint.MaxBitrate480P + bitrateDelta);
            var bitrate2 = (int)(_constraint.MaxBitrate480P - bitrateDelta - 1);

            //Act
            var adjustBitrate1 = _adjustmentVideo.AdjustVideoBitrate(size, bitrate1);
            var adjustBitrate2 = _adjustmentVideo.AdjustVideoBitrate(size, bitrate2);

            //Assert
            Assert.AreEqual(_constraint.MaxBitrate480P, adjustBitrate1);
            Assert.AreEqual(bitrate2, adjustBitrate2);
        }

        [TestMethod]
        public void AdjustVideoBitrateForLessThan480PTest()
        {
            //Arrange
            const uint sizeDelta = 0;
            const uint bitrateDelta = 457530;

            var size = (int)(_constraint.Size480P - sizeDelta - 1);
            var bitrate1 = (int)(_constraint.DefaultBitrate + bitrateDelta);
            var bitrate2 = (int)(_constraint.DefaultBitrate - bitrateDelta - 1);

            //Act
            var adjustBitrate1 = _adjustmentVideo.AdjustVideoBitrate(size, bitrate1);
            var adjustBitrate2 = _adjustmentVideo.AdjustVideoBitrate(size, bitrate2);

            //Assert
            Assert.AreEqual(_constraint.DefaultBitrate, adjustBitrate1);
            Assert.AreEqual(bitrate2, adjustBitrate2);
        }

        [TestMethod]
        public void AdjustVideoBitrateThrowExceptionTest()
        {
            //Arrange
            const int bitrate = 0;
            var badBitrate = Math.Abs(bitrate)*-1;

            //Act & Assert
            var exception = ExceptionAssert.Throws<VideoFormatException>(() => _adjustmentVideo.AdjustVideoBitrate(123, badBitrate));

            //Act
            Assert.AreEqual(ParamType.VideoBitRate, exception.ParamType);
        }

        [TestMethod]
        public void AdjustConstantFrameRateTest()
        {
            //Arrange
            var frameRate = _constraint.MaxFrameRate-1;

            //Act
            var adjustFrameRate = _adjustmentVideo.AdjustFrameRate(frameRate, MetadataConstant.ConstantFrameRate);

            //Assert
            Assert.AreEqual(frameRate, adjustFrameRate);
        }

        [TestMethod]
        public void AdjustConstantFrameRateOutsideOfMaxAndMinTest()
        {
            //Arrange
            var badFrameRate1 = _constraint.MaxFrameRate + 1;
            var badFrameRate2 = _constraint.MinFrameRate - 1;

            //Act
            var adjustFrameRate1 = _adjustmentVideo.AdjustFrameRate(badFrameRate1, MetadataConstant.ConstantFrameRate);
            var adjustFrameRate2 = _adjustmentVideo.AdjustFrameRate(badFrameRate2, MetadataConstant.ConstantFrameRate);

            //Assert
            Assert.AreEqual(_constraint.MaxFrameRate, adjustFrameRate1);
            Assert.AreEqual(_constraint.MinFrameRate, adjustFrameRate2);
        }

        [TestMethod]
        public void AdjustVariableFrameRateTest()
        {
            //Arrange
            var frameRate = _constraint.MaxFrameRate - 1;

            //Act
            var adjustFrameRate1 = _adjustmentVideo.AdjustFrameRate(0, MetadataConstant.VariableFrameRate);
            var adjustFrameRate2 = _adjustmentVideo.AdjustFrameRate(frameRate, MetadataConstant.VariableFrameRate);

            //Assert
            Assert.AreEqual(_constraint.FrameRate, adjustFrameRate1);
            Assert.AreEqual(frameRate, adjustFrameRate2);
        }

        [TestMethod]
        public void AdjustKeyFrameRateTest()
        {
            //Arrange
            var badKeyFrameRate1 = _constraint.MaxKeyFrameRate + 1;
            var badKeyFrameRate2 = _constraint.MinKeyFrameRate - 1;
            var rightKeyFrameRate = _constraint.MaxKeyFrameRate - 1;

            //Act
            var adjustRightKeyFrameRate = _adjustmentVideo.AdjustKeyFrameRate(rightKeyFrameRate);
            var adjustBadKeyFrameRate1 = _adjustmentVideo.AdjustKeyFrameRate(badKeyFrameRate1);
            var adjustBadKeyFrameRate2 = _adjustmentVideo.AdjustKeyFrameRate(badKeyFrameRate2);

            //Assert
            Assert.AreEqual(rightKeyFrameRate, adjustRightKeyFrameRate);
            Assert.AreEqual(_constraint.DefaultKeyFrameRate, adjustBadKeyFrameRate1);
            Assert.AreEqual(_constraint.DefaultKeyFrameRate, adjustBadKeyFrameRate2);
        }

        [TestMethod]
        public void AdjustCodecForMp4Test()
        {
            //Arrange
            const string anyCodec = "anyCodec";

            //Act
            var adjustCodec = _adjustmentVideo.AdjustVideoCodec(MetadataConstant.Mp4Container, anyCodec);

            //Assert
            Assert.AreEqual(MetadataConstant.AvcCodec, adjustCodec);
        }

        [TestMethod]
        public void AdjustCodecForWebmTest()
        {
            //Arrange
            const string anyCodec = "anyCodec";

            //Act
            var adjustCodec = _adjustmentVideo.AdjustVideoCodec(MetadataConstant.WebmContainer, anyCodec);

            //Assert
            Assert.AreEqual(MetadataConstant.Vp8Codec, adjustCodec);
        }

        [TestMethod]
        public void AdjustCodecThrowExceptionTest()
        {
            //Act & Assert
            var exception = ExceptionAssert.Throws<VideoFormatException>(() => _adjustmentVideo.AdjustVideoCodec(MetadataConstant.WebmContainer, null));

            //Assert
            Assert.AreEqual(ParamType.VideoCodec, exception.ParamType);
        }

        [TestMethod]
        public void AdjustContainerTest()
        {
            //Arrange
            const string currentContainer = "currentContainer";
            const string newContainer = "newContainer";

            //Act
            var adjustContainer = _adjustmentVideo.AdjustMediaContainer(currentContainer, newContainer);

            //Assert
            Assert.AreEqual(adjustContainer,newContainer);
        }

        [TestMethod]
        public void AdjustContainerThrowExceptionTest()
        {
            //Arrange
            const string newContainer = "newContainer";

            //Act & Assert
            var exception = ExceptionAssert.Throws<VideoFormatException>(()=>_adjustmentVideo.AdjustMediaContainer(null, newContainer));

            //Assert
            Assert.AreEqual(ParamType.MediaContainer, exception.ParamType);
        }


        [TestMethod]
        public void AdjustProfileForMp4Test()
        {
            //Arrange
            const string anyProfile = "anyProfile";

            //Act
            var adjustAnyProfile = _adjustmentVideo.AdjustVideoProfile(MetadataConstant.Mp4Container, anyProfile);
            var adjustBaselineProfile = _adjustmentVideo.AdjustVideoProfile(MetadataConstant.Mp4Container, MetadataConstant.AvcBaselineProfile);
            var adjustMainProfile = _adjustmentVideo.AdjustVideoProfile(MetadataConstant.Mp4Container, MetadataConstant.AvcMainProfile);

            //Assert
            Assert.AreEqual(MetadataConstant.AvcMainProfile, adjustAnyProfile);
            Assert.AreEqual(MetadataConstant.AvcBaselineProfile, adjustBaselineProfile);
            Assert.AreEqual(MetadataConstant.AvcMainProfile, adjustMainProfile);
        }

        [TestMethod]
        public void AdjustProfileForWebmTest()
        {
            //Arrange
            const string anyProfile = "anyProfile";

            //Act
            var adjustAnyProfile = _adjustmentVideo.AdjustVideoProfile(MetadataConstant.WebmContainer, anyProfile);

            //Assert
            Assert.IsNull(adjustAnyProfile);
        }

        [TestMethod]
        public void AdjustVideoRotateSizeTest()
        {
            //Arrange 
            const int width = 200;
            const int height = 100;
            const double videoRotation0 = 0;
            const double videoRotation90 = 90;
            const double videoRotation180 = 180;
            const double videoRotation270 = 270;

            //Act
            var size1 = _adjustmentVideo.AdjustVideoRotateSize(width, height, videoRotation0);
            var size2 = _adjustmentVideo.AdjustVideoRotateSize(width, height, videoRotation90);
            var size3 = _adjustmentVideo.AdjustVideoRotateSize(width, height, videoRotation180);
            var size4 = _adjustmentVideo.AdjustVideoRotateSize(width, height, videoRotation270);

            //Assert
            Assert.AreEqual(width, size1.Width);
            Assert.AreEqual(height, size1.Height);

            Assert.AreEqual(width, size2.Height);
            Assert.AreEqual(height, size2.Width);

            Assert.AreEqual(width, size3.Width);
            Assert.AreEqual(height, size3.Height);

            Assert.AreEqual(width, size4.Height);
            Assert.AreEqual(height, size4.Width);
        }
    }
}
