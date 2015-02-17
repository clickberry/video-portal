using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Portal.BackEnd.Encoder.Pipeline.Step;
using Portal.BackEnd.Encoder.Settings;
using Portal.Domain.BackendContext.Entity.Base;
using Portal.Domain.BackendContext.Enum;
using Portal.SLL.Abstract;
using Wrappers;
using Wrappers.Interface;

namespace Portal.BackEnd.Encoder.Test.PipelineTests
{
    [TestClass]
    public class CreatorStepTest
    {
        [TestMethod]
        public void CreateGetTaskStepTest()
        {
            //Arrange
            var pipelineMediator = new Mock<IStepMediator>();
            var webClient = new Mock<IEncodeWebClient>();
            var creatorFactory = new Mock<IEncodeCreatorFactory>();
            var tempFileManager = new Mock<ITempFileManager>();

            //Act
            var pipelineStep = new CreatorStep(pipelineMediator.Object, webClient.Object, creatorFactory.Object, tempFileManager.Object);

            //Assert
            Assert.IsInstanceOfType(pipelineStep, typeof(PipelineStepBase<GettingEntityStepData>));
            Assert.IsInstanceOfType(pipelineStep, typeof(PipelineStep<GettingEntityStepData>));
            pipelineMediator.Verify(m => m.AddCreatorStep(pipelineStep), Times.Once());
        }

        [TestMethod]
        public void ExecuteMethodTest()
        {
            //Arrange
            var pipelineMediator = new Mock<IStepMediator>();
            var webClient = new Mock<IEncodeWebClient>();
            var encodeData = new Mock<IEncodeData>();
            var creatorFactory = new Mock<IEncodeCreatorFactory>();
            var tempFileManager = new Mock<ITempFileManager>();
            var encodeCreator = new Mock<IEncodeCreator>();
            var ffmpegParser = new Mock<IFfmpegParser>();
            var dataReceiveHandler = new Mock<IDataReceivedHandler>();
            var encodeStringFactory = new Mock<IVideoEncodeStringFactory>();
            var encodeStringBuilder = new Mock<IEncodeStringBuilder>();

            var stepData = new GettingEntityStepData()
            {
                EncodeData = encodeData.Object
            };

            var pipelineStep = new CreatorStep(pipelineMediator.Object, webClient.Object, creatorFactory.Object, tempFileManager.Object);

            creatorFactory.Setup(m => m.Create(encodeData.Object)).Returns(encodeCreator.Object);
            encodeCreator.Setup(m => m.CreateFfmpegParser()).Returns(ffmpegParser.Object);
            encodeCreator.Setup(m => m.CreateDataReceivedHandler(ffmpegParser.Object)).Returns(dataReceiveHandler.Object);
            encodeCreator.Setup(m => m.CreateEncodeStringFactory()).Returns(encodeStringFactory.Object);
            encodeCreator.Setup(m => m.CreateEncodeStringBuilder(tempFileManager.Object, encodeStringFactory.Object)).Returns(encodeStringBuilder.Object);
            pipelineStep.SetData(stepData);

            //Act
            pipelineStep.Execute(It.IsAny<CancellationTokenSourceWrapper>());

            //Assert
            pipelineMediator.Verify(m => m.Send(It.Is<CreatorStepData>(d =>
                                                                       d.DataReceivedHandler == dataReceiveHandler.Object &&
                                                                       d.EncodeStringBuilder == encodeStringBuilder.Object &&
                                                                       d.EncoderState == EncoderState.Completed), pipelineStep),
                                    Times.Once());
        }
    }

    internal class FakeEncodeData:IEncodeData
    {
        #region Implementation of IEncodeData

        public string FileId { get; set; }
        public string ContentType { get; set; }

        #endregion
    }
}