// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Portal.BLL.Statistics.Filter
{
    public interface IFiltersFactory
    {
        List<IStatWatchingFilter> CreateStatWatchingFilters();
        List<IStatUserRegistrationFilter> CreateStatUserRegistrationFilters();
        List<IStatProjectUploadingFilter> CreateStatProjectUploadingFilters();
        List<IStatProjectDeletionFilter> CreateStatProjectDeletionFilters();
        List<IStatProjectCancellationFilter> CreateStatProjectCancellationFilters();
    }
}