// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Statistics.Helper;
using Portal.BLL.Statistics.Reporter;
using Portal.Domain.StatisticContext;
using Portal.DTO.Reports;
using Portal.Mappers.Statistics;

namespace Portal.BLL.Concrete.Statistics.Reporter
{
    public class StandardReportBuilder : IStandardReportBuilder
    {
        private readonly IIntervalHelper _intervalHelper;
        private readonly IReportMapper _reportMapper;
        private readonly IStandardReportService _standardReportService;

        public StandardReportBuilder(IStandardReportService standardReportService, IReportMapper reportMapper, IIntervalHelper intervalHelper)
        {
            _standardReportService = standardReportService;
            _reportMapper = reportMapper;
            _intervalHelper = intervalHelper;
        }

        public async Task<StandardReport> BuildReport(DateTime dateTime)
        {
            List<DomainReport> reports = await _standardReportService.GetReports(dateTime);

            DomainReport dayDomainReport = reports.FirstOrDefault(p => p.Interval == "1");
            DomainReport weekDomainReport = reports.FirstOrDefault(p => p.Interval == "7");
            DomainReport monthDomainReport = reports.FirstOrDefault(p => p.Interval == "30");
            DomainReport allDaysDomainReport = reports.FirstOrDefault(p => p.Interval == "All");
            Interval dayInterval = _intervalHelper.GetLastDay(dateTime);
            Interval weekInterval = _intervalHelper.GetLastWeek(dateTime);
            Interval monthInterval = _intervalHelper.GetLastMonth(dateTime);
            Interval allInterval = _intervalHelper.GetAllDays(dateTime);

            Report dayReport = _reportMapper.DomainReportToDto(dayDomainReport, dayInterval);
            Report weekReport = _reportMapper.DomainReportToDto(weekDomainReport, weekInterval);
            Report monthReport = _reportMapper.DomainReportToDto(monthDomainReport, monthInterval);
            Report allDaysReport = _reportMapper.DomainReportToDto(allDaysDomainReport, allInterval);

            return new StandardReport
            {
                DateTime = dateTime,
                LastDay = dayReport,
                LastWeek = weekReport,
                LastMonth = monthReport,
                AllDays = allDaysReport
            };
        }

        public IEnumerable<Report> BuildReports(DateTime start, DateTime end)
        {
            Interval interval = _intervalHelper.GetInterval(start, end);

            IEnumerable<DomainReport> domainReports = _standardReportService.GetDayReports(interval);
            IEnumerable<Report> reoprts = domainReports.Select(r => _reportMapper.DomainReportToDto(r, _intervalHelper.GetInterval(r.Tick, r.Tick)));

            return reoprts;
        }
    }
}