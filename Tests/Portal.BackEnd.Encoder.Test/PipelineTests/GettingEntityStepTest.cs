using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Portal.BackEnd.Encoder.Pipeline.Step;
using Portal.Domain.BackendContext.Entity.Base;
using Portal.Domain.BackendContext.Enum;
using Wrappers;

namespace Portal.BackEnd.Encoder.Test.PipelineTests
{
    [TestClass]
    public class GettingEntityStepTest
    {
        [TestMethod]
        public void CreateGettingEntityStepTest()
        {
            //Arrange
            var pipelineMediator = new Mock<IStepMediator>();
            var webClient = new Mock<IEncodeWebClient>();

            //Act
            var pipelineStep = new GettingEntityStep(pipelineMediator.Object, webClient.Object);

            //Assert
            Assert.IsInstanceOfType(pipelineStep, typeof(PipelineStepBase<TaskStepData>));
            Assert.IsInstanceOfType(pipelineStep, typeof(PipelineStepBase<TaskStepData>));
            pipelineMediator.Verify(m => m.AddGettingEntityStep(pipelineStep), Times.Once());
        }

        [TestMethod]
        public void ExecouteTest()
        {
            //Arrange
            const TypeOfTask typeOfTask = TypeOfTask.Video;

            var pipelineMediator = new Mock<IStepMediator>();
            var webClient = new Mock<IEncodeWebClient>();

            var pipelineStep = new GettingEntityStep(pipelineMediator.Object, webClient.Object);
            var stepData=new TaskStepData()
                {
                    TypeOfTask = typeOfTask
                };
            var encodeData = new Mock<IEncodeData>();

            pipelineMediator.Setup(m => m.Send(stepData, pipelineStep));
            webClient.Setup(m => m.GetEntity(typeOfTask)).Returns(encodeData.Object);

            pipelineStep.SetData(stepData);

            //Act
            pipelineStep.Execute(It.IsAny<CancellationTokenSourceWrapper>());

            //Assert
            pipelineMediator.Verify(m => m.Send(It.Is<GettingEntityStepData>(d => d.EncodeData == encodeData.Object &&
                                                                                  d.EncoderState == EncoderState.Completed), pipelineStep));
        }
    }
}
