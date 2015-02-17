using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Data;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Portal.BackEnd.Encoder.Pipeline.Step;
using Portal.Domain.BackendContext.Enum;
using Wrappers;

namespace Portal.BackEnd.Encoder.Test.PipelineTests
{
    [TestClass]
    public class GetTaskStepTest
    {
        [TestMethod]
        public void CreateGetTaskStepTest()
        {
            //Arrange
            var pipelineMediator = new Mock<IStepMediator>();
            var webClient = new Mock<IEncodeWebClient>();

            //Act
            var pipelineStep = new GetTaskStep(pipelineMediator.Object, webClient.Object);

            //Assert
            Assert.IsInstanceOfType(pipelineStep, typeof(PipelineStepBase<StepData>));
            Assert.IsNotInstanceOfType(pipelineStep, typeof(PipelineStep<StepData>));
            pipelineMediator.Verify(m=>m.AddGetTaskStep(pipelineStep), Times.Once());
        }

        [TestMethod]
        public void ExecuteMethodTest()
        {
            //Arrange
            const string id = "id";
            const string resource = "resource";
            const TypeOfTask type = TypeOfTask.Video;

            var taskData = new TaskData()
                               {
                                   Id = id,
                                   Resource = resource,
                                   Type = type
                               };

            var mediator = new Mock<IStepMediator>();
            var webClient = new Mock<IEncodeWebClient>();

            var pipelineStep = new GetTaskStep(mediator.Object, webClient.Object);

            webClient.Setup(m => m.GetTask()).Returns(taskData);

            //Act
            pipelineStep.Execute(It.IsAny<CancellationTokenSourceWrapper>());

            //Assert
            webClient.Verify(m => m.GetTask(), Times.Once());
            mediator.Verify(m => m.Send(It.Is<TaskStepData>(d => d.Resource == resource &&
                                                                 d.TypeOfTask == type &&
                                                                 d.TaskId == id &&
                                                                 d.EncoderState == EncoderState.Completed), pipelineStep), Times.Once());
        }
    }
}
