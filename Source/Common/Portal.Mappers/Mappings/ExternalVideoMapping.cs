// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using Portal.DAL.Entities.Table;
using Portal.Domain.ProjectContext;

namespace Portal.Mappers.Mappings
{
    public sealed class ExternalVideoMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<ProjectEntity, DomainExternalVideo>()
                .ConvertUsing(entity => new DomainExternalVideo
                {
                    ProductName = entity.VideoSourceProductName,
                    VideoUri = entity.VideoSource
                });

            Mapper.CreateMap<DomainExternalVideo, ProjectEntity>()
                .ConvertUsing(video => new ProjectEntity
                {
                    VideoType = 1,
                    VideoSourceProductName = video.ProductName,
                    VideoSource = video.VideoUri
                });
        }
    }
}