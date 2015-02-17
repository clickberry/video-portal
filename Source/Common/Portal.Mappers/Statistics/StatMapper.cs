// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.DAL.Entities.Table;
using Portal.Domain.StatisticContext;

namespace Portal.Mappers.Statistics
{
    public class StatMapper : IStatMapper
    {
        public DomainStatWatching StatWatchingToDomain(StatWatchingV2Entity entity)
        {
            return new DomainStatWatching
            {
                AnonymousId = entity.AnonymousId,
                DateTime = entity.DateTime,
                ProjectId = entity.ProjectId,
                Tick = entity.Tick,
                UrlReferrer = entity.UrlReferrer,
                UserId = entity.UserId,
                IsAuthenticated = entity.IsAuthenticated,
                EventId = entity.EventId,
                UserAgent = entity.UserAgent
            };
        }

        public DomainStatUserRegistration StatUserRegistrationToDomain(StatUserRegistrationV2Entity entity)
        {
            return new DomainStatUserRegistration
            {
                DateTime = entity.DateTime,
                Tick = entity.Tick,
                UserId = entity.UserId,
                EventId = entity.EventId,
                IdentityProvider = entity.IdentityProvider,
                ProductName = entity.ProductName
            };
        }

        public DomainStatProjectUploading StatProjectUploadingToDomain(StatProjectUploadingV2Entity entity)
        {
            return new DomainStatProjectUploading
            {
                DateTime = entity.DateTime,
                Tick = entity.Tick,
                UserId = entity.UserId,
                EventId = entity.EventId,
                ProductName = entity.ProductName,
                ProjectId = entity.ProjectId
            };
        }

        public DomainStatProjectDeletion StatProjectDeletionToDomain(StatProjectDeletionV2Entity entity)
        {
            return new DomainStatProjectDeletion
            {
                DateTime = entity.DateTime,
                Tick = entity.Tick,
                UserId = entity.UserId,
                EventId = entity.EventId,
                ProductName = entity.ProductName,
                ProjectId = entity.ProjectId
            };
        }

        public DomainStatProjectState StatProjectStateToDomain(StatProjectStateV3Entity entity, bool isSuccessfulUopload)
        {
            return new DomainStatProjectState
            {
                ProjectId = entity.ProjectId,
                DateTime = entity.DateTime,
                Producer = entity.Producer,
                Version = entity.Version,
                IsSuccessfulUpload = isSuccessfulUopload
            };
        }
    }
}