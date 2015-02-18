// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Wrappers.Interface;

namespace Wrappers.Implementation
{
    public class DateTimeWrapper : IDateTimeWrapper
    {
        public DateTime CurrentDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}