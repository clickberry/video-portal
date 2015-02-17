// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.DTO.Common;

namespace Portal.DTO.Files
{
    /// <summary>
    ///     File.
    /// </summary>
    public sealed class File : FileEntity
    {
        /// <summary>
        ///     Gets or sets a file id.
        /// </summary>
        public string Id { get; set; }
    }
}