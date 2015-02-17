using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using FfmpegBackend.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FfmpegBackendTest
{
    [TestClass]
    public class StartPipelineStepTest
    {
        //TODO All Steps work through a Mediator
        [TestMethod]
        public void ExecuteMethodTest()
        {
            //Arrange
            var pipelineMediator = new Mock<IPipelineMediator>();
            var restClient = new Mock<IEncodeRestClient>();
            var pipelineStep = new StartPipelineStep(pipelineMediator.Object, restClient.Object);

            restClient.Setup(m => m.GetTask()).Returns(new TaskData()
                                                           {
                                                               TypeOfTask = TypeOfTask.Encode
                                                           });

            //Act
            pipelineStep.Execute();

            //Assert
            pipelineMediator.Verify(m=>m.Send(It.IsAny<EncodeStepData>()), Times.Once());
        }
    }

    public class EncodeStepData:StepDataBase
    {
    }

    public abstract class StepDataBase
    {
        public string TaskId { get; set; }
    }

    public class StartPipelineStep:IPipelineStep
    {
        private IPipelineMediator _pipelineMediator;
        private IEncodeRestClient _restClient;

        public StartPipelineStep(IPipelineMediator pipelineMediator, IEncodeRestClient restClient)
        {
            _pipelineMediator = pipelineMediator;
            _restClient = restClient;
        }

        public void Execute()
        {
            _pipelineMediator.Send(new EncodeStepData());
        }
    }

    public interface IPipelineMediator
    {
        void Send(StepDataBase stepData);
    }
}
