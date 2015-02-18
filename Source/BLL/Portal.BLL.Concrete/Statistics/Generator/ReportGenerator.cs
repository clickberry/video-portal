// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.BLL.Statistics.Generator;
using Portal.BLL.Statistics.Reporter;
using Portal.Domain.StatisticContext;
using Portal.Exceptions.CRUD;

namespace Portal.BLL.Concrete.Statistics.Generator
{
    public class ReportGenerator : IReportGenerator
    {
        private readonly IReportBuilderService _reportBuilderService;
        private readonly IStandardReportService _standardReportService;

        public ReportGenerator(IStandardReportService standardReportService, IReportBuilderService reportBuilderService)
        {
            _reportBuilderService = reportBuilderService;
            _standardReportService = standardReportService;
        }

        public async Task Generate(DateTime dateTime)
        {
            List<DomainReport> domainReports = _reportBuilderService.BuildReports(dateTime);
            await _standardReportService.WriteReports(domainReports, dateTime);
        }

        public async Task GenerateIfNotExist(DateTime dateTime)
        {
            try
            {
                await _standardReportService.GetReports(dateTime);
                return;
            }
            catch (NotFoundException)
            {
            }

            List<DomainReport> domainReports = _reportBuilderService.BuildReports(dateTime);
            await _standardReportService.WriteReports(domainReports, dateTime);
        }
    }
}