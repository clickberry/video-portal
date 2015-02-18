// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Portal.DAL.Context;
using Portal.DAL.Entities.Storage;
using Portal.DAL.Entities.Table;
using Portal.DAL.FileSystem;
using Portal.DAL.User;
using Portal.Exceptions.CRUD;

namespace Portal.DAL.Azure.FileSystem
{
    public sealed class FileSystem : IFileSystem
    {
        private const string BlobName = "file";
        private const string CacheControl = "public, max-age=31536000";
        private readonly CloudBlobClient _blobClient;
        private readonly ITableRepository<FileEntity> _fileRepository;
        private readonly IUserRepository _userRepository;

        public FileSystem(IUserRepository userRepository, IRepositoryFactory repositoryFactory, CloudBlobClient blobClient)
        {
            _userRepository = userRepository;
            _blobClient = blobClient;
            _fileRepository = repositoryFactory.Create<FileEntity>();
        }

        public async Task<StorageFile> UploadArtifactFromStreamAsync(StorageFile file, CancellationToken cancellationToken = new CancellationToken())
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            if (file.Stream == null)
            {
                throw new ArgumentNullException("file", "Stream value should be defined.");
            }

            if (string.IsNullOrEmpty(file.UserId))
            {
                throw new ArgumentNullException("file", "UserId value should be defined.");
            }

            // Adds file to storage
            return await AddFileToStorageFromStreamAsync(file, true, cancellationToken);
        }

        public async Task<StorageFile> UploadFileFromStreamAsync(StorageFile file, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            if (file.Stream == null)
            {
                throw new ArgumentNullException("file", "Stream value should be defined.");
            }

            if (string.IsNullOrEmpty(file.UserId))
            {
                throw new ArgumentNullException("file", "UserId value should be defined.");
            }

            // Checks user storage space
            await CheckUserSpaceAsync(file);

            // Adds file to storage
            return await AddFileToStorageFromStreamAsync(file, false, cancellationToken);
        }

        public async Task<StorageFile> DownloadFileToStreamAsync(StorageFile file, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            if (string.IsNullOrEmpty(file.Id))
            {
                throw new ArgumentNullException("file", "Id value should be defined.");
            }

            if (file.Stream == null)
            {
                throw new ArgumentNullException("file", "Stream value should be defined.");
            }

            // Receive table entity
            FileEntity storageFile = await _fileRepository.GetAsync(file.Id);

            // Get blob reference
            CloudBlockBlob blob = _blobClient.GetContainerReference(storageFile.Id).GetBlockBlobReference(BlobName);

            // Download blob to stream
            await blob.DownloadToStreamAsync(file.Stream, cancellationToken);

            return new StorageFile
            {
                ContentType = storageFile.ContentType,
                Created = storageFile.Created,
                FileName = storageFile.Name,
                Id = storageFile.Id,
                Length = storageFile.Length,
                Modified = storageFile.Modified,
                UserId = storageFile.UserId,
                Uri = blob.Uri
            };
        }

        public async Task<StorageFile> GetFilePropertiesAsync(StorageFile file, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            if (string.IsNullOrEmpty(file.Id))
            {
                throw new ArgumentNullException("file", "Id value should be defined.");
            }

            // Receive table entity
            FileEntity storageFile = await _fileRepository.GetAsync(file.Id);
            if (storageFile == null)
            {
                throw new NotFoundException(string.Format("File {0} could not be found in storage", file.Id));
            }

            // Get blob reference
            CloudBlockBlob blob = _blobClient.GetContainerReference(storageFile.Id).GetBlockBlobReference(BlobName);

            await BlobIsExists(blob, storageFile.Id);

            return new StorageFile
            {
                ContentType = storageFile.ContentType,
                Created = storageFile.Created,
                FileName = storageFile.Name,
                Id = storageFile.Id,
                Length = storageFile.Length,
                Modified = storageFile.Modified,
                UserId = storageFile.UserId,
                Uri = blob.Uri
            };
        }

        public async Task<StorageFile> GetFilePropertiesSlimAsync(StorageFile file, CancellationToken cancellationToken = new CancellationToken())
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            if (string.IsNullOrEmpty(file.Id))
            {
                throw new ArgumentNullException("file", "Id value should be defined.");
            }

            // Receive table entity
            FileEntity storageFile = await _fileRepository.GetAsync(file.Id);

            return new StorageFile
            {
                ContentType = storageFile.ContentType,
                Created = storageFile.Created,
                FileName = storageFile.Name,
                Id = storageFile.Id,
                Length = storageFile.Length,
                Modified = storageFile.Modified,
                UserId = storageFile.UserId,
                Uri = null
            };
        }

        public async Task DeleteFileAsync(StorageFile file, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            if (string.IsNullOrEmpty(file.Id))
            {
                throw new ArgumentNullException("file", "Id value should be defined.");
            }

            if (string.IsNullOrEmpty(file.UserId))
            {
                throw new ArgumentNullException("file", "UserId value should be defined.");
            }

            // Receive table entity
            FileEntity storageFile = await _fileRepository.GetAsync(file.Id);
            if (storageFile == null)
            {
                throw new NotFoundException(string.Format("File {0} could not be found in storage", file.Id));
            }

            // Delete storage file & storage space entities
            await _fileRepository.DeleteAsync(storageFile);

            // Remove file from blob
            CloudBlobContainer container = _blobClient.GetContainerReference(storageFile.Id);
            await container.DeleteIfExistsAsync(cancellationToken);
        }

        private static async Task BlobIsExists(CloudBlockBlob blob, string fileId)
        {
            bool blobIsExist = await blob.ExistsAsync();
            if (!blobIsExist)
            {
                throw new NotFoundException(String.Format("File with id {0} does not exist.", fileId));
            }
        }

        /// <summary>
        ///     Checks user storage space.
        /// </summary>
        /// <param name="file">Storage file.</param>
        private async Task CheckUserSpaceAsync(StorageFile file)
        {
            // Checks user storage space
            List<FileEntity> storageSpace = await _fileRepository.ToListAsync(p => p.UserId == file.UserId);
            long currentSpace = storageSpace.Where(p => !p.IsArtifact).Sum(p => p.Length);

            UserEntity userProfile = await _userRepository.GetAsync(file.UserId);

            if (currentSpace + file.Length > userProfile.MaximumStorageSpace)
            {
                throw new EntityTooLargeException();
            }
        }

        /// <summary>
        ///     Uploads file to storage from stream.
        /// </summary>
        /// <param name="file">Storage file.</param>
        /// <param name="isArtifact">Defines whether file is artifact.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Result file.</returns>
        private async Task<StorageFile> AddFileToStorageFromStreamAsync(StorageFile file, bool isArtifact, CancellationToken cancellationToken)
        {
            // Result storage file
            var storageFile = new FileEntity
            {
                ContentType = file.ContentType,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
                Length = file.Length,
                Name = file.FileName,
                UserId = file.UserId,
                IsArtifact = isArtifact
            };

            // Create table entity
            storageFile = await _fileRepository.AddAsync(storageFile);

            // Create container
            CloudBlobContainer container = _blobClient.GetContainerReference(storageFile.Id);
            await TryCreateContainer(container, cancellationToken);
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            }, cancellationToken);

            // Create blob
            CloudBlockBlob blob = container.GetBlockBlobReference(BlobName);
            if (await blob.ExistsAsync(cancellationToken))
            {
                throw new ConflictException(string.Format("File with id {0} is already exists in Blob storage", storageFile.Id));
            }

            // Upload data
            blob.UploadFromStream(file.Stream);

            // Set properties
            await blob.FetchAttributesAsync(cancellationToken);
            blob.Properties.ContentType = file.ContentType;
            blob.Properties.CacheControl = CacheControl;
            await blob.SetPropertiesAsync(cancellationToken);

            return new StorageFile
            {
                ContentType = storageFile.ContentType,
                Created = storageFile.Created,
                FileName = storageFile.Name,
                Id = storageFile.Id,
                Length = storageFile.Length,
                Modified = storageFile.Modified,
                UserId = storageFile.UserId,
                Uri = blob.Uri
            };
        }

        private static async Task TryCreateContainer(CloudBlobContainer container, CancellationToken cancellationToken)
        {
            //
            // 2013-03-29
            //
            // HACK!
            //
            // "The magnitude of this hack compares favorably with that of the national debt" (c) Microsoft
            //

            while (true)
            {
                try
                {
                    await container.CreateIfNotExistsAsync(cancellationToken);
                    break;
                }
                catch (StorageException exception)
                {
                    RequestResult requestInformation = exception.RequestInformation;
                    var statusCode = (HttpStatusCode)requestInformation.HttpStatusCode;

                    if (statusCode != HttpStatusCode.Conflict)
                    {
                        throw;
                    }
                }
            }
        }
    }
}