// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Portal.BLL.Statistics.Helper;
using Portal.BLL.Statistics.Reporter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Concrete.Statistics.Reporter
{
    public class ReportBuilderService : IReportBuilderService
    {
        private readonly ICompilerFactory _compilerFactory;
        private readonly IIntervalHelper _intervalHelper;

        public ReportBuilderService(ICompilerFactory compilerFactory, IIntervalHelper intervalHelper)
        {
            _compilerFactory = compilerFactory;
            _intervalHelper = intervalHelper;
        }

        public List<DomainReport> BuildReports(DateTime dateTime)
        {
            Interval dayInterval = _intervalHelper.GetLastDay(dateTime);
            Interval weekInterval = _intervalHelper.GetLastWeek(dateTime);
            Interval monthInterval = _intervalHelper.GetLastMonth(dateTime);

            ICompiler dayReportCompiler = _compilerFactory.Create(dayInterval);
            ICompiler weekReportCompiler = _compilerFactory.Create(weekInterval);
            ICompiler monthReportCompiler = _compilerFactory.Create(monthInterval);
            ICompiler allDaysReortCompiler = _compilerFactory.Create(null);

            DomainReport dayReport = dayReportCompiler.CompileReport();
            DomainReport weekReport = weekReportCompiler.CompileReport(dayReport);
            DomainReport monthReport = monthReportCompiler.CompileReport(dayReport);
            DomainReport allDaysReport = allDaysReortCompiler.CompileReport(dayReport);

            var reports = new List<DomainReport>
            {
                dayReport,
                weekReport,
                monthReport,
                allDaysReport
            };

            return reports;
        }
    }
}