// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.Domain.ProcessedVideoContext.States
{
    /// <summary>
    ///     Defines task processing states.
    /// </summary>
    public enum TaskState
    {
        Waiting = 0,

        Reserved = 1,

        Processing = 2,

        Completed = 4,

        Failed = 5,

        Cancelled = 6,

        Corrupted = 7,

        Deleted = 8
    }
}