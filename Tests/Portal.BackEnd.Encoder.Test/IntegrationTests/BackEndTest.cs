using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.MiddleEndClient;
using Portal.BackEnd.Encoder.Test.IntegrationTests.Infrastructure;
using Portal.DAL.FileSystem;
using Portal.Domain.BackendContext.Constant;
using Portal.Domain.BackendContext.Entity;
using Portal.Domain.BackendContext.Enum;
using RestSharp;
using RestSharp.Serializers;
using SimpleInjector;

namespace Portal.BackEnd.Encoder.Test.IntegrationTests
{
    [TestClass]
    public class BackEndTest
    {
        const string TaskId = "taskId";
        const string Resource = "resource";

        private EncodeManager _manager;
        private Mock<IRestClient> _restClient;
        private CancellationTokenSource _token;
        
        private Mock<IRestResponse> _getTaskResponse;
        private Mock<IRestResponse> _getEntityResponse;
        private Mock<IRestResponse> _setStatusResponse;
        private Mock<IRestResponse> _deleteResponse;

        private IRestRequest _getTaskRequest;
        private IRestRequest _getEntityRequest;
        private IRestRequest _setStatusRequest;
        private IRestRequest _deleteRequest;

        private Action<string> _action;

        [TestInitialize]
        public void Initializer()
        {
            _getTaskResponse = new Mock<IRestResponse>();
            _getEntityResponse = new Mock<IRestResponse>();
            _setStatusResponse = new Mock<IRestResponse>();
            _deleteResponse = new Mock<IRestResponse>();
            _restClient = new Mock<IRestClient>();

           _action = null;

            var container = new Container(new ContainerOptions()
            {
                AllowOverridingRegistrations = true
            });
            var initiolizer = new IoCInitializer(_restClient.Object);
            initiolizer.Initialize(container);
            
            container.Register<IFileSystem>(() => new FakeFileSystem());

            _manager = container.GetInstance<EncodeManager>();
            _token = container.GetInstance<CancellationTokenSource>();
        }

        [TestMethod]
        public void EncodeMp4TaskTest()
        {
            //Arrange
            var taskData = new EncodeTaskData()
                               {
                                   TaskId = TaskId
                               };
            var encodeData = new VideoEncodeData()
                                 {
                                     FileId = ConfigurationManager.AppSettings.Get("TestVideo"),
                                     ContentType = "video/mp4",
                                     AudioParam = new AudioParam()
                                                      {
                                                          AudioCodec = MetadataConstant.AacCodec,
                                                          AudioBitrate = 64000
                                                      },
                                     VideoParam = new VideoParam()
                                                      {
                                                          VideoWidth = 480,
                                                          VideoHeight = 360,
                                                          
                                                          VideoCodec = MetadataConstant.AvcCodec,
                                                          VideoBitrate = 500000,
                                                          VideoProfile = MetadataConstant.AvcMainProfile,
                                                          MediaContainer = MetadataConstant.Mp4Container,
                                                          FrameRate = 25,
                                                          KeyFrameRate = 10
                                                      }
                                 };

            var serializer = new JsonSerializer();
            var jsonEncodeData = serializer.Serialize(encodeData);
            var jsonTaskData = serializer.Serialize(taskData);
            var getTaskHeaders = VideoGetTaskResponseHeaders(Resource);

            ResponseSetup(jsonTaskData, jsonEncodeData, getTaskHeaders);
            RestClientSetup(Resource);
            
            //Act
            var task = _manager.Start();
            task.Wait();

            Trace.WriteLine(_deleteRequest.Parameters.FirstOrDefault(p => p.Name == EncoderStatusParameters.Result).Value);

            //Assert
            Assert.IsTrue(_deleteRequest.Parameters.Any(p => p.Name == EncoderStatusParameters.Result && (EncoderState)p.Value == EncoderState.Completed));
        }

        [TestMethod]
        public void EncodeWebmTaskTest()
        {
            //Arrange
            var taskData = new EncodeTaskData()
                               {
                                   TaskId = TaskId
                               };
            var encodeData = new VideoEncodeData()
                                 {
                                     FileId = ConfigurationManager.AppSettings.Get("TestVideo"),
                                     ContentType = "video/webm",
                                     AudioParam = new AudioParam()
                                                      {
                                                          AudioCodec = MetadataConstant.VorbisCodec,
                                                          AudioBitrate = 64000,
                                                      },
                                     VideoParam = new VideoParam()
                                                      {
                                                          VideoWidth = 480,
                                                          VideoHeight = 360,

                                                          VideoCodec = MetadataConstant.Vp8Codec,
                                                          VideoBitrate = 500000,
                                                          MediaContainer = MetadataConstant.WebmContainer,
                                                          FrameRate = 25,
                                                          KeyFrameRate = 10
                                                      }
                                 };

            var serializer = new JsonSerializer();
            var jsonEncodeData = serializer.Serialize(encodeData);
            var jsonTaskData = serializer.Serialize(taskData);
            var getTaskHeaders = VideoGetTaskResponseHeaders(Resource);

            ResponseSetup(jsonTaskData, jsonEncodeData, getTaskHeaders);
            RestClientSetup(Resource);

            //Act
            var task = _manager.Start();
            task.Wait();

            Trace.WriteLine(_deleteRequest.Parameters.FirstOrDefault(p => p.Name == EncoderStatusParameters.Result).Value);

            //Assert
            Assert.IsTrue(_deleteRequest.Parameters.Any(p => p.Name == EncoderStatusParameters.Result && (EncoderState)p.Value == EncoderState.Completed));
        }

        [TestMethod]
        public void EncodeScreenshostTaskTest()
        {
            //Arrange
            var taskData = new EncodeTaskData()
            {
                TaskId = TaskId
            };
            var encodeData = new ScreenshotEncodeData()
            {
                FileId = ConfigurationManager.AppSettings.Get("TestVideo"),
                ContentType = "image/jpeg",
                ScreenshotParam = new ScreenshotParam()
                                      {
                                          TimeOffset=1
                                      }
            };

            var serializer = new JsonSerializer();
            var jsonEncodeData = serializer.Serialize(encodeData);
            var jsonTaskData = serializer.Serialize(taskData);
            var getTaskHeaders = ScreenshotGetTaskResponseHeaders(Resource);

            ResponseSetup(jsonTaskData, jsonEncodeData, getTaskHeaders);
            RestClientSetup(Resource);

            //Act
            var task = _manager.Start();
            task.Wait();

            Trace.WriteLine(_deleteRequest.Parameters.FirstOrDefault(p => p.Name == EncoderStatusParameters.Result).Value);

            //Assert
            Assert.IsTrue(_deleteRequest.Parameters.Any(p => p.Name == EncoderStatusParameters.Result && (EncoderState)p.Value == EncoderState.Completed));
        }

        [TestMethod]
        public void FfmpegErrorTest()
        {
            //Arrange
            var taskData = new EncodeTaskData()
            {
                TaskId = TaskId
            };
            var errorEncodeData = new VideoEncodeData()
                {
                    FileId = ConfigurationManager.AppSettings.Get("TestVideo"),
                    VideoParam = new VideoParam()
                        {
                            MediaContainer = MetadataConstant.Mp4Container
                        },
                    AudioParam = new AudioParam()
                        {
                            AudioCodec = MetadataConstant.AacCodec
                        }
                };

            var serializer = new JsonSerializer();
            var jsonEncodeData = serializer.Serialize(errorEncodeData);
            var jsonTaskData = serializer.Serialize(taskData);
            var getTaskHeaders = VideoGetTaskResponseHeaders(Resource);

            ResponseSetup(jsonTaskData, jsonEncodeData, getTaskHeaders);
            RestClientSetup(Resource);

            _restClient.Setup(m => m.Execute(_deleteRequest)).Callback(() => _token.Cancel());

            //Act
            var task = _manager.Start();
            task.Wait();

            Trace.WriteLine(_deleteRequest.Parameters.FirstOrDefault(p => p.Name == EncoderStatusParameters.Result).Value);

            //Assert
            Assert.IsTrue(_deleteRequest.Parameters.Any(p => p.Name == EncoderStatusParameters.Result && (EncoderState)p.Value == EncoderState.Failed));
        }

        /// <summary>
        /// For Succesful test need special video file.
        /// </summary>
        [TestMethod]
        public void FfmpegHangingTest()
        {
            //Arrange
            var taskData = new EncodeTaskData()
            {
                TaskId = TaskId
            };
            var encodeData = new VideoEncodeData()
                                 {
                                     FileId = ConfigurationManager.AppSettings.Get("HangingVideo"),
                                     ContentType = "video/webm",
                                     AudioParam = new AudioParam()
                                                      {
                                                          AudioCodec = MetadataConstant.VorbisCodec,
                                                          AudioBitrate = 128000,
                                                      },
                                     VideoParam = new VideoParam()
                                                      {
                                                          VideoWidth = 480,
                                                          VideoHeight = 360,

                                                          VideoCodec = MetadataConstant.Vp8Codec,
                                                          VideoBitrate = 500000,
                                                          MediaContainer = MetadataConstant.WebmContainer,
                                                          FrameRate = 25,
                                                          KeyFrameRate = 10
                                                      }
                                 };

            var serializer = new JsonSerializer();
            var jsonEncodeData = serializer.Serialize(encodeData);
            var jsonTaskData = serializer.Serialize(taskData);
            var getTaskHeaders = VideoGetTaskResponseHeaders(Resource);

            ResponseSetup(jsonTaskData, jsonEncodeData, getTaskHeaders);
            RestClientSetup(Resource);

            //Act
            var task = _manager.Start();
            task.Wait();

            Trace.WriteLine(_deleteRequest.Parameters.FirstOrDefault(p => p.Name == EncoderStatusParameters.Result).Value);

            //Assert
            //Assert.IsTrue(_setStatusRequest.Parameters.Any(p=>p.Name==ProcessStatusParameters.State && (ProcessState)p.Value==ProcessState.Hanging));
            Assert.IsTrue(_deleteRequest.Parameters.Any(p => p.Name == EncoderStatusParameters.Result && (EncoderState)p.Value == EncoderState.Cancelled));
        }

        private void RestClientSetup(string resource)
        {
            _restClient.Setup(m => m.Execute(It.Is<IRestRequest>(p => p.Resource == RestHelper.TaskResource && p.Method == Method.POST)))
                .Callback<IRestRequest>(request => _getTaskRequest = request)
                .Returns(_getTaskResponse.Object);

            _restClient.Setup(m => m.Execute(It.Is<IRestRequest>(p => p.Resource == resource && p.Method == Method.GET)))
                .Callback<IRestRequest>(request => _getEntityRequest = request)
                .Returns(_getEntityResponse.Object);

            _restClient.Setup(m => m.Execute(It.Is<IRestRequest>(p => p.Resource == resource
                                                                      && p.Method == Method.PUT)))
                .Callback<IRestRequest>(request => _setStatusRequest = request)
                .Returns(_setStatusResponse.Object);

            //_restClient.Setup(m => m.Execute(It.Is<IRestRequest>(p => p.Resource == Resource
            //                                                          && p.Method == Method.PUT
            //                                                          && p.Parameters.Any(param => param.Name == ProcessStatusParameters.State && (ProcessState)param.Value == ProcessState.Hanging))))
            //    .Callback<IRestRequest>(request =>
            //    {
            //        _setStatusRequest = request;
            //        //_action(TaskId);
            //    })
            //    .Returns(_setStatusResponse.Object);

            _restClient.Setup(m => m.Execute(It.Is<IRestRequest>(p => p.Resource == resource && p.Method == Method.POST)))
                .Callback<IRestRequest>(request => _deleteRequest = request)
                .Returns(_deleteResponse.Object)
                .Callback(_token.Cancel);
        }

        private void ResponseSetup(string jsonTaskData, string jsonEncodeData, List<Parameter> getTaskHeaders)
        {
            _getTaskResponse.Setup(p => p.Content).Returns(jsonTaskData);
            _getTaskResponse.Setup(p => p.StatusCode).Returns(HttpStatusCode.Redirect);
            _getTaskResponse.Setup(p => p.Headers).Returns(getTaskHeaders);

            _getEntityResponse.Setup(p => p.Content).Returns(jsonEncodeData);
            _getEntityResponse.Setup(p => p.StatusCode).Returns(HttpStatusCode.OK);

            _setStatusResponse.Setup(p => p.StatusCode).Returns(HttpStatusCode.OK);

            _deleteResponse.Setup(p => p.StatusCode).Returns(HttpStatusCode.OK);
        }

        private List<Parameter> VideoGetTaskResponseHeaders(string resource)
        {
            var getTaskHeaders = new List<Parameter>
                                     {
                                         new Parameter()
                                             {
                                                 Name = HeaderParameters.ContentType,
                                                 Value = ContentType.TaskVideo,
                                                 Type = ParameterType.HttpHeader
                                             },
                                         new Parameter()
                                             {
                                                 Name = HeaderParameters.Location,
                                                 Value = resource,
                                                 Type = ParameterType.HttpHeader
                                             }
                                     };
            return getTaskHeaders;
        }

        private List<Parameter> ScreenshotGetTaskResponseHeaders(string resource)
        {
            var getTaskHeaders = new List<Parameter>
                                     {
                                         new Parameter()
                                             {
                                                 Name = HeaderParameters.ContentType,
                                                 Value = ContentType.TaskScreenshot,
                                                 Type = ParameterType.HttpHeader
                                             },
                                         new Parameter()
                                             {
                                                 Name = HeaderParameters.Location,
                                                 Value = resource,
                                                 Type = ParameterType.HttpHeader
                                             }
                                     };
            return getTaskHeaders;
        }
    }
}
