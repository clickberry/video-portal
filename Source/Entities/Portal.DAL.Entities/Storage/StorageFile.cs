// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;

namespace Portal.DAL.Entities.Storage
{
    /// <summary>
    ///     Storage file entity.
    /// </summary>
    public sealed class StorageFile
    {
        public StorageFile(Stream stream, string contentType)
        {
            Stream = stream;
            ContentType = contentType;
            Length = Stream.Length;
        }

        public StorageFile()
        {
        }

        /// <summary>
        ///     Gets a file stream.
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        ///     Gets a creation date.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     Gets a modification date.
        /// </summary>
        public DateTime Modified { get; set; }

        /// <summary>
        ///     Gets or sets a file identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets a file length.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        ///     Gets or sets an owner user identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Gets a file mime type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     Gets or sets a file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     Gets or sets a file URI.
        /// </summary>
        public Uri Uri { get; set; }


        [Obsolete("For backward compatibility only")]
        public string Hash { get; set; }
    }
}