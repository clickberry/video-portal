// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.BLL.Statistics.Helper;
using Portal.DAL.Entities.Table;
using Portal.Domain.StatisticContext;
using Portal.DTO.Reports;

namespace Portal.Mappers.Statistics
{
    public interface IReportMapper
    {
        Report DomainReportToDto(DomainReport domain, Interval interval);
        DomainReport ReportEntityToDomain(StandardReportV3Entity entity);
        StandardReportV3Entity DomainReportToEntity(DomainReport domainReport, string tick);
    }
}