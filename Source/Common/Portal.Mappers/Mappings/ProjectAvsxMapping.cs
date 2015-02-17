// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using Portal.DAL.Entities.Storage;
using Portal.Domain.ProjectContext;

namespace Portal.Mappers.Mappings
{
    public sealed class ProjectAvsxMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<StorageFile, DomainAvsx>()
                .ConvertUsing(
                    f => new DomainAvsx
                    {
                        Created = f.Created,
                        FileId = f.Id,
                        FileLength = f.Length,
                        FileName = f.FileName,
                        Modified = f.Modified,
                        ContentType = f.ContentType,
                    });

            Mapper.CreateMap<DomainAvsx, StorageFile>()
                .ConvertUsing(
                    avsx => new StorageFile
                    {
                        Created = avsx.Created,
                        Id = avsx.FileId,
                        Length = avsx.FileLength,
                        FileName = avsx.FileName,
                        Modified = avsx.Modified,
                        ContentType = avsx.ContentType,
                    });
        }
    }
}