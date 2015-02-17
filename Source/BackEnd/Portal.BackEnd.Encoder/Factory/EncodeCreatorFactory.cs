// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Portal.BackEnd.Encoder.Interface;
using Portal.Domain.BackendContext.Entity;
using Portal.Domain.BackendContext.Entity.Base;

namespace Portal.BackEnd.Encoder.Factory
{
    public class EncodeCreatorFactory : IEncodeCreatorFactory
    {
        private readonly Dictionary<Type, Func<IEncodeData, IEncodeCreator>> _dictionary;

        public EncodeCreatorFactory()
        {
            _dictionary = new Dictionary<Type, Func<IEncodeData, IEncodeCreator>>
            {
                { typeof (VideoEncodeData), data => new VideoEncodeCreator((VideoEncodeData)data) },
                { typeof (ScreenshotEncodeData), data => new ScreenshotEncodeCreator((ScreenshotEncodeData)data) }
            };
        }

        public IEncodeCreator Create(IEncodeData encodeData)
        {
            return _dictionary[encodeData.GetType()](encodeData);
        }
    }
}