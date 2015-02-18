// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using AutoMapper;
using Portal.DAL.Entities.Storage;
using Portal.Domain.ProjectContext;

namespace Portal.Mappers.Mappings
{
    public sealed class ProjectVideoMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<DomainVideo, StorageFile>()
                .ConvertUsing(
                    p => new StorageFile
                    {
                        ContentType = p.ContentType,
                        Created = p.Created,
                        Id = p.FileId,
                        Length = p.FileLength,
                        FileName = p.FileName,
                        Modified = p.Modified
                    });

            Mapper.CreateMap<StorageFile, DomainVideo>()
                .ConvertUsing(
                    p => new DomainVideo
                    {
                        ContentType = p.ContentType,
                        Created = p.Created,
                        FileId = p.Id,
                        FileLength = p.Length,
                        FileName = p.FileName,
                        Modified = p.Modified
                    });
        }
    }
}