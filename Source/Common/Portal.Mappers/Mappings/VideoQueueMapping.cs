// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using AutoMapper;
using Portal.DAL.Entities.Table;
using Portal.Domain.ProcessedVideoContext;

namespace Portal.Mappers.Mappings
{
    public sealed class VideoQueueMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<VideoQueueEntity, DomainVideoQueue>()
                .ConvertUsing(p => new DomainVideoQueue
                {
                    ObjectId = p.Id,
                    ProjectId = p.ProjectId,
                    FileId = p.FileId,
                    UserId = p.UserId
                });

            Mapper.CreateMap<DomainVideoQueue, VideoQueueEntity>()
                .ConvertUsing(p => new VideoQueueEntity
                {
                    Id = p.ObjectId,
                    ProjectId = p.ProjectId,
                    FileId = p.FileId,
                    UserId = p.UserId
                });
        }
    }
}