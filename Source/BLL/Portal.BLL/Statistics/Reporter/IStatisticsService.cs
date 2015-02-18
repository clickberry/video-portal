// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using Portal.BLL.Statistics.Helper;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Statistics.Reporter
{
    public interface IStatisticsService
    {
        IEnumerable<DomainStatWatching> GetWatchings(Interval interval);
        IEnumerable<DomainStatUserRegistration> GetUserRegistrations(Interval interval);
        IEnumerable<DomainStatProjectUploading> GetProjectUploadings(Interval interval);
        IEnumerable<DomainStatProjectDeletion> GetProjectDeletions(Interval interval);
        DomainStatProjectState GetProjectState(string projectId);
    }
}