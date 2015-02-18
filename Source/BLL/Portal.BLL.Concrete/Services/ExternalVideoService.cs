// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Portal.BLL.Services;
using Portal.DAL.Entities.Table;
using Portal.DAL.Project;
using Portal.Domain.ProjectContext;
using Portal.Exceptions.CRUD;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    public sealed class ExternalVideoService : IExternalVideoService
    {
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;

        public ExternalVideoService(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<DomainExternalVideo> AddAsync(string projectId, DomainExternalVideo entity)
        {
            ProjectEntity project = await _projectRepository.GetAsync(projectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", projectId));
            }

            if (project.VideoType == (int)VideoType.Internal && !string.IsNullOrEmpty(project.OriginalVideoFileId) ||
                project.VideoType == (int)VideoType.External && !string.IsNullOrEmpty(project.VideoSource))
            {
                throw new ConflictException(string.Format("Project {0} already contains a video.", projectId));
            }

            // updating project
            project.VideoType = (int)VideoType.External;
            project.VideoSource = entity.VideoUri;
            project.VideoSourceProductName = entity.ProductName;
            project.Modified = DateTime.UtcNow;

            await _projectRepository.SetVideoAsync(project.Id, project.VideoType, project.VideoSource, project.VideoSourceProductName);

            return _mapper.Map<ProjectEntity, DomainExternalVideo>(project);
        }

        public async Task<DomainExternalVideo> GetAsync(string projectId)
        {
            ProjectEntity project = await _projectRepository.GetAsync(projectId);
            if (project == null || project.VideoType != (int)VideoType.External || string.IsNullOrEmpty(project.VideoSource))
            {
                throw new NotFoundException(string.Format("Project's {0} external video was not found.", projectId));
            }

            return _mapper.Map<ProjectEntity, DomainExternalVideo>(project);
        }

        public async Task DeleteAsync(string projectId)
        {
            ProjectEntity project = await _projectRepository.GetAsync(projectId);
            if (project == null || project.VideoType != (int)VideoType.External || string.IsNullOrEmpty(project.VideoSource))
            {
                throw new NotFoundException(string.Format("Project's {0} external video was not found.", projectId));
            }

            await _projectRepository.SetVideoAsync(projectId, (int)VideoType.Internal, null);
        }
    }
}