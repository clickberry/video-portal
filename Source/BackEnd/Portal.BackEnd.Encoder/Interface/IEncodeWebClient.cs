// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BackEnd.Encoder.Data;
using Portal.Domain.BackendContext.Entity.Base;
using Portal.Domain.BackendContext.Enum;
using Wrappers;

namespace Portal.BackEnd.Encoder.Interface
{
    public interface IEncodeWebClient
    {
        string Resource { get; }

        void Initialize(string resource, string taskId, CancellationTokenSourceWrapper cancellationTokenSourceWrapper);
        TaskData GetTask();
        IEncodeData GetEntity(TypeOfTask encodeDataDeserializer);
        void SetStatus(int progress);
        void FinishTask(EncoderState encoderState, string fileId, string errorMessage);
    }
}