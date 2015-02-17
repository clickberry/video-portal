using FfmpegBackend.Data;
using FfmpegBackend.EncodeTask;
using FfmpegBackend.Factory;
using FfmpegBackend.Ffmpeg;
using FfmpegBackend.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using RestSharp.Deserializers;
using Wrappers;

namespace FfmpegBackendTest.FactoryTests
{
    [TestClass]
    public class SnapshotEncodeCreatorTest
    {
        [TestMethod]
        public void CreateSnapshotFfmpegTest()
        {
            //Arrange
            var snapshotCreator = new SnapshotEncodeCreator();
            var ffmpegParam = new Mock<IFfmpegParameters>();

            //Act
            var snapshotFfmpeg = snapshotCreator.FfmpegCreate(ffmpegParam.Object, It.IsAny<ProcessWrapper>(), It.IsAny<IWatchDogTimer>());

            //Assert
            Assert.AreEqual(typeof(SnapshotFfmpeg), snapshotFfmpeg.GetType());
        }

        [TestMethod]
        public void CreateSnapshotFfmpegStringTest()
        {
            //Arrange
            var snapshotCreator = new SnapshotEncodeCreator();
            var encodeData = new SnapshotEncodeData();

            //Act
            var snapshotFfmpegString = snapshotCreator.FfmpegParametersCreate(encodeData);
            
            //Assert
            Assert.AreEqual(typeof(SnapshotFfmpegParameters), snapshotFfmpegString.GetType());
        }

        [TestMethod]
        public void CreateSnapshotEncodeTaskTest()
        {
            //Arrange
            var videoCreator = new SnapshotEncodeCreator();

            //Act
            var encodeData = videoCreator.EncodeTaskCreate();

            //Assert
            Assert.AreEqual(typeof(SnapshotEncodeTask), encodeData.GetType());
        }
    }
}