using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Data;
using Portal.BackEnd.Encoder.Exceptions;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.MiddleEndClient;
using Portal.Domain.BackendContext.Entity.Base;
using Portal.Domain.BackendContext.Enum;
using RestSharp;
using TestExtension;
using Wrappers;
using Wrappers.Interface;

namespace Portal.BackEnd.Encoder.Test.MiddleEndClientTests
{
    [TestClass]
    public class EncodeWebClientTest
    {
        private Mock<IRestHelper> _restHelper;
        private EncodeWebClient _encodeWebClient;
        private Mock<IRestRequest> _request;
        private Mock<IRestResponse> _response;
        private Mock<CancellationTokenSourceWrapper> _tokenSource;
        private Mock<IDateTimeWrapper> _dateTimeWrapper;

        private const string TaskId="taskId";
        private const string Resource = "resouece";

        [TestInitialize]
        public void Initialize()
        {
            _request = new Mock<IRestRequest>();
            _response = new Mock<IRestResponse>();
            _restHelper = new Mock<IRestHelper>();
            _tokenSource=new Mock<CancellationTokenSourceWrapper>();
            _dateTimeWrapper = new Mock<IDateTimeWrapper>();
            
            _encodeWebClient = new EncodeWebClient(_restHelper.Object, _dateTimeWrapper.Object);
            _encodeWebClient.Initialize(Resource, TaskId, _tokenSource.Object);

            _dateTimeWrapper.SetupSequence(m => m.CurrentDateTime()).Returns(new DateTime(2013, 2, 25));

            _restHelper.Setup(m => m.GetResponse(It.IsAny<IRestRequest>())).Returns(_response.Object);
        }

        [TestMethod]
        public void InitializeTest()
        {
            //Arrange
            var restHelper = new Mock<IRestHelper>();
            var dateTimeWrapper = new Mock<IDateTimeWrapper>();
            var encodeWebClient = new EncodeWebClient(restHelper.Object, dateTimeWrapper.Object);

            //Act
            encodeWebClient.Initialize(Resource, TaskId, _tokenSource.Object);

            //Assert
            Assert.AreEqual(Resource,_encodeWebClient.Resource);
        }
        
        [TestMethod]
        public void GetTaskTest()
        {
            //Arrange
            var data = new TaskData();

            _restHelper.Setup(m => m.TaskRequestCreate()).Returns(_request.Object);
            _restHelper.Setup(m => m.GetTaskData(It.IsAny<IRestResponse>())).Returns(data);

            //Act
            var taskData = _encodeWebClient.GetTask();

            //Assert
            Assert.AreEqual(data,taskData);

            _restHelper.Verify(m=>m.GetResponse(_request.Object), Times.Once());
            _restHelper.Verify(m=>m.GetTaskData(_response.Object),Times.Once());
        }

        [TestMethod]
        public void GetEntityTest()
        {
            //Arrange
            const TypeOfTask typeOfTask=TypeOfTask.Video;

            var data = new Mock<IEncodeData>();
            
            _restHelper.Setup(m => m.EncodeDataRequestCreate(Resource)).Returns(_request.Object);
            _restHelper.Setup(m => m.CreateEncodeData(_response.Object, typeOfTask)).Returns(data.Object);
 
            //Act
            var encodeData = _encodeWebClient.GetEntity(typeOfTask);

            //Assert
            Assert.AreEqual(data.Object,encodeData);
        }

        [TestMethod]
        public void SetStatusTest()
        {
            //Arrange
            const int progress = 12;

            _restHelper.Setup(m => m.SetStatusRequestCreate(Resource, progress)).Returns(_request.Object);

            //Act
            _encodeWebClient.SetStatus(progress);

            //Assert
            _restHelper.Verify(m => m.SetStatusRequestCreate(Resource, progress), Times.Once());
            _restHelper.Verify(m => m.GetResponse(_request.Object), Times.Once());
        }

        [TestMethod]
        public void FrequentlySetStatusTest()
        {
            //Arrange
            const int progress1 = 12;
            const int progress2 = 22;
            const int progress3 = 33;
            var dateTime = new DateTime(2013, 02, 25, 12, 0, 0);

            var count = 1;

            _dateTimeWrapper.Setup(m => m.CurrentDateTime()).Returns(()=>dateTime.AddSeconds(5*count++));
            _restHelper.Setup(m => m.SetStatusRequestCreate(It.IsAny<string>(), It.IsAny<int>())).Returns(_request.Object);

            //Act
            _encodeWebClient.SetStatus(progress1);
            _encodeWebClient.SetStatus(progress2);
            _encodeWebClient.SetStatus(progress3);

            //Assert
            _restHelper.Verify(m => m.SetStatusRequestCreate(Resource, progress1), Times.Once());
            _restHelper.Verify(m => m.SetStatusRequestCreate(Resource, progress2), Times.Never());
            _restHelper.Verify(m => m.SetStatusRequestCreate(Resource, progress3), Times.Once());
            _restHelper.Verify(m => m.GetResponse(_request.Object), Times.Exactly(2));
        }

        [TestMethod]
        public void SetStatusProgress100PercentTest()
        {
            //Arrange
            const int progress1 = 12;
            const int progress2 = 100;
            var dateTime = new DateTime(2013, 02, 25, 12, 0, 0);

            var count = 1;

            _dateTimeWrapper.Setup(m => m.CurrentDateTime()).Returns(() => dateTime.AddSeconds(5 * count++));
            _restHelper.Setup(m => m.SetStatusRequestCreate(It.IsAny<string>(), It.IsAny<int>())).Returns(_request.Object);

            //Act
            _encodeWebClient.SetStatus(progress1);
            _encodeWebClient.SetStatus(progress2);

            //Assert
            _restHelper.Verify(m => m.SetStatusRequestCreate(Resource, progress1), Times.Once());
            _restHelper.Verify(m => m.SetStatusRequestCreate(Resource, progress2), Times.Once());
            _restHelper.Verify(m => m.GetResponse(_request.Object), Times.Exactly(2));
        }

        [TestMethod]
        public void SetStatusNotThrownResponseTimeoutEsception()
        {
            //Arrange
            _restHelper.Setup(m => m.SetStatusRequestCreate(It.IsAny<string>(), It.IsAny<int>())).Returns(_request.Object);
            _restHelper.Setup(m => m.GetResponse(_request.Object)).Throws(new ResponseTimeoutException("", null));

            //Act
            ExceptionAssert.NotThrows<ResponseTimeoutException>(()=>_encodeWebClient.SetStatus(23)); 
        }

        [TestMethod]
        public void FinishTaskTest()
        {
            //Arrange
            const EncoderState encoderState=EncoderState.Failed;
            const string fileHash = "fileHash";
            const string errorMessage = "errorMessage";

            _restHelper.Setup(m => m.FinishTaskRequestCreate(Resource, encoderState, fileHash, errorMessage)).Returns(_request.Object);

            //Act
            _encodeWebClient.FinishTask(encoderState, fileHash, errorMessage);

            //Assert
            _restHelper.Verify(m => m.GetResponse(_request.Object), Times.Once());
        }
    }
}
