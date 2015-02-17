// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
    public sealed class ProjectProcessedScreenshotService : IService<DomainProjectProcessedScreenshot>
    {
        private readonly IFileSystem _fileSystem;
        private readonly ITableRepository<ProjectEntity> _projectRepository;
        private readonly IFileUriProvider _uriProvider;

        public ProjectProcessedScreenshotService(IRepositoryFactory repositoryFactory, IFileUriProvider uriProvider, IFileSystem fileSystem)
        {
            _uriProvider = uriProvider;
            _fileSystem = fileSystem;
            _projectRepository = repositoryFactory.Create<ProjectEntity>();
        }

        public async Task<DomainProjectProcessedScreenshot> AddAsync(DomainProjectProcessedScreenshot entity)
        {
            ProjectEntity project = await _projectRepository.SingleOrDefaultAsync(p => p.Id == entity.ProjectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", entity.ProjectId));
            }

            ProjectEntity.EncodedScreenshot screenshot = project.EncodedScreenshots.FirstOrDefault(s => s.ContentType == entity.ContentType && s.Width == entity.Width);
            if (screenshot == null)
            {
                // if does not exist

                // adding to the project
                screenshot = new ProjectEntity.EncodedScreenshot
                {
                    FileId = entity.FileId,
                    ContentType = entity.ContentType,
                    Width = entity.Width,
                    Height = entity.Height
                };

                List<ProjectEntity.EncodedScreenshot> encodedScreenshots = project.EncodedScreenshots.ToList();
                encodedScreenshots.Add(screenshot);
                project.EncodedScreenshots = encodedScreenshots.ToArray();

                await _projectRepository.UpdateAsync(project);
            }


            // building result
            return BuildDomainObject(project, screenshot);
        }

        public async Task<List<DomainProjectProcessedScreenshot>> AddAsync(IList<DomainProjectProcessedScreenshot> entities)
        {
            IEnumerable<IGrouping<string, DomainProjectProcessedScreenshot>> entitiesByProject = entities.GroupBy(p => p.ProjectId);

            var projectTasks = new List<Task>();
            var results = new List<DomainProjectProcessedScreenshot>();

            foreach (var projectEntities in entitiesByProject)
            {
                string projectId = projectEntities.Key;
                IGrouping<string, DomainProjectProcessedScreenshot> groupedEntities = projectEntities;

                // adding entities to project
                projectTasks.Add(Task.Run(async () =>
                {
                    ProjectEntity project = await _projectRepository.SingleOrDefaultAsync(p => p.Id == projectId);
                    if (project == null)
                    {
                        throw new NotFoundException(string.Format("Project {0} was not found", projectId));
                    }

                    List<ProjectEntity.EncodedScreenshot> projectScreenshots = project.EncodedScreenshots.ToList();
                    foreach (DomainProjectProcessedScreenshot entity in groupedEntities)
                    {
                        ProjectEntity.EncodedScreenshot screenshot = project.EncodedScreenshots.FirstOrDefault(v => v.ContentType == entity.ContentType && v.Width == entity.Width);
                        if (screenshot == null)
                        {
                            // if does not exist

                            // adding to the project
                            screenshot = new ProjectEntity.EncodedScreenshot
                            {
                                FileId = entity.FileId,
                                ContentType = entity.ContentType,
                                Width = entity.Width,
                                Height = entity.Height
                            };

                            projectScreenshots.Add(screenshot);
                        }


                        // building result
                        results.Add(BuildDomainObject(project, screenshot));
                    }

                    // updating project
                    project.EncodedScreenshots = projectScreenshots.ToArray();
                    await _projectRepository.UpdateAsync(project);
                }));
            }

            await Task.WhenAll(projectTasks);

            return results;
        }

        public async Task<DomainProjectProcessedScreenshot> GetAsync(DomainProjectProcessedScreenshot entity)
        {
            ProjectEntity project = await _projectRepository.SingleOrDefaultAsync(p => p.Id == entity.ProjectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", entity.ProjectId));
            }

            ProjectEntity.EncodedScreenshot screenshot = project.EncodedScreenshots.Single(s => s.FileId == entity.FileId);

            // building result
            return BuildDomainObject(project, screenshot);
        }

        public async Task<List<DomainProjectProcessedScreenshot>> GetListAsync(DomainProjectProcessedScreenshot entity)
        {
            ProjectEntity project = await _projectRepository.SingleOrDefaultAsync(p => p.Id == entity.ProjectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", entity.ProjectId));
            }

            return project.EncodedScreenshots.Select(screenshot => BuildDomainObject(project, screenshot)).ToList();
        }

        public async Task<DomainProjectProcessedScreenshot> EditAsync(DomainProjectProcessedScreenshot entity)
        {
            ProjectEntity project = await _projectRepository.SingleOrDefaultAsync(p => p.Id == entity.ProjectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", entity.ProjectId));
            }

            ProjectEntity.EncodedScreenshot screenshot = project.EncodedScreenshots.FirstOrDefault(s => s.FileId == entity.FileId);
            if (screenshot == null)
            {
                return null;
            }

            var newScreenshot = new ProjectEntity.EncodedScreenshot
            {
                FileId = entity.FileId,
                ContentType = entity.ContentType,
                Width = entity.Width,
                Height = entity.Height
            };


            // updating project
            List<ProjectEntity.EncodedScreenshot> encodedScreenshots = project.EncodedScreenshots.ToList();
            encodedScreenshots.Remove(screenshot);
            encodedScreenshots.Add(newScreenshot);
            project.EncodedScreenshots = encodedScreenshots.ToArray();

            await _projectRepository.UpdateAsync(project);


            // building result
            return BuildDomainObject(project, newScreenshot);
        }

        public async Task DeleteAsync(DomainProjectProcessedScreenshot entity)
        {
            ProjectEntity project = await _projectRepository.SingleOrDefaultAsync(p => p.Id == entity.ProjectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found", entity.ProjectId));
            }

            var tasks = new List<Task>();
            foreach (ProjectEntity.EncodedScreenshot screenshot in project.EncodedScreenshots)
            {
                tasks.Add(_fileSystem.DeleteFileAsync(
                    new StorageFile
                    {
                        Id = screenshot.FileId,
                        UserId = project.UserId
                    }));
            }

            if (project.EncodedScreenshots.Any())
            {
                project.EncodedScreenshots = new ProjectEntity.EncodedScreenshot[] { };
                tasks.Add(_projectRepository.UpdateAsync(project));
            }

            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
            }
        }

        public Task DeleteAsync(IList<DomainProjectProcessedScreenshot> entities)
        {
            throw new NotImplementedException();
        }


        private DomainProjectProcessedScreenshot BuildDomainObject(ProjectEntity project, ProjectEntity.EncodedScreenshot entity)
        {
            return new DomainProjectProcessedScreenshot
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