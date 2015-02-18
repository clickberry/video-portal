// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.DAL.Entities.Storage;
using Portal.DAL.FileSystem;
using Portal.Domain.FileContext;
using Portal.Exceptions.CRUD;

namespace Portal.BLL.Concrete.Services
{
    public sealed class UserFileService : IService<DomainUserFile>
    {
        private readonly IFileSystem _fileSystem;
        private readonly IFileUriProvider _uriProvider;

        public UserFileService(IFileUriProvider uriProvider, IFileSystem fileSystem)
        {
            _uriProvider = uriProvider;
            _fileSystem = fileSystem;
        }

        public async Task<DomainUserFile> AddAsync(DomainUserFile entity)
        {
            StorageFile storageFile;

            using (FileStream stream = File.OpenRead(entity.FileUri))
            {
                storageFile = await _fileSystem.UploadFileFromStreamAsync(
                    new StorageFile(stream, entity.ContentType)
                    {
                        UserId = entity.UserId,
                        ContentType = entity.ContentType,
                        Length = entity.FileLength,
                        FileName = entity.FileName
                    });
            }

            return new DomainUserFile
            {
                ContentType = storageFile.ContentType,
                Created = storageFile.Created,
                FileId = storageFile.Id,
                FileLength = storageFile.Length,
                FileUri = _uriProvider.CreateUri(storageFile.Id),
                Modified = storageFile.Modified,
                UserId = storageFile.UserId,
                FileName = storageFile.FileName
            };
        }

        public Task<List<DomainUserFile>> AddAsync(IList<DomainUserFile> entity)
        {
            throw new NotImplementedException();
        }

        public async Task<DomainUserFile> GetAsync(DomainUserFile entity)
        {
            StorageFile storageFile = await _fileSystem.GetFilePropertiesAsync(new StorageFile { Id = entity.FileId });

            return new DomainUserFile
            {
                ContentType = storageFile.ContentType,
                Created = storageFile.Created,
                FileId = storageFile.Id,
                FileLength = storageFile.Length,
                FileUri = _uriProvider.CreateUri(storageFile.Id),
                Modified = storageFile.Modified,
                UserId = storageFile.UserId,
                FileName = storageFile.FileName
            };
        }

        public Task<List<DomainUserFile>> GetListAsync(DomainUserFile entity)
        {
            throw new NotImplementedException();
        }

        public Task<DomainUserFile> EditAsync(DomainUserFile entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(DomainUserFile entity)
        {
            StorageFile storageFile = await _fileSystem.GetFilePropertiesAsync(new StorageFile
            {
                Id = entity.FileId
            });

            // User is allowed to delete only his files
            if (storageFile.UserId != entity.UserId)
            {
                throw new ForbiddenException();
            }

            await _fileSystem.DeleteFileAsync(storageFile);
        }

        public Task DeleteAsync(IList<DomainUserFile> entity)
        {
            throw new NotImplementedException();
        }
    }
}