using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Portal.BackEnd.Encoder.Pipeline.Step;
using Portal.BackEnd.Encoder.Status;
using Portal.Domain.BackendContext.Enum;
using Wrappers;

namespace Portal.BackEnd.Encoder.Test.PipelineTests
{
    [TestClass]
    public class FinishStepTest
    {
        private Mock<IStepMediator> _pipelineMediator;
        private Mock<IEncodeWebClient> _restClient;
        private Mock<ITempFileManager> _tempFileManager;

        [TestInitialize]
        public void Initialize()
        {
            _pipelineMediator = new Mock<IStepMediator>();
            _restClient = new Mock<IEncodeWebClient>();
            _tempFileManager = new Mock<ITempFileManager>();
        }

        [TestMethod]
        public void CreateFinishStepTest()
        {
            //Act
            var pipelineStep = new FinishStep(_pipelineMediator.Object, _restClient.Object, _tempFileManager.Object);

            //Assert
            Assert.IsInstanceOfType(pipelineStep, typeof(PipelineStepBase<UploadStepData>));
            Assert.IsNotInstanceOfType(pipelineStep, typeof(PipelineStep<UploadStepData>));
            _pipelineMediator.Verify(m => m.AddFinishStep(pipelineStep), Times.Once());
        }

        [TestMethod]
        public void ExecuteFinishStepTest()
        {
            //Arrange
            const EncoderState encoderState = EncoderState.Cancelled;
            const string fileHash = "fileHash";
            const string errorMessage = "errorMessage";
           
            var pipelineStep = new FinishStep(_pipelineMediator.Object, _restClient.Object, _tempFileManager.Object);
            var stepData = new UploadStepData()
                {
                    EncoderState = encoderState,
                    FileHash = fileHash,
                    ErrorMessage=errorMessage
                };

            pipelineStep.SetData(stepData);


            //Act
            pipelineStep.Execute(It.IsAny<CancellationTokenSourceWrapper>());

            //Assert
            _tempFileManager.Verify(m => m.DeleteAllTempFiles());
            _restClient.Verify(m => m.FinishTask(encoderState, fileHash, errorMessage), Times.Once());
        }
    }
}