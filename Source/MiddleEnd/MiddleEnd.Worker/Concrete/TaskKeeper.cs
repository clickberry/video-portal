// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MiddleEnd.Worker.Abstract;
using Portal.BLL.Services;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.DAL.Project;
using Portal.Domain.ProcessedVideoContext;
using Portal.Domain.ProcessedVideoContext.States;
using Portal.Domain.ProjectContext;
using Portal.Mappers;

namespace MiddleEnd.Worker.Concrete
{
    /// <summary>
    ///     Handles synchronization of the scheduled tasks.
    /// </summary>
    public sealed class TaskKeeper : ITaskKeeper
    {
        private readonly List<TaskState> _completedStates = new List<TaskState>
        {
            TaskState.Completed,
            TaskState.Corrupted,
            TaskState.Deleted
        };

        private readonly IMapper _mapper;
        private readonly ITableRepository<ProcessedScreenshotEntity> _processedScreenshotRepository;
        private readonly ITableRepository<ProcessedVideoEntity> _processedVideoRepository;
        private readonly IService<DomainProjectProcessedScreenshot> _projectProcessedScreenshotService;
        private readonly IService<DomainProjectProcessedVideo> _projectProcessedVideoService;
        private readonly IProjectRepository _projectRepository;
        private readonly List<IProcessedEntity> _tasksList = new List<IProcessedEntity>();
        private readonly ITableRepository<VideoQueueEntity> _videoQueueRepository;
        private readonly List<DomainVideoQueue> _videoQueues = new List<DomainVideoQueue>();
        private DateTime _lastSave;

        public TaskKeeper(
            IMapper mapper,
            IRepositoryFactory repositoryFactory,
            IProjectRepository projectRepository,
            IService<DomainProjectProcessedScreenshot> projectProcessedScreenshotService,
            IService<DomainProjectProcessedVideo> projectProcessedVideoService)
        {
            _mapper = mapper;

            _projectRepository = projectRepository;
            _projectProcessedScreenshotService = projectProcessedScreenshotService;
            _projectProcessedVideoService = projectProcessedVideoService;

            _processedScreenshotRepository = repositoryFactory.Create<ProcessedScreenshotEntity>();
            _processedVideoRepository = repositoryFactory.Create<ProcessedVideoEntity>();
            _videoQueueRepository = repositoryFactory.Create<VideoQueueEntity>();

            _lastSave = DateTime.MinValue;
        }

        public IEnumerable<IProcessedEntity> GetTasks()
        {
            return _tasksList;
        }

        /// <summary>
        ///     Updates entities.
        /// </summary>
        public async Task Update()
        {
            await UpdateProcessingsList();
            await UpdateModifiedEntities();
        }

        /// <summary>
        ///     Updates modified entities.
        /// </summary>
        /// <returns></returns>
        private async Task UpdateModifiedEntities()
        {
            DateTime currentDateTime = DateTime.UtcNow;

            // Update modified entities
            List<IProcessedEntity> updatedEntitiesList = GetTasks().Where(p => p.Modified > _lastSave).ToList();

            if (updatedEntitiesList.Count == 0)
            {
                return;
            }

            // Update tasks
            await SaveModifiedEntities(updatedEntitiesList);

            // Check completed
            List<DomainVideoQueue> completedVideos = _videoQueues.Where(p => p.Tasks.All(q => _completedStates.Contains(q.State))).ToList();
            IEnumerable<IGrouping<string, IProcessedEntity>> processedTasks = updatedEntitiesList.Where(p => p.State == TaskState.Completed).GroupBy(p => p.ProjectId);

            foreach (var processedEntities in processedTasks)
            {
                await AddProcessedEntities(processedEntities.Key, processedEntities.ToList());
            }

            // Check for completed queues
            foreach (DomainVideoQueue videoQueue in completedVideos)
            {
                await SaveModifiedEntities(videoQueue.Tasks);
                await DeleteProcessedEntities(videoQueue.Tasks);

                // Remove processed entities
                foreach (IProcessedEntity processedEntity in videoQueue.Tasks)
                {
                    _tasksList.Remove(processedEntity);
                }

                // Removed processed videoQueue
                try
                {
                    VideoQueueEntity videoQueueEntity = _mapper.Map<DomainVideoQueue, VideoQueueEntity>(videoQueue);
                    await _videoQueueRepository.DeleteAsync(videoQueueEntity);
                }
                catch (Exception e)
                {
                    Trace.TraceError("Failed to delete video processings: {0}", e);
                }

                _videoQueues.Remove(videoQueue);
            }

            _lastSave = currentDateTime;
        }

        private async Task SaveModifiedEntities(List<IProcessedEntity> processedEntities)
        {
            IEnumerable<ProcessedScreenshotEntity> screenshots = processedEntities.OfType<DomainProcessedScreenshot>()
                .Select(p => _mapper.Map<DomainProcessedScreenshot, ProcessedScreenshotEntity>(p));

            try
            {
                await _processedScreenshotRepository.UpdateAsync(screenshots);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to edit processed screenshot: {0}", e);
            }

            IEnumerable<ProcessedVideoEntity> videos = processedEntities.OfType<DomainProcessedVideo>()
                .Select(p => _mapper.Map<DomainProcessedVideo, ProcessedVideoEntity>(p));

            try
            {
                await _processedVideoRepository.UpdateAsync(videos);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to edit processed video: {0}", e);
            }
        }

        private async Task DeleteProcessedEntities(List<IProcessedEntity> processedEntities)
        {
            IEnumerable<ProcessedScreenshotEntity> screenshots = processedEntities.OfType<DomainProcessedScreenshot>()
                .Select(p => _mapper.Map<DomainProcessedScreenshot, ProcessedScreenshotEntity>(p));

            try
            {
                await _processedScreenshotRepository.DeleteAsync(screenshots);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to delete processed screenshot: {0}", e);
            }

            IEnumerable<ProcessedVideoEntity> videos = processedEntities.OfType<DomainProcessedVideo>()
                .Select(p => _mapper.Map<DomainProcessedVideo, ProcessedVideoEntity>(p));

            try
            {
                await _processedVideoRepository.DeleteAsync(videos);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to delete processed video: {0}", e);
            }
        }

        private async Task AddProcessedEntities(string projectId, IList<IProcessedEntity> processedEntities)
        {
            ProjectEntity project = await _projectRepository.GetAsync(projectId);

            if (project == null)
            {
                return;
            }

            List<DomainProjectProcessedScreenshot> screenshots = processedEntities.OfType<DomainProcessedScreenshot>()
                .Select(p => new DomainProjectProcessedScreenshot
                {
                    ProjectId = project.Id,
                    UserId = project.UserId,
                    FileId = p.DestinationFileId,
                    ContentType = p.ContentType,
                    Height = p.ScreenshotParam.ImageHeight,
                    Width = p.ScreenshotParam.ImageWidth,
                }).ToList();

            try
            {
                await _projectProcessedScreenshotService.AddAsync(screenshots);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to add project processed screenshots: {0}", e);
            }

            List<DomainProjectProcessedVideo> videos = processedEntities.OfType<DomainProcessedVideo>()
                .Select(p => new DomainProjectProcessedVideo
                {
                    ProjectId = project.Id,
                    UserId = project.UserId,
                    FileId = p.DestinationFileId,
                    ContentType = p.ContentType,
                    Height = p.VideoParam.VideoHeight,
                    Width = p.VideoParam.VideoWidth,
                }).ToList();

            try
            {
                await _projectProcessedVideoService.AddAsync(videos);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to add project processed videos: {0}", e);
            }
        }

        /// <summary>
        ///     Updates the processing list.
        /// </summary>
        /// <returns></returns>
        private async Task UpdateProcessingsList()
        {
            // Receive all tasks
            List<DomainVideoQueue> videoQueues;

            try
            {
                videoQueues = (await _videoQueueRepository.ToListAsync())
                    .Select(p => _mapper.Map<VideoQueueEntity, DomainVideoQueue>(p)).ToList();
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to receive video queue item: {0}", e);
                return;
            }

            // Update tasks collection
            foreach (DomainVideoQueue videoQueue in videoQueues)
            {
                // Skip processed entities
                var fileId = videoQueue.FileId;
                if (_videoQueues.Any(p => p.FileId == fileId))
                {
                    continue;
                }

                await UpdateVideoQueue(videoQueue);

                _videoQueues.Add(videoQueue);
                _tasksList.AddRange(videoQueue.Tasks);
            }
        }

        private async Task UpdateVideoQueue(DomainVideoQueue videoQueue)
        {
            videoQueue.Tasks = new List<IProcessedEntity>();

            // Receive processed videos
            try
            {
                videoQueue.Tasks.AddRange(
                    (await _processedVideoRepository.ToListAsync(p => p.SourceFileId == videoQueue.FileId))
                        .Select(p => _mapper.Map<ProcessedVideoEntity, DomainProcessedVideo>(p)));
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to receive items from processed videos", e);
            }

            // Receive processed screenshots
            try
            {
                videoQueue.Tasks.AddRange(
                    (await _processedScreenshotRepository.ToListAsync(p => p.SourceFileId == videoQueue.FileId))
                        .Select(p => _mapper.Map<ProcessedScreenshotEntity, DomainProcessedScreenshot>(p)));
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to receive items from processed screenshots: {0}", e);
            }

            foreach (IProcessedEntity processedEntity in videoQueue.Tasks)
            {
                processedEntity.ProjectId = videoQueue.ProjectId;
                processedEntity.TaskId = Guid.NewGuid().ToString();
                processedEntity.Modified = DateTime.UtcNow;
            }
        }
    }
}