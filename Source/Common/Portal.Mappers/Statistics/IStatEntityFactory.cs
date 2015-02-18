// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Portal.DAL.Entities.Table;
using Portal.Domain.ProjectContext;
using Portal.Domain.StatisticContext;

namespace Portal.Mappers.Statistics
{
    public interface IStatEntityFactory
    {
        StatProjectDeletionV2Entity CreateProjectDeletionEntity(string eventId, DateTime dateTime, DomainActionData domain, string projectId);

        StatProjectUploadingV2Entity CreateProjectUploadingEntity(string eventId, DateTime dateTime, DomainActionData domain, string projectId, string projectName, ProjectType projectType,
            ProjectSubtype projectSubtype);

        StatUserRegistrationV2Entity CreateUserRegistrationEntity(string eventId, DateTime dateTime, DomainActionData domain);
        StatWatchingV2Entity CreateWatchingEntity(string eventId, DateTime dateTime, DomainActionData domain, string projectId);
        StatProjectStateV3Entity CreateProjectCreatingEntity(DateTime dateTime, DomainActionData domain, string projectId, string actionType);
        StatUserLoginV2Entity CreateUserLoginEntity(DomainActionData domain);
    }
}