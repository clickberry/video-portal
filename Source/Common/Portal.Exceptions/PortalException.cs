// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.Exceptions
{
    /// <summary>
    ///     Base class for all custom portal exceptions.
    /// </summary>
    public abstract class PortalException : ApplicationException
    {
        protected PortalException()
        {
        }

        protected PortalException(Exception innerException)
            : base(innerException == null ? string.Empty : innerException.Message, innerException)
        {
        }

        protected PortalException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}