// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.Exceptions.CRUD
{
    public sealed class PaymentRequiredException : PortalException
    {
        public PaymentRequiredException(Exception innerInnerException = null)
            : base(innerInnerException)
        {
        }

        public PaymentRequiredException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}