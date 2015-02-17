// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Portal.BLL.Statistics.Filter;

namespace Portal.BLL.Concrete.Statistics.Filter
{
    public class FiltersChainBuilder : IFiltersChainBuilder
    {
        public IStatWatchingFilter BuildStatWatchingFilter(List<IStatWatchingFilter> statWatchingFilters)
        {
            for (int i = 0; i < statWatchingFilters.Count - 1; i++)
            {
                IStatWatchingFilter filter = statWatchingFilters[i];
                IStatWatchingFilter nextFilter = statWatchingFilters[i + 1];
                filter.Set(nextFilter);
            }

            return statWatchingFilters[0];
        }

        public IStatUserRegistrationFilter BuildStatUserRegistrationFilter(List<IStatUserRegistrationFilter> userRegistrationFilters)
        {
            for (int i = 0; i < userRegistrationFilters.Count - 1; i++)
            {
                IStatUserRegistrationFilter filter = userRegistrationFilters[i];
                IStatUserRegistrationFilter nextFilter = userRegistrationFilters[i + 1];
                filter.Set(nextFilter);
            }

            return userRegistrationFilters[0];
        }

        public IStatProjectUploadingFilter BuildStatProjectUploadingFilter(List<IStatProjectUploadingFilter> statProjectUploadingFilters)
        {
            for (int i = 0; i < statProjectUploadingFilters.Count - 1; i++)
            {
                IStatProjectUploadingFilter filter = statProjectUploadingFilters[i];
                IStatProjectUploadingFilter nextFilter = statProjectUploadingFilters[i + 1];
                filter.Set(nextFilter);
            }

            return statProjectUploadingFilters[0];
        }

        public IStatProjectDeletionFilter BuildStatProjectDeletionFilter(List<IStatProjectDeletionFilter> statProjectDeletionFilters)
        {
            for (int i = 0; i < statProjectDeletionFilters.Count - 1; i++)
            {
                IStatProjectDeletionFilter filter = statProjectDeletionFilters[i];
                IStatProjectDeletionFilter nextFilter = statProjectDeletionFilters[i + 1];
                filter.Set(nextFilter);
            }

            return statProjectDeletionFilters[0];
        }

        public IStatProjectCancellationFilter BuildStatProjectCancellationFilter(List<IStatProjectCancellationFilter> statProjectCancellationFilters)
        {
            for (int i = 0; i < statProjectCancellationFilters.Count - 1; i++)
            {
                IStatProjectCancellationFilter filter = statProjectCancellationFilters[i];
                IStatProjectCancellationFilter nextFilter = statProjectCancellationFilters[i + 1];
                filter.Set(nextFilter);
            }

            return statProjectCancellationFilters[0];
        }

        public IReportFilter BuildSuccessfulProjectUploadingsFilter(List<IReportFilter> reportFilters)
        {
            for (int i = 0; i < reportFilters.Count - 1; i++)
            {
                IReportFilter filter = reportFilters[i];
                IReportFilter nextFilter = reportFilters[i + 1];
                filter.Set(nextFilter);
            }

            return reportFilters[0];
        }
    }
}