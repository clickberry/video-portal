// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.DTO.Watch
{
    /// <summary>
    ///     Public video.
    /// </summary>
    public sealed class WatchVideo
    {
        /// <summary>
        ///     Gets or sets a content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     Gets or sets a width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        ///     Gets or sets a height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        ///     Gets or sets an URI.
        /// </summary>
        public string Uri { get; set; }
    }
}