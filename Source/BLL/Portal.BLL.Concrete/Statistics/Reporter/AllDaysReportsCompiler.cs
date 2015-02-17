// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BLL.Statistics.Reporter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Concrete.Statistics.Reporter
{
    public class AllDaysReportsCompiler : ICompiler
    {
        private readonly IReportAccumulator _reportAccumulator;
        private readonly IStandardReportService _standardReportService;

        public AllDaysReportsCompiler(IReportAccumulator reportAccumulator, IStandardReportService standardReportService)
        {
            _reportAccumulator = reportAccumulator;
            _standardReportService = standardReportService;
        }

        public DomainReport CompileReport(DomainReport additionalReport = null)
        {
            DomainReport lastAllDaysReport = _standardReportService.GetLastAllDaysReport().Result;
            var domainReport = new DomainReport { Interval = "All" };

            _reportAccumulator.Accumulate(lastAllDaysReport, domainReport);
            _reportAccumulator.Accumulate(additionalReport, domainReport);

            return domainReport;
        }
    }
}