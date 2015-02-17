using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Portal.BackEnd.Encoder.Pipeline.Step;

namespace Portal.BackEnd.Encoder.Test.PipelineTests
{
    [TestClass]
    public class StepMediatorTest
    {
        private StepMediator _mediator;

        private Mock<IPipelineStep> _getTaskStep;
        private Mock<IPipelineStep> _initializingWebClientStep;
        private Mock<IPipelineStep> _gettingEntityStep;
        private Mock<IPipelineStep> _downloadStep;
        private Mock<IPipelineStep> _creatorStep;
        private Mock<IPipelineStep> _encodeStep;
        private Mock<IPipelineStep> _uplodStep;
        private Mock<IPipelineStep> _finishStep;

        [TestInitialize]
        public void Initialize()
        {
            _mediator = new StepMediator();

            _getTaskStep = new Mock<IPipelineStep>();
            _initializingWebClientStep = new Mock<IPipelineStep>();
            _gettingEntityStep = new Mock<IPipelineStep>();
            _downloadStep = new Mock<IPipelineStep>();
            _creatorStep = new Mock<IPipelineStep>();
            _encodeStep = new Mock<IPipelineStep>();
            _uplodStep=new Mock<IPipelineStep>();
            _finishStep = new Mock<IPipelineStep>();
            
            _mediator.AddGetTaskStep(_getTaskStep.Object);
            _mediator.AddInitializingWebClientStep(_initializingWebClientStep.Object);
            _mediator.AddGettingEntityStep(_gettingEntityStep.Object);
            _mediator.AddDownloadStep(_downloadStep.Object);
            _mediator.AddCreatorStep(_creatorStep.Object);
            _mediator.AddEncodeStep(_encodeStep.Object);
            _mediator.AddUploadStep(_uplodStep.Object);
            _mediator.AddFinishStep(_finishStep.Object);

            SetStepDataConstraint<TaskStepData>(_initializingWebClientStep);
            SetStepDataConstraint<TaskStepData>(_gettingEntityStep);
            SetStepDataConstraint<GettingEntityStepData>(_downloadStep);
            SetStepDataConstraint<GettingEntityStepData>(_creatorStep);
            SetStepDataConstraint<CreatorStepData>(_encodeStep);
            SetStepDataConstraint<EncodeStepData>(_uplodStep);
            SetStepDataConstraint<UploadStepData>(_finishStep);
        }

        private void SetStepDataConstraint<T>(Mock<IPipelineStep> pipelineStep) where T : StepData
        {
            pipelineStep.Setup(m => m.SetData(It.Is<StepData>(d => !(d is T)))).Throws<InvalidCastException>();
        }
        
        [TestMethod]
        public void SendDataFromGetTaskStepTest()
        {
            //Arrange
            var stepData = new TaskStepData();

            //Act
            _mediator.Send(stepData, _getTaskStep.Object);

            //Assert
            _initializingWebClientStep.Verify(m => m.SetData(stepData));
            _gettingEntityStep.Verify(m => m.SetData(stepData));
        }

        [TestMethod]
        public void SendDataFromGettingEntityStepTest()
        {
            //Arrange
            var stepData = new GettingEntityStepData();

            //Act
            _mediator.Send(stepData, _gettingEntityStep.Object);

            //Assert
            _downloadStep.Verify(m => m.SetData(stepData));
        }

        [TestMethod]
        public void SendDataFromDownloadStepTest()
        {
            //Arrange
            var stepData = new GettingEntityStepData();

            //Act
            _mediator.Send(stepData, _downloadStep.Object);

            //Assert
            _creatorStep.Verify(m => m.SetData(stepData));
        }

        [TestMethod]
        public void SendDataFromCreatorStepTest()
        {
            //Arrange
            var stepData = new CreatorStepData();

            //Act
            _mediator.Send(stepData, _creatorStep.Object);

            //Assert
            _encodeStep.Verify(m => m.SetData(stepData), Times.Once());
        }

        [TestMethod]
        public void SendDataFromEncodeStepTest()
        {
            //Arrange
            var stepData = new EncodeStepData();

            //Act
            _mediator.Send(stepData, _encodeStep.Object);

            //Assert
            _uplodStep.Verify(m => m.SetData(stepData));
        }

        [TestMethod]
        public void SendDataFromUploadTest()
        {
            //Arrange
            var stepData = new UploadStepData();

            //Act
            _mediator.Send(stepData, _uplodStep.Object);

            //Assert
            _finishStep.Verify(m => m.SetData(stepData));
        }
    }
}