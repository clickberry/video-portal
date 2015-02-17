// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BackEnd.Encoder.Interface;
using Portal.Domain.BackendContext.Entity;
using Portal.Domain.BackendContext.Entity.Base;
using Portal.Domain.BackendContext.Enum;
using RestSharp;
using RestSharp.Deserializers;

namespace Portal.BackEnd.Encoder.MiddleEndClient
{
    public class EncodeDeserializer : IEncodeDeserializer
    {
        private readonly IDeserializer _deserializer;

        public EncodeDeserializer(IDeserializer deserializer)
        {
            _deserializer = deserializer;
        }

        public EncodeTaskData EncodeTaskDataDeserealize(IRestResponse response)
        {
            EncodeTaskData encodeTaskData;
            encodeTaskData = _deserializer.Deserialize<EncodeTaskData>(response);

            return encodeTaskData;
        }

        public IEncodeData EncodeDataDeserialize(IRestResponse response, TypeOfTask typeOfTask)
        {
            switch (typeOfTask)
            {
                case TypeOfTask.Video:
                    return _deserializer.Deserialize<VideoEncodeData>(response);

                case TypeOfTask.Screenshot:
                    return _deserializer.Deserialize<ScreenshotEncodeData>(response);

                default:
                    return null;
            }
        }
    }
}