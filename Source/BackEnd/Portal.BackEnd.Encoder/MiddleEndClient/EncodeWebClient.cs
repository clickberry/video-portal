// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Portal.BackEnd.Encoder.Data;
using Portal.BackEnd.Encoder.Exceptions;
using Portal.BackEnd.Encoder.Interface;
using Portal.Domain.BackendContext.Entity.Base;
using Portal.Domain.BackendContext.Enum;
using RestSharp;
using Wrappers;
using Wrappers.Interface;

namespace Portal.BackEnd.Encoder.MiddleEndClient
{
    public class EncodeWebClient : IEncodeWebClient
    {
        private readonly IDateTimeWrapper _dateTimeWrapper;
        private readonly IRestHelper _restHelper;
        private readonly TimeSpan _updateInterval = new TimeSpan(0, 0, 0, 10);
        private DateTime _lastUpdate;

        public EncodeWebClient(IRestHelper restHelper, IDateTimeWrapper dateTimeWrapper)
        {
            _restHelper = restHelper;
            _dateTimeWrapper = dateTimeWrapper;
        }

        public string Resource { get; private set; }

        public void Initialize(string resource, string taskId, CancellationTokenSourceWrapper cancellationTokenSourceWrapper)
        {
            Resource = resource;
        }

        public TaskData GetTask()
        {
            IRestRequest request = _restHelper.TaskRequestCreate();
            IRestResponse response = _restHelper.GetResponse(request);
            TaskData taskData = _restHelper.GetTaskData(response);

            return taskData;
        }

        public IEncodeData GetEntity(TypeOfTask typeOfTask)
        {
            IRestRequest request = _restHelper.EncodeDataRequestCreate(Resource);
            IRestResponse response = _restHelper.GetResponse(request);
            IEncodeData encodeData = _restHelper.CreateEncodeData(response, typeOfTask);

            return encodeData;
        }

        public void SetStatus(int progress)
        {
            if (_updateInterval <= _dateTimeWrapper.CurrentDateTime().Subtract(_lastUpdate) || progress == 100)
            {
                IRestRequest request = _restHelper.SetStatusRequestCreate(Resource, progress);

                try
                {
                    _restHelper.GetResponse(request);
                }
                catch (ResponseTimeoutException)
                {
                }

                _lastUpdate = _dateTimeWrapper.CurrentDateTime();
            }
        }

        public void FinishTask(EncoderState encoderState, string fileId, string errorMessage)
        {
            IRestRequest request = _restHelper.FinishTaskRequestCreate(Resource, encoderState, fileId, errorMessage);
            _restHelper.GetResponse(request);
        }
    }
}