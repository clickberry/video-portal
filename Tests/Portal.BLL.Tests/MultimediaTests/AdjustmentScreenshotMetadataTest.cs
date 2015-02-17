using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.BLL.Concrete.Multimedia.AdjustmentParameters;
using Portal.BLL.Concrete.Multimedia.Constraints;
using Portal.Exceptions.Multimedia;
using TestExtension;

namespace Portal.BLL.Tests.MultimediaTests
{
    [TestClass]
    public class AdjustmentScreenshotMetadataTest
    {
        private ScreenshotConstraints _constraints;
        private AdjustmentScreenshotMetadata _adjustmentScreenshotMetadata;

        [TestInitialize]
        public void Initialize()
        {
            _constraints = new ScreenshotConstraints();
            _adjustmentScreenshotMetadata = new AdjustmentScreenshotMetadata(_constraints);
        }

        [TestMethod]
        public void AdjustScreenshotWidthTest()
        {
            //Arrange
            const int width = 123;

            //Act
            var adjustWidth = _adjustmentScreenshotMetadata.AdjustScreenshotWidth(width);
            
            //Assert
            Assert.AreEqual(width, adjustWidth);
        }

        [TestMethod]
        public void AdjustScreenshotWidthMoreMaxWidthTest()
        {
            //Arrange
            var width = _constraints.MaxWidth + 1;

            //Act & Assert
            var exception = ExceptionAssert.Throws<VideoFormatException>(() => _adjustmentScreenshotMetadata.AdjustScreenshotWidth(width));

            //Assert
            Assert.AreEqual(ParamType.Width, exception.ParamType);
        }

        [TestMethod]
        public void AdjustScreenshotWidthLessWidthTest()
        {
            //Arrange
            var width = _constraints.MinWidth - 1;

            //Act & Assert
            var exception = ExceptionAssert.Throws<VideoFormatException>(() => _adjustmentScreenshotMetadata.AdjustScreenshotWidth(width));

            //Assert
            Assert.AreEqual(ParamType.Width, exception.ParamType);
        }

        [TestMethod]
        public void AdjustScreenshotHeightTest()
        {
            //Arrange
            const int height = 123;

            //Act
            var adjustHeight = _adjustmentScreenshotMetadata.AdjustScreenshotHeight(height);

            //Assert
            Assert.AreEqual(height, adjustHeight);
        }

        [TestMethod]
        public void AdjustScreenshotHeightMoreMaxHeightTest()
        {
            //Arrange
            var height = _constraints.MaxWidth + 1;

            //Act & Assert
            var exception = ExceptionAssert.Throws<VideoFormatException>(() => _adjustmentScreenshotMetadata.AdjustScreenshotHeight(height));

            //Assert
            Assert.AreEqual(ParamType.Height, exception.ParamType);
        }

        [TestMethod]
        public void AdjustScreenshotHeightLessHeightTest()
        {
            //Arrange
            var height = _constraints.MinWidth - 1;

            //Act & Assert
            var exception = ExceptionAssert.Throws<VideoFormatException>(() => _adjustmentScreenshotMetadata.AdjustScreenshotHeight(height));

            //Assert
            Assert.AreEqual(ParamType.Height, exception.ParamType);
        }

        [TestMethod]
        public void AdjustOTimeOffsetTest()
        {
            //Arrange
            const double duration1 = 100;
            const double duration2 = 30;
            const double offsetTime1 = 30;
            const double offsetTime2 = 9;

            //Act
            var adjustTimeOffset1 = _adjustmentScreenshotMetadata.AdjustScreenshotTimeOffset(duration1);
            var adjustTimeOffset2 = _adjustmentScreenshotMetadata.AdjustScreenshotTimeOffset(duration2);

            //Act
            Assert.AreEqual(offsetTime1, adjustTimeOffset1);
            Assert.AreEqual(offsetTime2, adjustTimeOffset2);
        }
    }
}
