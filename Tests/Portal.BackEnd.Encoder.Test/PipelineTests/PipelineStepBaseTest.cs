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
    public class PipelineStepBaseTest
    {
        [TestMethod]
        public void CanExecutTest()
        {
            //Arrange
            var mediator = new Mock<IStepMediator>();
            var webClient = new Mock<IEncodeWebClient>();

            var pipelineStep = new PipelineStepStub(mediator.Object, webClient.Object);
            
            //Act
            var canExecute = pipelineStep.CanExecute();

            //Assert
            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void SetDataTest()
        {
            //Arrange
            var mediator = new Mock<IStepMediator>();
            var webClient = new Mock<IEncodeWebClient>();

            var pipelineStep = new PipelineStepStub(mediator.Object, webClient.Object);
            var stepData = new StepDataStub();

            //Act
            pipelineStep.SetData(stepData);

            //Assert
            Assert.AreEqual(stepData, pipelineStep.ProtectedStepData);
        }

        [TestMethod]
        public void SetDataWhithStepDataTest()
        {
            //Arrange
            const EncoderState encoderState = EncoderState.Cancelled;
            const string message = "message";

            var mediator = new Mock<IStepMediator>();
            var webClient = new Mock<IEncodeWebClient>();

            var pipelineStep = new PipelineStepStub(mediator.Object, webClient.Object);
            var stepData = new StepData()
                {
                    EncoderState = encoderState,
                    ErrorMessage = message
                };

            //Act
            pipelineStep.SetData(stepData);

            //Assert
            Assert.AreEqual(encoderState, pipelineStep.ProtectedStepData.EncoderState);
            Assert.AreEqual(message, pipelineStep.ProtectedStepData.ErrorMessage);
        }

        private class StepDataStub:StepData
        {

        }

        private class PipelineStepStub : PipelineStepBase<StepDataStub>
        {
            public StepDataStub ProtectedStepData
            {
                get { return StepData; }
            }

            public PipelineStepStub(IStepMediator mediator, IEncodeWebClient webClient)
                : base(mediator, webClient)
            {
            }

            public override void Execute(CancellationTokenSourceWrapper tokenSource)
            {
            }
        }
    }
}