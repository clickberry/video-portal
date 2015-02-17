using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiddleEnd.Worker.Abstract;
using MiddleEnd.Worker.Entities;
using Moq;
using Portal.Domain.SchedulerContext;

namespace MiddleEnd.Worker.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var managerMock = new Mock<ITaskKeeper>();
            managerMock.Setup(p => p.GetNext(It.IsAny<List<ProcessedEntityType>>())).Returns(() => new ManagerTask());

            // Act
            ManagerTask result = managerMock.Object.GetNext(new List<ProcessedEntityType>
                {
                    ProcessedEntityType.Video,
                    ProcessedEntityType.Screenshot
                });

            // Assert
            Assert.IsNotNull(result);
        }
    }
}