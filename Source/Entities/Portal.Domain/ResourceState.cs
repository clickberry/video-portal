// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.Domain
{
    /// <summary>
    /// Defines resource state.
    /// </summary>
    public enum ResourceState
    {
        Available = 0,

        Blocked = 1,

        Deleted = 2
    }
}