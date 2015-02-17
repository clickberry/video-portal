// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using Portal.DAL.Entities.Table;
using Portal.Domain.Notifications;

namespace Portal.Mappers.Mappings
{
    public sealed class PushNotificationMappings : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<PushNotificationEntity, DomainNotification>()
                .ConvertUsing(
                    entity => new DomainNotification
                    {
                        Created = entity.Created,
                        Id = entity.Id,
                        ProjectId = entity.ProjectId,
                        Title = entity.Title,
                        UserId = entity.UserId
                    });

            Mapper.CreateMap<DomainNotification, PushNotificationEntity>()
                .ConvertUsing(
                    notification => new PushNotificationEntity
                    {
                        Created = notification.Created,
                        Id = notification.Id,
                        ProjectId = notification.ProjectId,
                        Title = notification.Title,
                        UserId = notification.UserId
                    });
        }
    }
}