using System;
using System.Threading.Tasks;
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
    public class EncodeStepTest
    {
        private const string ContentType = "contentType";
        private const string Arguments = "arguments";

        private Mock<IStepMediator> _pipelineMediator;
        private Mock<IEncodeWebClient> _webClient;
        private Mock<IFfmpeg> _ffmpegProcess;
        private Mock<IWatchDogTimer> _watchDogTimer;
        private Mock<IDataReceivedHandler> _dataReceiveHandler;
        private Mock<IEncodeStringBuilder> _encodeStringBuilder;
        private EncodeStep _pipelineStep;
        private EncoderStatus _encoderStatus;
        private CreatorStepData _stepData;
        private Mock<CancellationTokenSourceWrapper> _tokenSource;
        
        [TestInitialize]
        public void Initialize()
        {
            _pipelineMediator = new Mock<IStepMediator>();
            _webClient = new Mock<IEncodeWebClient>();
            _ffmpegProcess = new Mock<IFfmpeg>();
            _watchDogTimer = new Mock<IWatchDogTimer>();
            _dataReceiveHandler = new Mock<IDataReceivedHandler>();
            _encodeStringBuilder = new Mock<IEncodeStringBuilder>();
            _tokenSource = new Mock<CancellationTokenSourceWrapper>();

            _pipelineStep = new EncodeStep(_pipelineMediator.Object, _webClient.Object, _ffmpegProcess.Object, _watchDogTimer.Object);

            _encoderStatus = new EncoderStatus()
                {
                    EncoderState = EncoderState.Failed,
                    ErrorMessage = "errorMessage"
                };
            _stepData = new CreatorStepData()
                {
                    EncoderState = EncoderState.Completed,
                    EncodeStringBuilder = _encodeStringBuilder.Object,
                    DataReceivedHandler = _dataReceiveHandler.Object
                };

            _encodeStringBuilder.Setup(m => m.GetContentType()).Returns(ContentType);
            _encodeStringBuilder.Setup(m => m.GetFfmpegArguments()).Returns(Arguments);
            _ffmpegProcess.Setup(m => m.Start(Arguments, _tokenSource.Object, _dataReceiveHandler.Object.ProcessData)).Returns(() =>
                {
                    var tcs = new TaskCompletionSource<EncoderStatus>();
                    tcs.SetResult(_encoderStatus);
                    return tcs.Task;
                });

            _pipelineStep.SetData(_stepData);
        }

        [TestMethod]
        public void CreateEncodeStepTest()
        {
            //Arrange
            var pipelineMediator = new Mock<IStepMediator>();
            var webClient = new Mock<IEncodeWebClient>();
            var ffmpegProcess = new Mock<IFfmpeg>();
            var watchDogTimer = new Mock<IWatchDogTimer>();

            //Act
            var pipelineStep = new EncodeStep(pipelineMediator.Object, webClient.Object, ffmpegProcess.Object, watchDogTimer.Object);

            //Assert
            Assert.IsInstanceOfType(pipelineStep, typeof(PipelineStepBase<CreatorStepData>));
            Assert.IsInstanceOfType(pipelineStep, typeof(PipelineStep<CreatorStepData>));
            pipelineMediator.Verify(m => m.AddEncodeStep(pipelineStep), Times.Once());
        }

        [TestMethod]
        public void ExecuteTest()
        {
            //Arrange
            _watchDogTimer.Setup(p => p.IsOverflowing).Returns(false);

            //Act
            _pipelineStep.Execute(_tokenSource.Object);

            //Assert
            _dataReceiveHandler.Verify(m => m.Register(_watchDogTimer.Object.Reset), Times.Once());
            _dataReceiveHandler.Verify(m => m.Register(_webClient.Object.SetStatus), Times.Once());
            _watchDogTimer.Verify(m => m.Start(_tokenSource.Object), Times.Once());
            _watchDogTimer.Verify(m => m.Stop(), Times.Once());
            _pipelineMediator.Verify(m => m.Send(It.Is<EncodeStepData>(d => d.EncoderState == _encoderStatus.EncoderState &&
                                                                            d.ErrorMessage == _encoderStatus.ErrorMessage &&
                                                                            d.ContentType == ContentType), _pipelineStep), Times.Once());
            _webClient.Verify(m=>m.SetStatus(100), Times.Once());
        }

        [TestMethod]
        public void ExecuteWhenWatchDogTimerOverflowingTest()
        {
            //Arrange
            _watchDogTimer.Setup(p => p.IsOverflowing).Returns(true);

            //Act
            _pipelineStep.Execute(_tokenSource.Object);

            //Assert
            _pipelineMediator.Verify(m => m.Send(It.Is<EncodeStepData>(d => d.EncoderState == EncoderState.Hanging &&
                                                                            d.ErrorMessage == "Ffmpeg is Hanging"), _pipelineStep), Times.Once());
            _webClient.Verify(m => m.SetStatus(100), Times.Never());
        }
    }
}