// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.DTO.Common
{
    /// <summary>
    ///     File entity.
    /// </summary>
    public class FileEntity
    {
        /// <summary>
        ///     Gets or sets a file uri.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        ///     Gets or sets a name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     Gets or sets a file length.
        /// </summary>
        public long Length { get; set; }
    }
}