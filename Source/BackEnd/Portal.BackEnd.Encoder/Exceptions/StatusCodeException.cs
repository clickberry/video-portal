// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;

namespace Portal.BackEnd.Encoder.Exceptions
{
    public class StatusCodeException : ResponseException
    {
        public StatusCodeException(HttpStatusCode httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
        }

        public StatusCodeException(HttpStatusCode httpStatusCode, string message = null) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        public HttpStatusCode HttpStatusCode { get; set; }
    }
}