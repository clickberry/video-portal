// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Portal.DAL.Entities.Storage;

namespace Portal.DAL.FileSystem
{
    /// <summary>
    ///     File system.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        ///     Uploads artifact from stream to the file system.
        ///     <remarks>Uploaded file won't be taken into account for storage space calculation.</remarks>
        /// </summary>
        /// <param name="file">Storage file.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Result file.</returns>
        Task<StorageFile> UploadArtifactFromStreamAsync(StorageFile file, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Uploads file from stream to the file system.
        /// </summary>
        /// <param name="file">Storage file.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Result file.</returns>
        Task<StorageFile> UploadFileFromStreamAsync(StorageFile file, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Downloads file to stream from file system.
        /// </summary>
        /// <param name="file">Storage file.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Result file.</returns>
        Task<StorageFile> DownloadFileToStreamAsync(StorageFile file, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Receives file properties from file system.
        /// </summary>
        /// <param name="file">Storage file.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Result file.</returns>
        Task<StorageFile> GetFilePropertiesAsync(StorageFile file, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Receives file properties from file system excluding requests to blob storage (only persistence info).
        /// </summary>
        /// <param name="file">Storage file.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Result file.</returns>
        Task<StorageFile> GetFilePropertiesSlimAsync(StorageFile file, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes file from file system.
        /// </summary>
        /// <param name="file">Storage file.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task DeleteFileAsync(StorageFile file, CancellationToken cancellationToken = default(CancellationToken));
    }
}