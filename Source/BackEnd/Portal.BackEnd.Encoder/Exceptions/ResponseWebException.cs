// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.BackEnd.Encoder.Exceptions
{
    public class ResponseWebException : ResponseException
    {
        public ResponseWebException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}