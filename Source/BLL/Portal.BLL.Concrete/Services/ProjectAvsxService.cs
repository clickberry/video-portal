// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.DAL.Entities.Storage;
using Portal.DAL.Entities.Table;
using Portal.DAL.FileSystem;
using Portal.DAL.Project;
using Portal.Domain.ProjectContext;
using Portal.Exceptions.CRUD;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    public sealed class ProjectAvsxService : IProjectAvsxService
    {
        private const string AvsxContentType = @"application/xml";
        private readonly IFileSystem _fileSystem;
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;
        private readonly IFileUriProvider _uriProvider;

        public ProjectAvsxService(IProjectRepository projectRepository, IMapper mapper, IFileUriProvider uriProvider, IFileSystem fileSystem)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _uriProvider = uriProvider;
            _fileSystem = fileSystem;
        }

        public async Task<DomainAvsx> AddAsync(string projectId, DomainAvsx entity)
        {
            ProjectEntity project = await _projectRepository.GetAsync(projectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", projectId));
            }

            // Check whether project already contains avsx
            if (!string.IsNullOrEmpty(project.AvsxFileId))
            {
                throw new ConflictException();
            }

            StorageFile storageFile;

            // uploading file
            using (FileStream stream = File.OpenRead(entity.FileUri))
            {
                storageFile = await _fileSystem.UploadFileFromStreamAsync(
                    new StorageFile(stream, AvsxContentType)
                    {
                        UserId = project.UserId,
                        FileName = entity.FileName,
                        Length = entity.FileLength
                    });
            }

            // updating project
            await _projectRepository.SetAvsxFileIdAsync(project.Id, storageFile.Id);

            DomainAvsx result = _mapper.Map<StorageFile, DomainAvsx>(storageFile);
            result.FileUri = _uriProvider.CreateUri(result.FileId);

            return result;
        }

        public async Task<DomainAvsx> GetAsync(string projectId)
        {
            ProjectEntity project = await _projectRepository.GetAsync(projectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", projectId));
            }

            if (string.IsNullOrEmpty(project.AvsxFileId))
            {
                throw new NotFoundException(string.Format("Project {0} does not have an avsx file", projectId));
            }

            StorageFile file = await _fileSystem.GetFilePropertiesAsync(new StorageFile { Id = project.AvsxFileId });
            if (file == null)
            {
                throw new NotFoundException(string.Format("Project's {0} avsx was not found", projectId));
            }

            DomainAvsx result = _mapper.Map<StorageFile, DomainAvsx>(file);
            result.FileUri = _uriProvider.CreateUri(result.FileId);

            return result;
        }

        public async Task DeleteAsync(string projectId)
        {
            ProjectEntity project = await _projectRepository.GetAsync(projectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", projectId));
            }

            if (string.IsNullOrEmpty(project.AvsxFileId))
            {
                throw new NotFoundException(string.Format("Project {0} does not have an avsx file", projectId));
            }

            StorageFile file = await _fileSystem.GetFilePropertiesAsync(new StorageFile { Id = project.AvsxFileId });
            if (file == null)
            {
                throw new NotFoundException(string.Format("Project's {0} avsx was not found", projectId));
            }

            // update project
            Task updateProjectTask = _projectRepository.SetAvsxFileIdAsync(projectId, null);

            await Task.WhenAll(new[]
            {
                _fileSystem.DeleteFileAsync(file),
                updateProjectTask
            });
        }
    }
}