// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using Portal.BLL.Concrete.Infrastructure.ProcessedEntity;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.DAL.Entities.Storage;
using Portal.DAL.Entities.Table;
using Portal.DAL.FileSystem;
using Portal.DAL.Project;
using Portal.Domain.ProcessedVideoContext;
using Portal.Domain.ProjectContext;
using Portal.Exceptions.CRUD;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    public sealed class ProjectVideoService : IProjectVideoService
    {
        private readonly IFileSystem _fileSystem;
        private readonly IMapper _mapper;
        private readonly IProcessedEntityManager _processedEntityManager;
        private readonly IProcessedVideoHandler _processedVideoHandler;
        private readonly IProjectRepository _projectRepository;
        private readonly IFileUriProvider _uriProvider;

        public ProjectVideoService(
            IProjectRepository projectRepository,
            IMapper mapper,
            IFileUriProvider uriProvider,
            IFileSystem fileSystem,
            IProcessedVideoHandler processedVideoHandler,
            IProcessedEntityManager processedEntityManager)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _uriProvider = uriProvider;
            _fileSystem = fileSystem;
            _processedVideoHandler = processedVideoHandler;
            _processedEntityManager = processedEntityManager;
        }

        public async Task<DomainVideo> AddAsync(string projectId, DomainVideo entity)
        {
            ProjectEntity project = await _projectRepository.GetAsync(projectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", projectId));
            }

            // Check whether project already contains video
            if (project.ProjectType == (int)VideoType.Internal && !string.IsNullOrEmpty(project.OriginalVideoFileId) ||
                project.ProjectType == (int)VideoType.External && !string.IsNullOrEmpty(project.VideoSource))
            {
                throw new ConflictException();
            }

            StorageFile storageFile;

            // adding file to the file system
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
            await _projectRepository.SetVideoAsync(project.Id, (int)VideoType.Internal, storageFile.Id);

            // building result
            DomainVideo result = _mapper.Map<StorageFile, DomainVideo>(storageFile);
            result.FileUri = _uriProvider.CreateUri(result.FileId);

            // adding task to the processing queue
            ProcessedMediaModel processedModel = await _processedEntityManager.GetProcessedMediaModel(entity.FileUri, projectId);
            await _processedVideoHandler.AddVideo(project.Id, project.UserId, result, processedModel);

            return result;
        }

        public async Task<DomainVideo> GetAsync(string projectId)
        {
            ProjectEntity project = await _projectRepository.GetAsync(projectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", projectId));
            }

            if (string.IsNullOrEmpty(project.OriginalVideoFileId))
            {
                throw new NotFoundException(string.Format("Project {0} does not have a video", projectId));
            }


            StorageFile file = await _fileSystem.GetFilePropertiesAsync(new StorageFile { Id = project.OriginalVideoFileId });
            if (file == null)
            {
                throw new NotFoundException(string.Format("Project's {0} video was not found", projectId));
            }

            DomainVideo video = _mapper.Map<StorageFile, DomainVideo>(file);
            video.FileUri = _uriProvider.CreateUri(video.FileId);

            return video;
        }

        public async Task DeleteAsync(string projectId)
        {
            ProjectEntity project = await _projectRepository.GetAsync(projectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", projectId));
            }

            if (string.IsNullOrEmpty(project.OriginalVideoFileId))
            {
                throw new NotFoundException(string.Format("Project {0} dows not have a video", projectId));
            }

            StorageFile file = await _fileSystem.GetFilePropertiesAsync(new StorageFile { Id = project.OriginalVideoFileId });
            if (file == null)
            {
                throw new NotFoundException(string.Format("Project's {0} video was not found", projectId));
            }


            // updating project
            Task updateProjectTask = _projectRepository.SetVideoAsync(projectId, (int)VideoType.Internal, null);

            await Task.WhenAll(new[]
            {
                _processedVideoHandler.RemoveVideo(projectId),
                _fileSystem.DeleteFileAsync(file),
                updateProjectTask
            });
        }
    }
}