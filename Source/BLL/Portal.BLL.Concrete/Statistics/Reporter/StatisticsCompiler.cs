// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using Portal.BLL.Statistics.Filter;
using Portal.BLL.Statistics.Helper;
using Portal.BLL.Statistics.Reporter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Concrete.Statistics.Reporter
{
    public class StatisticsCompiler : ICompiler
    {
        private readonly IFiltersManager _filtersManager;
        private readonly Interval _interval;
        private readonly IStatisticsService _statisticsService;

        public StatisticsCompiler(IFiltersManager filtersManager, IStatisticsService statisticsService, Interval interval)
        {
            _filtersManager = filtersManager;
            _statisticsService = statisticsService;
            _interval = interval;
        }

        public DomainReport CompileReport(DomainReport additionalReport = null)
        {
            var domainReport = new DomainReport { Interval = _interval.Days.ToString(CultureInfo.InvariantCulture) };
            IEnumerable<DomainStatWatching> watchings = _statisticsService.GetWatchings(_interval);
            IEnumerable<DomainStatUserRegistration> registrations = _statisticsService.GetUserRegistrations(_interval);
            IEnumerable<DomainStatProjectUploading> uploadings = _statisticsService.GetProjectUploadings(_interval);
            IEnumerable<DomainStatProjectDeletion> deletions = _statisticsService.GetProjectDeletions(_interval);

            foreach (DomainStatWatching domainStatWatching in watchings)
            {
                _filtersManager.FilterStatWatching(domainStatWatching, domainReport);
            }
            foreach (DomainStatUserRegistration domainStatUserRegistration in registrations)
            {
                _filtersManager.FilterStatUserRegistration(domainStatUserRegistration, domainReport);
            }
            foreach (DomainStatProjectUploading domainStatProjectUploading in uploadings)
            {
                DomainStatProjectState domainStatProjectState = _statisticsService.GetProjectState(domainStatProjectUploading.ProjectId);
                _filtersManager.FilterStatProjectUploading(domainStatProjectState, domainReport);
            }
            foreach (DomainStatProjectDeletion domainStatProjectDeletion in deletions)
            {
                DomainStatProjectState domainStatProjectState = _statisticsService.GetProjectState(domainStatProjectDeletion.ProjectId);
                _filtersManager.FilterStatProjectDeletion(domainStatProjectState, domainReport);
                _filtersManager.FilterStatProjectCancellation(domainStatProjectState, domainReport);
            }

            return domainReport;
        }
    }
}