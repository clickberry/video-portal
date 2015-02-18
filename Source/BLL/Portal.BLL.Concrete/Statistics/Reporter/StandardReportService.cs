// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Statistics.Helper;
using Portal.BLL.Statistics.Reporter;
using Portal.DAL.Entities.QueryObject;
using Portal.DAL.Entities.Table;
using Portal.DAL.Statistics;
using Portal.Domain.StatisticContext;
using Portal.Exceptions.CRUD;
using Portal.Mappers.Statistics;

namespace Portal.BLL.Concrete.Statistics.Reporter
{
    public class StandardReportService : IStandardReportService
    {
        private readonly IReportMapper _reportMapper;
        private readonly IStandardReportRepository _repository;
        private readonly ITableValueConverter _tableValueConverter;

        public StandardReportService(IStandardReportRepository repository, ITableValueConverter tableValueConverter, IReportMapper reportMapper)
        {
            _repository = repository;
            _tableValueConverter = tableValueConverter;
            _reportMapper = reportMapper;
        }

        public async Task<List<DomainReport>> GetReports(DateTime dateTime)
        {
            string tick = _tableValueConverter.DateTimeToTick(dateTime);
            List<StandardReportV3Entity> entities = await _repository.ToListAsync(p => p.Tick == tick);
            List<DomainReport> domains = entities.Select(entity => _reportMapper.ReportEntityToDomain(entity)).ToList();

            if (domains.Count == 0)
            {
                throw new NotFoundException();
            }

            return domains;
        }

        public async Task WriteReports(List<DomainReport> domainReports, DateTime dateTime)
        {
            string tick = _tableValueConverter.DateTimeToTick(dateTime);
            IEnumerable<StandardReportV3Entity> entities = domainReports.Select(domain => _reportMapper.DomainReportToEntity(domain, tick));
            await _repository.AddAsync(entities);
        }

        public IEnumerable<DomainReport> GetDayReports(Interval interval)
        {
            string startTick = _tableValueConverter.DateTimeToTick(interval.Start);
            string finishTick = _tableValueConverter.DateTimeToTick(interval.Finish);

            var queryObject = new ReportQueryObject
            {
                StartInterval = startTick,
                EndInterval = finishTick,
                IsStartInclude = true,
                IsEndInclude = false,
                Interval = "1"
            };

            IEnumerable<DomainReport> result = _repository.GetReportEntities(queryObject)
                .Select(_reportMapper.ReportEntityToDomain);

            return result;
        }

        public async Task DeleteReports(List<DomainReport> domainReports, DateTime dateTime)
        {
            string tick = _tableValueConverter.DateTimeToTick(dateTime);
            IEnumerable<StandardReportV3Entity> entities = domainReports.Select(domain => _reportMapper.DomainReportToEntity(domain, tick));
            await _repository.DeleteAsync(entities);
        }

        public async Task<DomainReport> GetLastAllDaysReport()
        {
            var queryObject = new ReportQueryObject
            {
                Interval = "All"
            };

            StandardReportV3Entity entity = await _repository.GetLastReport(queryObject);
            DomainReport domain = entity == null ? new DomainReport() : _reportMapper.ReportEntityToDomain(entity);

            return domain;
        }
    }
}