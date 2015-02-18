// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Portal.BLL.Statistics.Filter;
using Portal.BLL.Statistics.Helper;
using Portal.BLL.Statistics.Reporter;

namespace Portal.BLL.Concrete.Statistics.Reporter
{
    public class CompilerFactory : ICompilerFactory
    {
        private readonly IFiltersManager _filtersManager;
        private readonly IReportAccumulator _reportAccumulator;
        private readonly IStandardReportService _standardReportService;
        private readonly IStatisticsService _statisticsService;

        public CompilerFactory(IFiltersManager filtersManager, IReportAccumulator reportAccumulator, IStatisticsService statisticsService, IStandardReportService standardReportService)
        {
            _filtersManager = filtersManager;
            _reportAccumulator = reportAccumulator;
            _statisticsService = statisticsService;
            _standardReportService = standardReportService;
        }

        public ICompiler Create(Interval interval)
        {
            if (interval == null)
            {
                return new AllDaysReportsCompiler(_reportAccumulator, _standardReportService);
            }
            if ((interval.Finish - interval.Start) > TimeSpan.FromDays(1))
            {
                return new ReportsCompiler(_reportAccumulator, _standardReportService, interval);
            }
            return new StatisticsCompiler(_filtersManager, _statisticsService, interval);
        }
    }
}