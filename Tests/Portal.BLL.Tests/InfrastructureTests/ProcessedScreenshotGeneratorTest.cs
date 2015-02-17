using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Infrastructure.ProcessedEntity;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.Builder;
using Portal.BLL.Concrete.Multimedia.Factory;
using Portal.Domain.BackendContext.Constant;
using Portal.Domain.EncoderContext;
using Portal.Domain.ProcessedVideoContext;

namespace Portal.BLL.Tests.InfrastructureTests
{
    [TestClass]
    public class ProcessedScreenshotGeneratorTest
    {
        [TestMethod]
        public void GenerateProcessedScreenshotTest()
        {
            //Arrange
            var paramFactory = new Mock<IScreenshotAdjusterParamFactory>();
            var builder = new Mock<IProcessedScreenshotBuilder>();
            var generator = new ProcessedScreenshotGenerator(paramFactory.Object, builder.Object);

            var metadata = new Mock<IVideoMetadata>();
            var scrrenshotAdjusterParam = new ScreenshotAdjusterParam();
            var processedScreenshot = new DomainProcessedScreenshot();

            paramFactory.Setup(m => m.CreateScreenshotAdjusterParam(metadata.Object)).Returns(scrrenshotAdjusterParam);
            builder.Setup(m => m.BuildProcessedScreenshot(scrrenshotAdjusterParam, MetadataConstant.JpegFormat, ContentType.JpegContent)).Returns(processedScreenshot);

            //Act
            List<DomainProcessedScreenshot> list = generator.Generate(metadata.Object);

            //Assert
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Any(p => p == processedScreenshot));
        }
    }
}