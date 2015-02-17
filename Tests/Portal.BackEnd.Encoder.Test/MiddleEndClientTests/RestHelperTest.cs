using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Exceptions;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.MiddleEndClient;
using Portal.BackEnd.Encoder.Settings;
using Portal.Domain.BackendContext.Constant;
using Portal.Domain.BackendContext.Entity;
using Portal.Domain.BackendContext.Entity.Base;
using Portal.Domain.BackendContext.Enum;
using RestSharp;
using TestExtension;

namespace Portal.BackEnd.Encoder.Test.MiddleEndClientTests
{
    [TestClass]
    public class RestHelperTest
    {
        private Mock<IRestClient> _restClient;
        private Mock<IEncodeDeserializer> _deserializer;
        private RestHelper _helper;
        private RestHelperSettings _settings;

        [TestInitialize]
        public void Initialize()
        {
            _restClient = new Mock<IRestClient>();
            _deserializer = new Mock<IEncodeDeserializer>();
            _settings = new RestHelperSettings("CookieName", "CookieValue", "task types", "1.0.0", "BaseUrl");
            _helper = new RestHelper(_restClient.Object, _deserializer.Object, _settings);
        }

        [TestMethod]
        public void TaskRequestCreateTest()
        {
            //Arrange
            var userAgent = String.Format("Ffmpeg/{0} ({1})", _settings.FfmpegVersion, Environment.OSVersion.VersionString);

            //Act
            var request = _helper.TaskRequestCreate();

            //Assert
            Assert.AreEqual(Method.POST, request.Method);
            Assert.AreEqual(RestHelper.TaskResource, request.Resource);
            Assert.IsTrue(request.Parameters.Any(p => p.Type == ParameterType.Cookie && p.Name == _settings.CookieName && (string)p.Value == _settings.CookieValue));
            Assert.IsTrue(request.Parameters.Any(p => p.Type == ParameterType.HttpHeader && p.Name == HeaderParameters.UserAgent && (string) p.Value == userAgent));
            Assert.IsTrue(request.Parameters.Any(p => p.Type == ParameterType.HttpHeader && p.Name == HeaderParameters.Accept && (string)p.Value == _settings.TaskTypes));
        }

        [TestMethod]
        public void EncodeDataRequestCreateTest()
        {
            //Arrange
            const string resource = "resource";

            //Act
            var request = _helper.EncodeDataRequestCreate(resource);

            //Assert
            Assert.AreEqual(Method.GET, request.Method);
            Assert.AreEqual(resource, request.Resource);
            Assert.IsTrue(request.Parameters.Any(p => p.Type == ParameterType.Cookie && p.Name == _settings.CookieName && (string)p.Value == _settings.CookieValue));
        }

        [TestMethod]
        public void SetStatusRequestCreateTest()
        {
            //Arrange
            const string resource = "resource";
            const int progress = 34;

            //Act
            var request = _helper.SetStatusRequestCreate(resource, progress);

            //Assert
            Assert.AreEqual(Method.PUT, request.Method);
            Assert.AreEqual(resource, request.Resource);
            Assert.IsTrue(request.Parameters.Any(p => p.Type == ParameterType.Cookie && p.Name == _settings.CookieName && (string)p.Value == _settings.CookieValue));
            Assert.IsTrue(request.Parameters.Any(p => p.Name == ProcessStatusParameters.Progress && (int)p.Value == progress));
        }

        [TestMethod]
        public void FinishTaskRequestCreateTest()
        {
            //Arrange
            const string resource = "resource";
            const string errorMessage = "errorMessage";
            const string fileHash = "fileId";
            const EncoderState encoderState=EncoderState.Completed;
            

            //Act
            var request = _helper.FinishTaskRequestCreate(resource, encoderState, fileHash, errorMessage);

            //Assert
            Assert.AreEqual(Method.POST, request.Method);
            Assert.AreEqual(resource, request.Resource);
            Assert.IsTrue(request.Parameters.Any(p => p.Type == ParameterType.Cookie && p.Name == _settings.CookieName && (string)p.Value == _settings.CookieValue));
            Assert.IsTrue(request.Parameters.Any(p => p.Name == EncoderStatusParameters.Result && (EncoderState)p.Value == encoderState));
            Assert.IsTrue(request.Parameters.Any(p => p.Name == EncoderStatusParameters.Message && (string)p.Value == errorMessage));
            Assert.IsTrue(request.Parameters.Any(p => p.Name == EncoderStatusParameters.FileHash && (string)p.Value == fileHash));
            Assert.IsTrue(request.Parameters.Any(p => p.Name == "X-HTTP-Method-Override" && (string)p.Value == "DELETE"));
        }
        
        [TestMethod]
        public void GetResponseTest()
        {
            //Arrange
            var request = new Mock<IRestRequest>();
            var mockResponse = new Mock<IRestResponse>();

            mockResponse.Setup(p => p.StatusCode).Returns(HttpStatusCode.OK);
            _restClient.Setup(m => m.Execute(It.IsAny<IRestRequest>())).Returns(mockResponse.Object);

            //Act
            var response = _helper.GetResponse(request.Object);
            
            //Assert
            Assert.AreEqual(mockResponse.Object, response);
            _restClient.Verify(m => m.Execute(request.Object), Times.Once());
            _restClient.VerifySet((p)=>p.BaseUrl=_settings.BaseUrl);
        }

        [TestMethod]
        public void GetTaskTest()
        {
            //Arrange
            const string resource = "resource";
            const string taskId = "taskId";

            var response = new Mock<IRestResponse>();
            var headers = new List<Parameter>()
                              {
                                  new Parameter()
                                      {
                                          Name = HeaderParameters.ContentType,
                                          Value = String.Format("{0};bla-bla", ContentType.TaskVideo)
                                      },
                                  new Parameter()
                                      {
                                          Name = HeaderParameters.Location,
                                          Value = resource
                                      }
                              };
            var encodeTaskData=new EncodeTaskData()
                                   {
                                       TaskId=taskId
                                   };

            response.Setup(p => p.Headers).Returns(headers);
            _deserializer.Setup(m => m.EncodeTaskDataDeserealize(response.Object)).Returns(encodeTaskData);

            //Act
            var taskData = _helper.GetTaskData(response.Object);

            //Assert
            Assert.AreEqual(TypeOfTask.Video, taskData.Type);
            Assert.AreEqual(resource,taskData.Resource);
            Assert.AreEqual(taskId,taskData.Id);
        }

        [TestMethod]
        public void GetResponseThrowNoContentExceptionTest()
        {
            //Arrange
            var noContentResponse = new Mock<IRestResponse>();
            
            noContentResponse.Setup(p => p.StatusCode).Returns(HttpStatusCode.NoContent);

            _restClient.SetupSequence(m => m.Execute(It.IsAny<IRestRequest>()))
                       .Returns(noContentResponse.Object);

            //Act & Assert
            ExceptionAssert.Throws<NoContentException>(() => _helper.GetResponse(It.IsAny<IRestRequest>()));
        }

        [TestMethod]
        public void GetResponseThrowStatusCodeExceptionTest()
        {
            //Arrange
            var badRequestResponse = new Mock<IRestResponse>();
            var gatewayTimeoutResponse = new Mock<IRestResponse>();

            badRequestResponse.Setup(m => m.StatusCode).Returns(HttpStatusCode.BadRequest);
            gatewayTimeoutResponse.Setup(m => m.StatusCode).Returns(HttpStatusCode.GatewayTimeout);

            _restClient.SetupSequence(m => m.Execute(It.IsAny<IRestRequest>()))
                .Returns(badRequestResponse.Object)
                .Returns(gatewayTimeoutResponse.Object);

            //Act & Assert
            var exception1 = ExceptionAssert.Throws<StatusCodeException>(() => _helper.GetResponse(It.IsAny<IRestRequest>()));
            var exception2 = ExceptionAssert.Throws<StatusCodeException>(() => _helper.GetResponse(It.IsAny<IRestRequest>()));

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, exception1.HttpStatusCode);
            Assert.AreEqual(HttpStatusCode.GatewayTimeout, exception2.HttpStatusCode);
        }

        [TestMethod]
        public void GetResponseThrowWebExceptionTest()
        {
            //Arrange
            const string errorMessage = "errorMessage";

            var webExcepotion = new WebException(errorMessage);
            var mockResponse = new Mock<IRestResponse>();

            mockResponse.Setup(m => m.ErrorException).Returns(webExcepotion);
            _restClient.Setup(m => m.Execute(It.IsAny<IRestRequest>())).Returns(mockResponse.Object);


            //Act &Assert
            var exception = ExceptionAssert.Throws<ResponseWebException>(() => _helper.GetResponse(It.IsAny<IRestRequest>()));

            //Assert
            Assert.AreEqual(errorMessage,exception.Message);
            Assert.AreEqual(webExcepotion, exception.InnerException);
        }

        [TestMethod]
        public void GetResponseThrowResponseTimeoutExceptionTest()
        {
            //Arrange
            const string errorMessage = "errorMessage";

            var webExcepotion = new WebException(errorMessage,WebExceptionStatus.Timeout);
            var mockResponse = new Mock<IRestResponse>();

            mockResponse.Setup(m => m.ErrorException).Returns(webExcepotion);
            _restClient.Setup(m => m.Execute(It.IsAny<IRestRequest>())).Returns(mockResponse.Object);


            //Act &Assert
            var exception = ExceptionAssert.Throws<ResponseTimeoutException>(() => _helper.GetResponse(It.IsAny<IRestRequest>()));

            //Assert
            Assert.AreEqual(errorMessage, exception.Message);
            Assert.AreEqual(webExcepotion, exception.InnerException);
        }

        [TestMethod]
        public void CreateEncodeDataTest()
        {
            //Arrange
            const TypeOfTask typeOfTask=TypeOfTask.Video;

            var data = new Mock<IEncodeData>();
            var mockResponse = new Mock<IRestResponse>();

            _deserializer.Setup(m => m.EncodeDataDeserialize(mockResponse.Object, typeOfTask)).Returns(data.Object);

            //Act
            var encodeData = _helper.CreateEncodeData(mockResponse.Object, typeOfTask);

            //Assert
            Assert.AreEqual(data.Object, encodeData);
        }
    }
}
