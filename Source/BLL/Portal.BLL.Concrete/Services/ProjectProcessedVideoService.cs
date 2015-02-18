// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.DAL.Context;
using Portal.DAL.Entities.Storage;
using Portal.DAL.Entities.Table;
using Portal.DAL.FileSystem;
using Portal.Domain.ProjectContext;
using Portal.Exceptions.CRUD;

namespace Portal.BLL.Concrete.Services
{
    public sealed class ProjectProcessedVideoService : IService<DomainProjectProcessedVideo>
    {
        private readonly IFileSystem _fileSystem;
        private readonly ITableRepository<ProjectEntity> _projectRepository;
        private readonly IFileUriProvider _uriProvider;

        public ProjectProcessedVideoService(IRepositoryFactory repositoryFactory, IFileUriProvider uriProvider, IFileSystem fileSystem)
        {
            _uriProvider = uriProvider;
            _fileSystem = fileSystem;
            _projectRepository = repositoryFactory.Create<ProjectEntity>();
        }

        public async Task<DomainProjectProcessedVideo> AddAsync(DomainProjectProcessedVideo entity)
        {
            ProjectEntity project = await _projectRepository.SingleOrDefaultAsync(p => p.Id == entity.ProjectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", entity.ProjectId));
            }

            ProjectEntity.EncodedVideo video = project.EncodedVideos.FirstOrDefault(v => v.ContentType == entity.ContentType && v.Width == entity.Width);
            if (video == null)
            {
                // if does not exist

                // adding to the project
                video = new ProjectEntity.EncodedVideo
                {
                    FileId = entity.FileId,
                    ContentType = entity.ContentType,
                    Width = entity.Width,
                    Height = entity.Height
                };

                List<ProjectEntity.EncodedVideo> encodedVideos = project.EncodedVideos.ToList();
                encodedVideos.Add(video);
                project.EncodedVideos = encodedVideos.ToArray();

                await _projectRepository.UpdateAsync(project);
            }


            // building result
            return BuildDomainObject(project, video);
        }

        public async Task<List<DomainProjectProcessedVideo>> AddAsync(IList<DomainProjectProcessedVideo> entities)
        {
            IEnumerable<IGrouping<string, DomainProjectProcessedVideo>> entitiesByProject = entities.GroupBy(p => p.ProjectId);

            var projectTasks = new List<Task>();
            var results = new List<DomainProjectProcessedVideo>();

            foreach (var projectEntities in entitiesByProject)
            {
                string projectId = projectEntities.Key;
                IGrouping<string, DomainProjectProcessedVideo> groupedEntities = projectEntities;

                // adding entities to project
                projectTasks.Add(Task.Run(async () =>
                {
                    ProjectEntity project = await _projectRepository.SingleOrDefaultAsync(p => p.Id == projectId);
                    if (project == null)
                    {
                        throw new NotFoundException(string.Format("Project {0} was not found", projectId));
                    }

                    List<ProjectEntity.EncodedVideo> projectVideos = project.EncodedVideos.ToList();
                    foreach (DomainProjectProcessedVideo entity in groupedEntities)
                    {
                        ProjectEntity.EncodedVideo video = project.EncodedVideos.FirstOrDefault(v => v.ContentType == entity.ContentType && v.Width == entity.Width);

                        // Add if not exists
                        if (video == null)
                        {
                            // adding to the project
                            video = new ProjectEntity.EncodedVideo
                            {
                                FileId = entity.FileId,
                                ContentType = entity.ContentType,
                                Width = entity.Width,
                                Height = entity.Height
                            };

                            projectVideos.Add(video);
                        }

                        // building result
                        results.Add(BuildDomainObject(project, video));
                    }

                    // updating project
                    project.EncodedVideos = projectVideos.ToArray();
                    await _projectRepository.UpdateAsync(project);
                }));
            }

            await Task.WhenAll(projectTasks);

            return results;
        }

        public async Task<DomainProjectProcessedVideo> GetAsync(DomainProjectProcessedVideo entity)
        {
            ProjectEntity project = await _projectRepository.SingleOrDefaultAsync(p => p.Id == entity.ProjectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", entity.ProjectId));
            }

            ProjectEntity.EncodedVideo video = project.EncodedVideos.Single(s => s.FileId == entity.FileId);


            // building result
            return BuildDomainObject(project, video);
        }

        public async Task<List<DomainProjectProcessedVideo>> GetListAsync(DomainProjectProcessedVideo entity)
        {
            ProjectEntity project = await _projectRepository.SingleOrDefaultAsync(p => p.Id == entity.ProjectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", entity.ProjectId));
            }

            var results = new List<DomainProjectProcessedVideo>();
            foreach (ProjectEntity.EncodedVideo video in project.EncodedVideos)
            {
                DomainProjectProcessedVideo result = BuildDomainObject(project, video);
                results.Add(result);
            }

            return results;
        }

        public async Task<DomainProjectProcessedVideo> EditAsync(DomainProjectProcessedVideo entity)
        {
            ProjectEntity project = await _projectRepository.SingleOrDefaultAsync(p => p.Id == entity.ProjectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", entity.ProjectId));
            }

            ProjectEntity.EncodedVideo video = project.EncodedVideos.FirstOrDefault(s => s.FileId == entity.FileId);
            if (video == null)
            {
                return null;
            }

            var newVideo = new ProjectEntity.EncodedVideo
            {
                FileId = entity.FileId,
                ContentType = entity.ContentType,
                Width = entity.Width,
                Height = entity.Height
            };


            // updating project
            List<ProjectEntity.EncodedVideo> encodedVideos = project.EncodedVideos.ToList();
            encodedVideos.Remove(video);
            encodedVideos.Add(newVideo);
            project.EncodedVideos = encodedVideos.ToArray();

            await _projectRepository.UpdateAsync(project);


            // building result
            return BuildDomainObject(project, newVideo);
        }

        public async Task DeleteAsync(DomainProjectProcessedVideo entity)
        {
            ProjectEntity project = await _projectRepository.SingleOrDefaultAsync(p => p.Id == entity.ProjectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", entity.ProjectId));
            }

            var tasks = new List<Task>();
            foreach (ProjectEntity.EncodedVideo video in project.EncodedVideos)
            {
                tasks.Add(_fileSystem.DeleteFileAsync(
                    new StorageFile
                    {
                        Id = video.FileId,
                        UserId = project.UserId
                    }));
            }

            if (project.EncodedVideos.Any())
            {
                project.EncodedVideos = new ProjectEntity.EncodedVideo[] { };
                tasks.Add(_projectRepository.UpdateAsync(project));
            }

            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
            }
        }

        public Task DeleteAsync(IList<DomainProjectProcessedVideo> entities)
        {
            throw new NotImplementedException();
        }


        private DomainProjectProcessedVideo BuildDomainObject(ProjectEntity project, ProjectEntity.EncodedVideo entity)
        {
            return new DomainProjectProcessedVideo
            {
                FileId = entity.FileId,
                ContentType = entity.ContentType,
                Width = entity.Width,
                Height = entity.Height,
                FileUri = _uriProvider.CreateUri(entity.FileId),
                ProjectId = project.Id,
                UserId = project.UserId
            };
        }
    }
}