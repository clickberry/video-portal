// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.Exceptions.Billing
{
    public sealed class BillingException : PortalException
    {
        public BillingException(Exception innerInnerException = null)
            : base(innerInnerException)
        {
        }

        public BillingException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}