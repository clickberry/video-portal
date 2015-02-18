// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace MediaInfoLibrary
{
    public sealed class MediaInfoException : ApplicationException
    {
        public MediaInfoException(string message = null, Exception exception = null)
            : base(message, exception)
        {
        }
    }
}