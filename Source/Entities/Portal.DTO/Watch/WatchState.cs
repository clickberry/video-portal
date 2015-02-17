// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.DTO.Watch
{
    /// <summary>
    ///     Watch video state.
    /// </summary>
    public enum WatchState
    {
        /// <summary>
        ///     Not completely uploaded.
        /// </summary>
        Uploading,

        /// <summary>
        ///     In encoding state.
        /// </summary>
        Encoding,

        /// <summary>
        ///     Ready to watch.
        /// </summary>
        Ready
    }
}