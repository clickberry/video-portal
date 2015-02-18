// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using Portal.BLL.Statistics.Helper;
using Portal.BLL.Statistics.Reporter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Concrete.Statistics.Reporter
{
    public class ReportsCompiler : ICompiler
    {
        private readonly Interval _interval;
        private readonly IReportAccumulator _reportAccumulator;
        private readonly IStandardReportService _standardReportService;

        public ReportsCompiler(IReportAccumulator reportAccumulator, IStandardReportService standardReportService, Interval interval)
        {
            _reportAccumulator = reportAccumulator;
            _standardReportService = standardReportService;
            _interval = interval;
        }

        public DomainReport CompileReport(DomainReport additionalReport = null)
        {
            var report = new DomainReport { Interval = _interval.Days.ToString(CultureInfo.InvariantCulture) };
            IEnumerable<DomainReport> reports = _standardReportService.GetDayReports(_interval);

            foreach (DomainReport domainReport in reports)
            {
                _reportAccumulator.Accumulate(domainReport, report);
            }

            if (additionalReport != null)
            {
                _reportAccumulator.Accumulate(additionalReport, report);
            }

            return report;
        }
    }
}