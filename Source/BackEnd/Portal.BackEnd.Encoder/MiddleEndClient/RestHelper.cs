// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net;
using Configuration;
using Portal.BackEnd.Encoder.Data;
using Portal.BackEnd.Encoder.Exceptions;
using Portal.BackEnd.Encoder.Interface;
using Portal.Domain.BackendContext.Constant;
using Portal.Domain.BackendContext.Entity;
using Portal.Domain.BackendContext.Entity.Base;
using Portal.Domain.BackendContext.Enum;
using RestSharp;
using Wrappers;

namespace Portal.BackEnd.Encoder.MiddleEndClient
{
    public class RestHelper : IRestHelper
    {
        public const string TaskResource = "/api/tasks";
        private readonly IEncodeDeserializer _deserializer;
        private readonly IRestClient _restClient;
        private readonly IPortalBackendSettings _settings;

        public RestHelper(IRestClient restClient, IEncodeDeserializer deserializer, IPortalBackendSettings settings)
        {
            _restClient = restClient;
            _deserializer = deserializer;
            _settings = settings;
        }

        public TaskData GetTaskData(IRestResponse response)
        {
            EncodeTaskData encodeTaskData = _deserializer.EncodeTaskDataDeserealize(response);
            string resource = GetHeaderValue(response, HeaderParameters.Location);
            string contentType = GetHeaderValue(response, HeaderParameters.ContentType);
            TypeOfTask type = GetTypeOfTask(contentType);
            var taskData = new TaskData
            {
                Id = encodeTaskData.TaskId,
                Resource = resource,
                Type = type
            };

            return taskData;
        }

        public IRestResponse GetResponse(IRestRequest request)
        {
            _restClient.BaseUrl = _settings.BaseUrl;
            IRestResponse response = _restClient.Execute(request);

            if (response.ErrorException is WebException)
            {
                var webException = response.ErrorException as WebException;
                if (webException.Status != WebExceptionStatus.Timeout)
                {
                    throw new ResponseWebException(webException.Message, webException);
                }
                throw new ResponseTimeoutException(webException.Message, webException);
            }
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                throw new NoContentException();
            }
            if (response.StatusCode >= (HttpStatusCode)400)
            {
                throw new StatusCodeException(response.StatusCode);
            }
            return response;
        }

        public IRestRequest TaskRequestCreate()
        {
            string userAgent = String.Format("Ffmpeg/{0} ({1})", _settings.FfmpegVersion, Environment.OSVersion.VersionString);
            IRestRequest request = RequestCreate(TaskResource, Method.POST);
            request.AddHeader(HeaderParameters.UserAgent, userAgent);
            request.AddHeader(HeaderParameters.Accept, _settings.TaskTypes);
            return request;
        }

        public IRestRequest EncodeDataRequestCreate(string resource)
        {
            IRestRequest request = RequestCreate(resource, Method.GET);

            return request;
        }

        public IRestRequest SetStatusRequestCreate(string resource, int progress)
        {
            IRestRequest request = RequestCreate(resource, Method.PUT);
            request.AddParameter(ProcessStatusParameters.Progress, progress);

            return request;
        }

        public IRestRequest FinishTaskRequestCreate(string resource, EncoderState encoderState, string fileId, string errorMessage)
        {
            IRestRequest request = RequestCreate(resource, Method.POST);
            request.AddHeader("X-HTTP-Method-Override", "DELETE");
            request.AddParameter(EncoderStatusParameters.Result, encoderState);
            request.AddParameter(EncoderStatusParameters.FileId, fileId);
            request.AddParameter(EncoderStatusParameters.Message, errorMessage);

            return request;
        }

        public IEncodeData CreateEncodeData(IRestResponse restResponse, TypeOfTask typeOfTask)
        {
            IEncodeData encodeData = _deserializer.EncodeDataDeserialize(restResponse, typeOfTask);

            return encodeData;
        }

        private IRestRequest RequestCreate(string resource, Method method)
        {
            var request = new RestRequestWrapper(resource, method);
            request.AddHeader("Authorization", string.Format("Bearer {0}", _settings.BearerToken));

            return request;
        }

        private string GetHeaderValue(IRestResponse response, string headerName)
        {
            Parameter parameter = response.Headers.FirstOrDefault(p => p.Name == headerName);
            return parameter == null ? null : parameter.Value.ToString().Split(';')[0];
        }

        private TypeOfTask GetTypeOfTask(string contentType)
        {
            switch (contentType)
            {
                case ContentType.TaskVideo:
                    return TypeOfTask.Video;

                case ContentType.TaskScreenshot:
                    return TypeOfTask.Screenshot;
            }

            return TypeOfTask.None;
        }
    }
}