using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Portal.BackEnd.Encoder.Pipeline.Step;
using Wrappers;

namespace Portal.BackEnd.Encoder.Test.PipelineTests
{
    [TestClass]
    public class InitializingWebClientStepTest
    {
        [TestMethod]
        public void CreateConfiguringWebClientStepTest()
        {
            //Arrange
            var pipelineMediator = new Mock<IStepMediator>();
            var webClient = new Mock<IEncodeWebClient>();

            //Act
            var pipelineStep = new InitializingWebClientStep(pipelineMediator.Object, webClient.Object);

            //Assert
            Assert.IsInstanceOfType(pipelineStep, typeof(PipelineStepBase<TaskStepData>));
            Assert.IsInstanceOfType(pipelineStep, typeof(PipelineStepBase<TaskStepData>));
            pipelineMediator.Verify(m => m.AddInitializingWebClientStep(pipelineStep), Times.Once());
        }

        [TestMethod]
        public void ExecuteMethodTest()
        {
            //Arrange
            var stepData = new TaskStepData() { TaskId = "taskId", Resource="resource" };

            var pipelineMediator = new Mock<IStepMediator>();
            var webClient = new Mock<IEncodeWebClient>();
            var tokenSource = new Mock<CancellationTokenSourceWrapper>();

            var pipelineStep = new InitializingWebClientStep(pipelineMediator.Object, webClient.Object);
            pipelineStep.SetData(stepData);

            //Act
            pipelineStep.Execute(tokenSource.Object);

            //Assert
            webClient.Verify(m => m.Initialize(stepData.Resource, stepData.TaskId, tokenSource.Object), Times.Once());
        }
    }
}