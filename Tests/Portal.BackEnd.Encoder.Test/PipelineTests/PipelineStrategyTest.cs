using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline;
using Portal.BackEnd.Encoder.Pipeline.Step;
using Portal.BackEnd.Encoder.Settings;
using Portal.DAL.FileSystem;
using Wrappers.Interface;

namespace Portal.BackEnd.Encoder.Test.PipelineTests
{
    [TestClass]
    public class PipelineStrategyTest
    {
        [TestMethod]
        public void CreateStepsTest()
        {
            //Arrange
            var stepMediator = new Mock<IStepMediator>();
            var webClient = new Mock<IEncodeWebClient>();
            var creatorFactory = new Mock<IEncodeCreatorFactory>();
            var ffmpeg = new Mock<IFfmpeg>();
            var watchDogTimer = new Mock<IWatchDogTimer>();
            var storagePdrovider = new Mock<IFileSystem>();
            var tempFileManager = new Mock<ITempFileManager>();
            var fileWrapper = new Mock<IFileWrapper>();

            var uploadSettings = new UploadSettings("backendId");

            var strategy = new PipelineStrategy(stepMediator.Object, webClient.Object, creatorFactory.Object,
                                                ffmpeg.Object, watchDogTimer.Object, storagePdrovider.Object,
                                                tempFileManager.Object, uploadSettings, fileWrapper.Object);

            //Act
            var stepList = strategy.CreateSteps().ToList();

            //Assert
            Assert.IsInstanceOfType(stepList[0], typeof (GetTaskStep));
            Assert.IsInstanceOfType(stepList[1], typeof (InitializingWebClientStep));
            Assert.IsInstanceOfType(stepList[2], typeof (GettingEntityStep));
            Assert.IsInstanceOfType(stepList[3], typeof (DownloadStep));
            Assert.IsInstanceOfType(stepList[4], typeof (CreatorStep));
            Assert.IsInstanceOfType(stepList[5], typeof (EncodeStep));
            Assert.IsInstanceOfType(stepList[6], typeof (UploadStep));
            Assert.IsInstanceOfType(stepList[7], typeof (FinishStep));
        }
    }
}
