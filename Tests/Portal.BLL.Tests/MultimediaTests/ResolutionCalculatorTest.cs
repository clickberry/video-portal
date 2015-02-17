using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.BLL.Concrete.Multimedia.Calculator;

namespace Portal.BLL.Tests.MultimediaTests
{
    [TestClass]
    public class ResolutionCalculatorTest
    {
        [TestMethod]
        public void GetSingleOriginalResolutionTest()
        {
            //Arrange
            const int width = 150;
            const int height = 180;
            var calculator = new ResolutionCalculator();

            //Act
            var resolutionList = calculator.Calculate(width, height);

            //Assert
            Assert.AreEqual(1, resolutionList.Count);
            Assert.AreEqual(width, resolutionList[0].Width);
            Assert.AreEqual(height, resolutionList[0].Height);
        }

        [TestMethod]
        public void GetAllResolutionsForHDVideoTest()
        {
            //Arrange
            const int width = 1920;
            const int height = 1080;
            var calculator = new ResolutionCalculator();
            var expectedResolution = new List<IVideoSize>()
                                         {
                                             new VideoSize(640, 360),
                                             new VideoSize(854, 480),
                                             new VideoSize(1280, 720),
                                             new VideoSize(1920, 1080)
                                         };

            //Act
            var resolutionList = calculator.Calculate(width, height);

            //Assert
            Assert.IsTrue(expectedResolution.All(videoSize => resolutionList.Any(s => s.Width == videoSize.Width && s.Height == videoSize.Height)));
        }

        [TestMethod]
        public void GetMaxQuantityResolutionTest()
        {
            //Arrange
            const int width = 19200;
            const int height = 10800;
            var calculator = new ResolutionCalculator();
            var expectedResolution = new List<IVideoSize>()
                                         {
                                             new VideoSize(640, 360),
                                             new VideoSize(854, 480),
                                             new VideoSize(1280, 720),
                                             new VideoSize(1920, 1080),
                                             new VideoSize(19200, 10800)
                                         };

            //Act
            var resolutionList = calculator.Calculate(width, height);

            //Assert
            Assert.IsTrue(expectedResolution.All(videoSize => resolutionList.Any(s => s.Width == videoSize.Width && s.Height == videoSize.Height)));
        }
    }
}
