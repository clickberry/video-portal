// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.BLL.Statistics.Helper;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Statistics.Reporter
{
    public interface IStandardReportService
    {
        Task<List<DomainReport>> GetReports(DateTime dateTime);
        Task WriteReports(List<DomainReport> domainReports, DateTime dateTime);
        IEnumerable<DomainReport> GetDayReports(Interval interval);
        Task DeleteReports(List<DomainReport> domainReports, DateTime dateTime);
        Task<DomainReport> GetLastAllDaysReport();
    }
}