// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.Exceptions.Multimedia
{
    public class VideoFormatException : PortalException
    {
        public VideoFormatException(ParamType paramType, Exception innerException = null)
            : base(innerException)
        {
            ParamType = paramType;
        }

        public VideoFormatException(ParamType paramType, string message, Exception innerException = null)
            : base(message, innerException)
        {
            ParamType = paramType;
        }

        public ParamType ParamType { get; private set; }
    }
}