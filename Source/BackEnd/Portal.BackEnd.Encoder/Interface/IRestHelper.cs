// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BackEnd.Encoder.Data;
using Portal.Domain.BackendContext.Entity.Base;
using Portal.Domain.BackendContext.Enum;
using RestSharp;

namespace Portal.BackEnd.Encoder.Interface
{
    public interface IRestHelper
    {
        TaskData GetTaskData(IRestResponse response);
        IRestResponse GetResponse(IRestRequest request);
        IRestRequest TaskRequestCreate();
        IRestRequest EncodeDataRequestCreate(string resource);
        IRestRequest SetStatusRequestCreate(string resource, int progress);
        IRestRequest FinishTaskRequestCreate(string resource, EncoderState encoderState, string fileId, string errorMessage);
        IEncodeData CreateEncodeData(IRestResponse restResponse, TypeOfTask typeOfTask);
    }
}