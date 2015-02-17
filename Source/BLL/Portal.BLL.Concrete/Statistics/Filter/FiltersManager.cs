// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Concrete.Statistics.Filter
{
    public class FiltersManager : IFiltersManager
    {
        private readonly IStatProjectCancellationFilter _statProjectCancellationFilter;
        private readonly IStatProjectDeletionFilter _statProjectDeletionFilter;
        private readonly IStatProjectUploadingFilter _statProjectUploadingFilter;
        private readonly IStatUserRegistrationFilter _statUserRegistrationFilter;
        private readonly IStatWatchingFilter _statWatchingFilter;

        public FiltersManager(IFiltersFactory filtersFactory, IFiltersChainBuilder filtersChainBuilder)
        {
            List<IStatWatchingFilter> statWatchingFilters = filtersFactory.CreateStatWatchingFilters();
            List<IStatUserRegistrationFilter> statUserRegistrationFilters = filtersFactory.CreateStatUserRegistrationFilters();
            List<IStatProjectUploadingFilter> statProjectUploadingFilters = filtersFactory.CreateStatProjectUploadingFilters();
            List<IStatProjectDeletionFilter> statProjectDeletionFilters = filtersFactory.CreateStatProjectDeletionFilters();
            List<IStatProjectCancellationFilter> statProjectCancellationFilters = filtersFactory.CreateStatProjectCancellationFilters();

            _statWatchingFilter = filtersChainBuilder.BuildStatWatchingFilter(statWatchingFilters);
            _statUserRegistrationFilter = filtersChainBuilder.BuildStatUserRegistrationFilter(statUserRegistrationFilters);
            _statProjectUploadingFilter = filtersChainBuilder.BuildStatProjectUploadingFilter(statProjectUploadingFilters);
            _statProjectDeletionFilter = filtersChainBuilder.BuildStatProjectDeletionFilter(statProjectDeletionFilters);
            _statProjectCancellationFilter = filtersChainBuilder.BuildStatProjectCancellationFilter(statProjectCancellationFilters);
        }

        public void FilterStatWatching(DomainStatWatching domainStatWatching, DomainReport domainReport)
        {
            _statWatchingFilter.Call(domainStatWatching, domainReport);
        }

        public void FilterStatUserRegistration(DomainStatUserRegistration domainUserRegistration, DomainReport domainReport)
        {
            _statUserRegistrationFilter.Call(domainUserRegistration, domainReport);
        }

        public void FilterStatProjectUploading(DomainStatProjectState domainStatProjectState, DomainReport domainReport)
        {
            _statProjectUploadingFilter.Call(domainStatProjectState, domainReport);
        }

        public void FilterStatProjectDeletion(DomainStatProjectState domainStatProjectState, DomainReport domainReport)
        {
            _statProjectDeletionFilter.Call(domainStatProjectState, domainReport);
        }

        public void FilterStatProjectCancellation(DomainStatProjectState domainStatProjectState, DomainReport domainReport)
        {
            _statProjectCancellationFilter.Call(domainStatProjectState, domainReport);
        }
    }
}