using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MSTestExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.StorageClient.Protocol;
using Moq;
using Portal.Domain;
using Portal.Repository.Queue;
using PortalProcessManager.Process;

namespace PortalProcessManagerTest
{
    [TestClass]
    public class QueueProcessTest
    {
        [TestMethod]
        public void QueuedSuccessfulTest()
        {
            //Arrange
            var queueMsg = new VideoMessage();
            var queueVideoRepository = new Mock<IQueueVideoRepository>();
            var queueProcess = new QueueProcess(1000, queueVideoRepository.Object);

            queueVideoRepository.Setup(m => m.GetMessage()).Returns(queueMsg);

            //Act
            var queueInfo = queueProcess.ProcessMethod(null, new CancellationToken());

            //Asert
            queueVideoRepository.Verify(m => m.GetMessage(), Times.Once());

            Assert.AreEqual(1, queueProcess.BlockingCollection.Count);
            Assert.AreEqual(queueMsg, queueInfo.VideoMessage);
        }

        [TestMethod]
        public void EmptyQueueTest()
        {
            //Arrange
            var queueVideoRepository = new Mock<IQueueVideoRepository>();
            var queueProcess = new QueueProcess(0, queueVideoRepository.Object);

            queueVideoRepository.Setup(m => m.GetMessage()).Returns((VideoMessage) null);

            //Act
            var queueInfo = queueProcess.ProcessMethod(null, new CancellationToken());

            //Asert
            Assert.AreEqual(null, queueInfo.VideoMessage);
        }

        [TestMethod]
        public void QueueFirstStartTest()
        {
            //Arrange
            var queueProcess = new QueueProcess(0, null);

            //Act
            queueProcess.FirstStart(new CancellationToken());

            //Asert
            Assert.AreEqual(1, queueProcess.BlockingCollection.BoundedCapacity);
            Assert.AreEqual(1, queueProcess.BlockingCollection.Count);
        }

        [TestMethod]
        public void QueueCancellationTokenTest()
        {
            //Arrange
            var token = new CancellationTokenSource();
            var queueProcess = new QueueProcess(0, null);

           token.Cancel();

            //Act & Assert
            CustomAssert.IsThrown<OperationCanceledException>(() => queueProcess.ProcessMethod(null, token.Token));
        }
    }
}
