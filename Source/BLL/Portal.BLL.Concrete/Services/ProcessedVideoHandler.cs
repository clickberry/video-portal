// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Services;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.Domain.ProcessedVideoContext;
using Portal.Domain.ProjectContext;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    /// <summary>
    ///     Schedules video processing.
    /// </summary>
    public sealed class ProcessedVideoHandler : IProcessedVideoHandler
    {
        private const string BackendId = "00000000-0000-0000-0000-000000000003";

        private readonly IMapper _mapper;
        private readonly ITableRepository<ProcessedScreenshotEntity> _processedScreenshotRepository;
        private readonly ITableRepository<ProcessedVideoEntity> _processedVideoRepository;
        private readonly IService<DomainProjectProcessedScreenshot> _projectProcessedScreenshotService;
        private readonly Lazy<IService<DomainProjectProcessedVideo>> _projectProcessedVideoServiceLazy;
        private readonly ITableRepository<VideoQueueEntity> _videoQueueRepository;

        public ProcessedVideoHandler(
            IRepositoryFactory repositoryFactory,
            IService<DomainProjectProcessedScreenshot> projectProcessedScreenshotService,
            Lazy<IService<DomainProjectProcessedVideo>> projectProcessedVideoService,
            IMapper mapper)
        {
            _projectProcessedScreenshotService = projectProcessedScreenshotService;
            _projectProcessedVideoServiceLazy = projectProcessedVideoService;
            _mapper = mapper;
            _processedScreenshotRepository = repositoryFactory.Create<ProcessedScreenshotEntity>();
            _processedVideoRepository = repositoryFactory.Create<ProcessedVideoEntity>();
            _videoQueueRepository = repositoryFactory.Create<VideoQueueEntity>();
        }

        private IService<DomainProjectProcessedVideo> ProjectProcessedVideoService
        {
            get { return _projectProcessedVideoServiceLazy.Value; }
        }

        public async Task AddVideo(string projectId, string userId, DomainVideo entity, ProcessedMediaModel processedMediaModel)
        {
            List<DomainProcessedScreenshot> processedScreenshots = processedMediaModel.DomainProcessedScreenshots;
            List<DomainProcessedVideo> processedVideos = processedMediaModel.DomainProcessedVideos;

            // Fill entities data
            var entities = new List<IProcessedEntity>(processedScreenshots);
            entities.AddRange(processedVideos);

            foreach (IProcessedEntity processedEntity in entities)
            {
                processedEntity.UserId = BackendId;
                processedEntity.SourceFileId = entity.FileId;
            }

            // Add video processing entities
            await Task.WhenAll(
                new Task[]
                {
                    _processedScreenshotRepository.AddAsync(processedScreenshots.Select(p => _mapper.Map<DomainProcessedScreenshot, ProcessedScreenshotEntity>(p))),
                    _processedVideoRepository.AddAsync(processedVideos.Select(p => _mapper.Map<DomainProcessedVideo, ProcessedVideoEntity>(p))),
                    _videoQueueRepository.AddAsync(_mapper.Map<DomainVideoQueue, VideoQueueEntity>(
                        new DomainVideoQueue
                        {
                            ProjectId = projectId,
                            FileId = entity.FileId,
                            UserId = userId
                        }))
                });
        }

        public Task RemoveVideo(string projectId)
        {
            return Task.WhenAll(new[]
            {
                _projectProcessedScreenshotService.DeleteAsync(
                    new DomainProjectProcessedScreenshot
                    {
                        ProjectId = projectId
                    }),
                ProjectProcessedVideoService.DeleteAsync(
                    new DomainProjectProcessedVideo
                    {
                        ProjectId = projectId
                    })
            });
        }
    }
}