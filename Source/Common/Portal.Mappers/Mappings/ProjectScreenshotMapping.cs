// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using Portal.DAL.Entities.Storage;
using Portal.Domain.ProjectContext;

namespace Portal.Mappers.Mappings
{
    public sealed class ProjectScreenshotMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<StorageFile, DomainScreenshot>()
                .ConvertUsing(
                    f => new DomainScreenshot
                    {
                        Created = f.Created,
                        FileId = f.Id,
                        FileLength = f.Length,
                        FileName = f.FileName,
                        Modified = f.Modified,
                        ContentType = f.ContentType,
                    });

            Mapper.CreateMap<DomainScreenshot, StorageFile>()
                .ConvertUsing(
                    f => new StorageFile
                    {
                        Created = f.Created,
                        Id = f.FileId,
                        Length = f.FileLength,
                        FileName = f.FileName,
                        Modified = f.Modified,
                        ContentType = f.ContentType,
                    });
        }
    }
}