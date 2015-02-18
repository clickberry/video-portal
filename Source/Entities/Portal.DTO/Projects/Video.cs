// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.DTO.Projects
{
    public sealed class Video
    {
        /// <summary>
        ///     Gets or sets a video file uri.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        ///     Gets or sets a content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     Gets or sets a video width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        ///     Gets or sets a video height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        ///     Gets or sets a creation date.
        /// </summary>
        public DateTime Created { get; set; }
    }
}