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
    public sealed class ProjectScreenshotService : IProjectScreenshotService
    {
        private readonly IFileSystem _fileSystem;
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;
        private readonly IFileUriProvider _uriProvider;

        public ProjectScreenshotService(IProjectRepository projectRepository, IMapper mapper, IFileUriProvider uriProvider, IFileSystem fileSystem)
        {
            _projectRepository = projectRepository;
            _uriProvider = uriProvider;
            _fileSystem = fileSystem;
            _mapper = mapper;
        }

        public async Task<DomainScreenshot> AddAsync(string projectId, DomainScreenshot entity)
        {
            ProjectEntity project = await _projectRepository.GetAsync(projectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", projectId));
            }

            // Check whether project already contains screenshot
            if (!string.IsNullOrEmpty(project.ScreenshotFileId))
            {
                throw new ConflictException();
            }

            StorageFile storageFile;

            // uploading file
            using (FileStream stream = File.OpenRead(entity.FileUri))
            {
                storageFile = await _fileSystem.UploadFileFromStreamAsync(
                    new StorageFile(stream, entity.ContentType)
                    {
                        UserId = project.UserId,
                        FileName = entity.FileName,
                        Length = entity.FileLength
                    });
            }

            // updating project
            await _projectRepository.SetScreenshotFileIdAsync(project.Id, storageFile.Id);

            // building result
            DomainScreenshot result = _mapper.Map<StorageFile, DomainScreenshot>(storageFile);
            result.FileUri = _uriProvider.CreateUri(storageFile.Id);

            return result;
        }

        public async Task<DomainScreenshot> GetAsync(string projectId)
        {
            ProjectEntity project = await _projectRepository.GetAsync(projectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", projectId));
            }

            if (string.IsNullOrEmpty(project.ScreenshotFileId))
            {
                throw new NotFoundException(string.Format("Project {0} does not have a screenshot", projectId));
            }

            StorageFile file = await _fileSystem.GetFilePropertiesAsync(new StorageFile { Id = project.ScreenshotFileId });
            if (file == null)
            {
                throw new NotFoundException(string.Format("Project's {0} screenshot was not found", projectId));
            }

            DomainScreenshot result = _mapper.Map<StorageFile, DomainScreenshot>(file);
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

            if (string.IsNullOrEmpty(project.ScreenshotFileId))
            {
                throw new NotFoundException(string.Format("Project {0} does not have a screenshot", projectId));
            }

            StorageFile file = await _fileSystem.GetFilePropertiesAsync(new StorageFile { Id = project.ScreenshotFileId });
            if (file == null)
            {
                throw new NotFoundException(string.Format("Project's {0} screenshot was not found", projectId));
            }

            // updating project
            Task updateProjectTask = _projectRepository.SetScreenshotFileIdAsync(projectId, null);

            await Task.WhenAll(new[]
            {
                _fileSystem.DeleteFileAsync(file),
                updateProjectTask
            });
        }
    }
}