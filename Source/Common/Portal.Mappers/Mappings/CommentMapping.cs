// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using AutoMapper;
using Portal.DAL.Entities.Table;
using Portal.Domain.ProjectContext;
using Portal.DTO.Projects;

namespace Portal.Mappers.Mappings
{
    public sealed class CommentMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<Tuple<CommentEntity, UserEntity>, DomainComment>().ConvertUsing(tuple =>
                new DomainComment
                {
                    Id = tuple.Item1.Id,
                    ProjectId = tuple.Item1.ProjectId,
                    DateTime = tuple.Item1.DateTime,
                    Body = tuple.Item1.Body,
                    UserId = tuple.Item2 != null ? tuple.Item2.Id : null,
                    UserName = tuple.Item2 != null ? tuple.Item2.Name : null,
                    UserEmail = tuple.Item2 != null ? tuple.Item2.Email : null
                });

            Mapper.CreateMap<DomainComment, CommentEntity>().ConvertUsing(domain =>
                new CommentEntity
                {
                    Id = domain.Id,
                    ProjectId = domain.ProjectId,
                    UserId = domain.UserId,
                    Body = domain.Body,
                    DateTime = domain.DateTime
                });

            Mapper.CreateMap<Tuple<DomainComment, string>, Comment>().ConvertUsing(tuple =>
                new Comment
                {
                    Id = tuple.Item1.Id,
                    DateTime = tuple.Item1.DateTime,
                    Body = tuple.Item1.Body,
                    UserId = tuple.Item1.UserId,
                    UserName = tuple.Item1.UserName,
                    AvatarUrl = tuple.Item2
                });

            Mapper.CreateMap<DomainComment, Comment>()
                .ForMember(d => d.AvatarUrl, o => o.Ignore());
        }
    }
}