// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Services;
using Portal.DAL.Entities.Storage;
using Portal.DAL.Entities.Table;
using Portal.DAL.FileSystem;
using Portal.DAL.Project;
using Portal.Domain.ProfileContext;
using Portal.Domain.ProjectContext;
using Portal.Exceptions.CRUD;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    public sealed class ProjectService : IProjectService
    {
        private const int SitemapPageSize = 10000;

        private readonly IFileSystem _fileSystem;
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;

        public ProjectService(
            IProjectRepository projectRepository,
            IMapper mapper,
            IFileSystem fileSystem)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _fileSystem = fileSystem;
        }

        public async Task<DomainProject> AddAsync(DomainProject entity)
        {
            entity.Created = DateTime.UtcNow;
            entity.Modified = DateTime.UtcNow;
            entity.HitsCount = 0;

            ProjectEntity projectEntity = _mapper.Map<DomainProject, ProjectEntity>(entity);
            ProjectEntity project = await _projectRepository.AddAsync(projectEntity);

            return _mapper.Map<ProjectEntity, DomainProject>(project);
        }

        public Task<List<DomainProject>> AddAsync(IList<DomainProject> entity)
        {
            throw new NotImplementedException();
        }

        public async Task<DomainProject> GetAsync(DomainProject entity)
        {
            ProjectEntity project = await _projectRepository.GetAsync(entity.Id);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project with id '{0}' was not found.", entity.Id));
            }

            if (project.UserId != entity.UserId)
            {
                throw new ForbiddenException();
            }

            return _mapper.Map<ProjectEntity, DomainProject>(project);
        }

        public async Task<List<DomainProject>> GetListAsync(DomainProject entity)
        {
            List<ProjectEntity> projects = await _projectRepository.GetUserProjectsAsync(entity.UserId);
            if (!projects.Any())
            {
                return new List<DomainProject>();
            }

            return projects.Select(p => _mapper.Map<ProjectEntity, DomainProject>(p)).ToList();
        }

        public async Task<int> GetSitemapPageCountAsync()
        {
            int result = _projectRepository.Context.Count();
            return result/SitemapPageSize + 1;
        }

        public async Task<List<DomainProject>> GetSitemapPageAsync(int pageNumber)
        {
            if (pageNumber < 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber");
            }

            List<ProjectEntity> projects = _projectRepository.Context.Skip(pageNumber*SitemapPageSize).Take(SitemapPageSize).ToList();
            return projects.Select(p => _mapper.Map<ProjectEntity, DomainProject>(p)).ToList();
        }

        public async Task<DomainProject> EditAsync(DomainProject entity)
        {
            ProjectEntity project = await _projectRepository.GetAsync(entity.Id);
            if (project.UserId != entity.UserId)
            {
                throw new ForbiddenException();
            }

            ProjectEntity updatedProject = _mapper.Map<DomainProject, ProjectEntity>(entity);

            // Overwriting not-modified fields
            updatedProject.Created = project.Created;
            updatedProject.Modified = DateTime.UtcNow;
            updatedProject.ProjectType = project.ProjectType;
            updatedProject.ProjectSubtype = project.ProjectSubtype;
            updatedProject.ProductId = project.ProductId;
            updatedProject.HitsCount = project.HitsCount;

            updatedProject = await _projectRepository.UpdateAsync(updatedProject);

            return _mapper.Map<ProjectEntity, DomainProject>(updatedProject);
        }

        public async Task DeleteAsync(DomainProject entity)
        {
            ProjectEntity project = await _projectRepository.GetAsync(entity.Id);
            if (project == null)
            {
                throw new NotFoundException();
            }

            if (project.UserId != entity.UserId)
            {
                throw new ForbiddenException();
            }


            var tasks = new List<Task>();

            // removing avsx
            if (!string.IsNullOrEmpty(project.AvsxFileId))
            {
                tasks.Add(_fileSystem.DeleteFileAsync(new StorageFile { Id = project.AvsxFileId, UserId = project.UserId }));
            }

            // removing video
            if (!string.IsNullOrEmpty(project.OriginalVideoFileId))
            {
                tasks.Add(_fileSystem.DeleteFileAsync(new StorageFile { Id = project.OriginalVideoFileId, UserId = project.UserId }));
            }

            // removing screenshot
            if (!string.IsNullOrEmpty(project.ScreenshotFileId))
            {
                tasks.Add(_fileSystem.DeleteFileAsync(new StorageFile { Id = project.ScreenshotFileId, UserId = project.UserId }));
            }

            // removing encoded screenshots
            tasks.AddRange(project.EncodedScreenshots.Select(s => _fileSystem.DeleteFileAsync(new StorageFile { Id = s.FileId, UserId = project.UserId })));

            // removing encoded videos
            tasks.AddRange(project.EncodedVideos.Select(s => _fileSystem.DeleteFileAsync(new StorageFile { Id = s.FileId, UserId = project.UserId })));

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception e)
            {
                // could not delete all related files
                Trace.TraceError("Could not delete all related files for project {0}: {1}", project.Id, e);
            }

            // removing project with all embedded data: avsx, video, screenshot
            await _projectRepository.DeleteAsync(project.Id);
        }

        public Task DeleteAsync(IList<DomainProject> entities)
        {
            return Task.WhenAll(entities.Select(DeleteAsync));
        }

        public Task IncrementHitsCounterAsync(string projectId)
        {
            return _projectRepository.IncrementHitsCounterAsync(projectId);
        }

        public Task UpdateLikesCounterAsync(string projectId, long count)
        {
            return _projectRepository.UpdateLikesCounterAsync(projectId, count);
        }

        public Task UpdateDislikesCounterAsync(string projectId, long count)
        {
            return _projectRepository.UpdateDislikesCounterAsync(projectId, count);
        }

        public Task IncrementAbuseCounterAsync(string projectId)
        {
            return _projectRepository.IncrementAbuseCounterAsync(projectId);
        }

        public async Task<List<DomainProject>> GetProjectListByUsersAsync(IEnumerable<DomainUser> users)
        {
            var userIds = users.Select(u => u.Id).ToArray();
            var projects = await _projectRepository.GetByUserIdsAsync(userIds);

            return projects.Select(p => _mapper.Map<ProjectEntity, DomainProject>(p)).ToList();
        }
    }
}