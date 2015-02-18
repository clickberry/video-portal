// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Portal.BLL.Statistics.Filter
{
    public interface IFiltersChainBuilder
    {
        IStatWatchingFilter BuildStatWatchingFilter(List<IStatWatchingFilter> statWatchingFilters);
        IStatUserRegistrationFilter BuildStatUserRegistrationFilter(List<IStatUserRegistrationFilter> userRegistrationFilters);
        IStatProjectUploadingFilter BuildStatProjectUploadingFilter(List<IStatProjectUploadingFilter> statProjectUploadingFilters);
        IStatProjectDeletionFilter BuildStatProjectDeletionFilter(List<IStatProjectDeletionFilter> statProjectDeletionFilters);
        IStatProjectCancellationFilter BuildStatProjectCancellationFilter(List<IStatProjectCancellationFilter> statProjectCancellationFilters);
        IReportFilter BuildSuccessfulProjectUploadingsFilter(List<IReportFilter> reportFilters);
    }
}