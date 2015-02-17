using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Portal.BackEnd.Encoder.Pipeline.Step;
using Portal.Domain.BackendContext.Enum;
using Wrappers;

namespace Portal.BackEnd.Encoder.Test.PipelineTests
{
    [TestClass]
    public class PipelineStepTest
    {
        [TestMethod]
        public void CanExecutTest()
        {
            //Arrange
            var mediator = new Mock<IStepMediator>();
            var webClient = new Mock<IEncodeWebClient>();

            var stepData = new StepData()
                {
                    EncoderState = EncoderState.Completed
                };
            var pipelineStep = new PipelineStepStub(mediator.Object, webClient.Object);

            pipelineStep.SetData(stepData);

            //Act
            var canExecute = pipelineStep.CanExecute();

            //Assert
            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void CanNotExecutTest()
        {
            //Arrange
            const string message = "message";

            var mediator1 = new Mock<IStepMediator>();
            var mediator2 = new Mock<IStepMediator>();
            var mediator3 = new Mock<IStepMediator>();
            var webClient = new Mock<IEncodeWebClient>();

            var stepData1 = new StepData() { EncoderState = EncoderState.Failed, ErrorMessage = message };
            var stepData2 = new StepData() { EncoderState = EncoderState.Cancelled, ErrorMessage = message };
            var stepData3 = new StepData() { EncoderState = EncoderState.Hanging, ErrorMessage = message };

            var pipelineStep1 = new PipelineStepStub(mediator1.Object, webClient.Object);
            var pipelineStep2 = new PipelineStepStub(mediator2.Object, webClient.Object);
            var pipelineStep3 = new PipelineStepStub(mediator3.Object, webClient.Object);

            pipelineStep1.SetData(stepData1);
            pipelineStep2.SetData(stepData2);
            pipelineStep3.SetData(stepData3);

            //Act
            var canExecute1 = pipelineStep1.CanExecute();
            var canExecute2 = pipelineStep2.CanExecute();
            var canExecute3 = pipelineStep3.CanExecute();

            //Assert
            Assert.IsFalse(canExecute1);
            Assert.IsFalse(canExecute2);
            Assert.IsFalse(canExecute3);
            mediator1.Verify(m => m.Send(It.Is<StepData>(d => d.EncoderState == EncoderState.Failed &&
                                                                  d.ErrorMessage == message), pipelineStep1), Times.Once());
            mediator2.Verify(m => m.Send(It.Is<StepData>(d => d.EncoderState == EncoderState.Cancelled &&
                                                                  d.ErrorMessage == message), pipelineStep2), Times.Once());
            mediator3.Verify(m => m.Send(It.Is<StepData>(d => d.EncoderState == EncoderState.Hanging &&
                                                                  d.ErrorMessage == message), pipelineStep3), Times.Once());
        }

        private class StepDataStub : StepData
        {

        }

        private class PipelineStepStub : PipelineStep<StepDataStub>
        {
            public PipelineStepStub(IStepMediator mediator, IEncodeWebClient webClient) : base(mediator, webClient)
            {
            }

            public override void Execute(CancellationTokenSourceWrapper tokenSource)
            {
                
            }
        }
    }
}