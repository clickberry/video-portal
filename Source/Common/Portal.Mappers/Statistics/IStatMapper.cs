// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.DAL.Entities.Table;
using Portal.Domain.StatisticContext;

namespace Portal.Mappers.Statistics
{
    public interface IStatMapper
    {
        DomainStatWatching StatWatchingToDomain(StatWatchingV2Entity entity);
        DomainStatUserRegistration StatUserRegistrationToDomain(StatUserRegistrationV2Entity entity);
        DomainStatProjectUploading StatProjectUploadingToDomain(StatProjectUploadingV2Entity entity);
        DomainStatProjectDeletion StatProjectDeletionToDomain(StatProjectDeletionV2Entity entity);
        DomainStatProjectState StatProjectStateToDomain(StatProjectStateV3Entity entity, bool isSuccessfulUopload);
    }
}