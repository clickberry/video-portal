// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.Domain.StatisticContext;

namespace Portal.BLL.Statistics.Filter
{
    public interface IFiltersManager
    {
        void FilterStatWatching(DomainStatWatching domainStatWatching, DomainReport domainReport);
        void FilterStatUserRegistration(DomainStatUserRegistration domainUserRegistration, DomainReport domainReport);
        void FilterStatProjectUploading(DomainStatProjectState domainStatProjectState, DomainReport domainReport);
        void FilterStatProjectDeletion(DomainStatProjectState domainStatProjectState, DomainReport domainReport);
        void FilterStatProjectCancellation(DomainStatProjectState domainStatProjectState, DomainReport domainReport);
    }
}