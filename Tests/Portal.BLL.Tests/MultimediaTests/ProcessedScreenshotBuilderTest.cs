using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.Adjusters;
using Portal.BLL.Concrete.Multimedia.Builder;
using Portal.Domain.BackendContext.Entity;

namespace Portal.BLL.Tests.MultimediaTests
{
    [TestClass]
    public class ProcessedScreenshotBuilderTest
    {
        [TestMethod]
        public void BuildProcessedScreenshotTest()
        {
            //Arrange
            const string imageFormat = "imageFormat";
            const string contentType = "contentType";

            var screenshotAdjuster = new Mock<IScreenshotAdjuster>();
            var builder = new ProcessdScreenshotBuilder(screenshotAdjuster.Object);
            
            var screenshotAdjusterParam = new ScreenshotAdjusterParam();
            var screenshotParm = new ScreenshotParam();

            screenshotAdjuster.Setup(m => m.AdjustScreenshotParam(screenshotAdjusterParam)).Returns(screenshotParm);

            //Act
            var processedScreenshot = builder.BuildProcessedScreenshot(screenshotAdjusterParam, imageFormat, contentType);

            //Assert
            Assert.AreEqual(screenshotParm,processedScreenshot.ScreenshotParam);
            Assert.AreEqual(imageFormat, processedScreenshot.ImageFormat);
            Assert.AreEqual(contentType,processedScreenshot.ContentType);
        }
    }
}
