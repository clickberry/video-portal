// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Portal.DAL.Entities.Table;
using Portal.Domain.ProjectContext;
using Portal.Domain.StatisticContext;

namespace Portal.Mappers.Statistics
{
    public class StatEntityFactory : IStatEntityFactory
    {
        private readonly ITableValueConverter _tableValueConverter;

        public StatEntityFactory(ITableValueConverter tableValueConverter)
        {
            _tableValueConverter = tableValueConverter;
        }

        public StatProjectDeletionV2Entity CreateProjectDeletionEntity(string eventId, DateTime dateTime, DomainActionData domain, string projectId)
        {
            string productName = _tableValueConverter.UserAgentToProductName(domain.UserAgent);

            return new StatProjectDeletionV2Entity
            {
                Tick = _tableValueConverter.DateTimeToTickWithGuid(dateTime),
                EventId = eventId,
                DateTime = dateTime,
                ProjectId = projectId,
                ProductName = productName,
                UserId = domain.UserId
            };
        }

        public StatProjectUploadingV2Entity CreateProjectUploadingEntity(string eventId, DateTime dateTime, DomainActionData domain, string projectId, string projectName, ProjectType projectType,
            ProjectSubtype projectSubtype)
        {
            string productName = _tableValueConverter.UserAgentToProductName(domain.UserAgent);
            string productVersion = _tableValueConverter.UserAgentToVersion(domain.UserAgent);

            return new StatProjectUploadingV2Entity
            {
                Tick = _tableValueConverter.DateTimeToTickWithGuid(dateTime),
                EventId = eventId,
                DateTime = dateTime,
                ProductName = productName,
                UserId = domain.UserId,
                ProjectId = projectId,
                ProjectName = projectName,
                IdentityProvider = domain.IdentityProvider,
                ProductVersion = productVersion,
                ProjectType = (int)projectType,
                TagType = (int)projectSubtype
            };
        }

        public StatUserRegistrationV2Entity CreateUserRegistrationEntity(string eventId, DateTime dateTime, DomainActionData domain)
        {
            string productName = _tableValueConverter.UserAgentToProductName(domain.UserAgent);

            return new StatUserRegistrationV2Entity
            {
                Tick = _tableValueConverter.DateTimeToTickWithGuid(dateTime),
                EventId = eventId,
                DateTime = dateTime,
                Email = domain.UserEmail,
                UserName = domain.UserName,
                IdentityProvider = domain.IdentityProvider,
                ProductName = productName,
                UserId = domain.UserId
            };
        }

        public StatWatchingV2Entity CreateWatchingEntity(string eventId, DateTime dateTime, DomainActionData domain, string projectId)
        {
            return new StatWatchingV2Entity
            {
                Tick = _tableValueConverter.DateTimeToTickWithGuid(dateTime),
                EventId = eventId,
                DateTime = dateTime,
                IsAuthenticated = domain.IsAuthenticated,
                UrlReferrer = domain.UrlReferrer,
                UserId = domain.UserId,
                AnonymousId = domain.AnonymousId,
                ProjectId = projectId,
                UserAgent = domain.UserAgent
            };
        }

        public StatProjectStateV3Entity CreateProjectCreatingEntity(DateTime dateTime, DomainActionData domain, string projectId, string actionType)
        {
            return new StatProjectStateV3Entity
            {
                ProjectId = projectId,
                ActionType = actionType,
                DateTime = dateTime,
                Producer = _tableValueConverter.UserAgentToProductName(domain.UserAgent),
                Version = _tableValueConverter.UserAgentToVersion(domain.UserAgent)
            };
        }

        public StatUserLoginV2Entity CreateUserLoginEntity(DomainActionData domain)
        {
            string productName = _tableValueConverter.UserAgentToProductName(domain.UserAgent);

            return new StatUserLoginV2Entity
            {
                IdentityProvider = domain.IdentityProvider,
                ProductName = productName,
                UserId = domain.UserId
            };
        }
    }
}