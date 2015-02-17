using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.Adjusters;
using Portal.BLL.Concrete.Multimedia.Builder;
using Portal.BLL.Concrete.Multimedia.Calculator;
using Portal.BLL.Concrete.Multimedia.Comparator;
using Portal.Domain.BackendContext.Entity;

namespace Portal.BLL.Tests.MultimediaTests
{
    [TestClass]
    public class ProcessedVideoBuilderTest
    {
        [TestMethod]
        public void BuildProcessedVideoTest()
        {
            //Arrange
            const string newContainer = "newContainer";
            const string contentType = "contentType";
            const bool isVideoParamEquals = true;
            const bool isAudioParamEquals = false;
            const int videoWidth = 2134;
           
            var videoAdjusterParam = new VideoAdjusterParam();
            var audioAdjusterParam = new AudioAdjusterParam();

            var videoParam = new VideoParam() { VideoWidth = videoWidth, MediaContainer = newContainer };
            var audioParam = new AudioParam();
            var outputFormat = string.Format("{0}x{1}", videoParam.MediaContainer, videoParam.VideoWidth);
            
            var videoSize = new Mock<IVideoSize>();
            var videoParamAdjuster = new Mock<IVideoAdjuster>();
            var audioParamAdjuster = new Mock<IAudioAdjuster>();
            var comparator = new Mock<IComparator>();

            videoParamAdjuster.Setup(m => m.AdjustVideoParam(videoAdjusterParam, newContainer, videoSize.Object)).Returns(videoParam);
            audioParamAdjuster.Setup(m => m.AdjustAudioParam(audioAdjusterParam, newContainer, videoSize.Object)).Returns(audioParam);
            comparator.Setup(m => m.VideoParamCompare(videoAdjusterParam, videoParam, newContainer, videoSize.Object)).Returns(isVideoParamEquals);
            comparator.Setup(m => m.AudioParamCompare(audioAdjusterParam, audioParam)).Returns(isAudioParamEquals);

            var builder = new ProcessedVideoBuilder(videoParamAdjuster.Object, audioParamAdjuster.Object, comparator.Object);

            //Act
            var processedVideo = builder.BuildProcessedVideo(videoAdjusterParam, audioAdjusterParam, newContainer, videoSize.Object, contentType);

            //Assert
            Assert.AreEqual(videoParam, processedVideo.VideoParam);
            Assert.AreEqual(audioParam, processedVideo.AudioParam);
            Assert.AreEqual(isVideoParamEquals, processedVideo.IsVideoCopy);
            Assert.AreEqual(isAudioParamEquals, processedVideo.IsAudioCopy);
            Assert.AreEqual(outputFormat, processedVideo.OutputFormat);
            Assert.AreEqual(contentType, processedVideo.ContentType);
        }
    }
}
