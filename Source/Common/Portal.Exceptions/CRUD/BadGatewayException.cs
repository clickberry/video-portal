// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.Exceptions.CRUD
{
    public sealed class BadGatewayException : PortalException
    {
        public BadGatewayException(Exception innerInnerException = null)
            : base(innerInnerException)
        {
        }

        public BadGatewayException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}