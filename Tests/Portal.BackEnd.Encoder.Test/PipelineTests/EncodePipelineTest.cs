using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline;
using Wrappers;

namespace Portal.BackEnd.Encoder.Test.PipelineTests
{
    [TestClass]
    public class EncodePipelineTest
    {
        [TestMethod]
        public void RunMethodTest()
        {
            //Arrange
            var pipelineStrategy = new Mock<IPipelineStrategy>();
            var tokenSourceFactory = new Mock<ITokenSourceFactory>();

            var step1 = new Mock<IPipelineStep>();
            var step2 = new Mock<IPipelineStep>();
            var tokenSource = new Mock<CancellationTokenSourceWrapper>();

            var pipeline = new EncodePipeline(pipelineStrategy.Object, tokenSourceFactory.Object);

            step1.Setup(m => m.CanExecute()).Returns(true);
            step2.Setup(m => m.CanExecute()).Returns(true);
            pipelineStrategy.Setup(m => m.CreateSteps()).Returns(new List<IPipelineStep>() {step1.Object, step2.Object});
            tokenSourceFactory.Setup(m => m.CreateTokenSource()).Returns(tokenSource.Object);

            //Act
            pipeline.Run();

            //Assert
            step1.Verify(m => m.Execute(tokenSource.Object), Times.Once());
            step2.Verify(m => m.Execute(tokenSource.Object), Times.Once());
        }

        [TestMethod]
        public void CanNotExecuteStepTest()
        {
            //Arrange
            var pipelineStrategy = new Mock<IPipelineStrategy>();
            var tokenSourceFactory = new Mock<ITokenSourceFactory>();

            var step1 = new Mock<IPipelineStep>();
            var step2 = new Mock<IPipelineStep>();
            var tokenSource = new Mock<CancellationTokenSourceWrapper>();

            var pipeline = new EncodePipeline(pipelineStrategy.Object, tokenSourceFactory.Object);

            step1.Setup(m => m.CanExecute()).Returns(false);
            step2.Setup(m => m.CanExecute()).Returns(false);
            pipelineStrategy.Setup(m => m.CreateSteps()).Returns(new List<IPipelineStep>() { step1.Object, step2.Object });
            tokenSourceFactory.Setup(m => m.CreateTokenSource()).Returns(tokenSource.Object);

            //Act
            pipeline.Run();

            //Assert
            step1.Verify(m => m.Execute(tokenSource.Object), Times.Never());
            step2.Verify(m => m.Execute(tokenSource.Object), Times.Never());
        }
    }
}
