// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.DAL.Context;
using Portal.DAL.Entities.QueryObject;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Statistics
{
    public interface IStandardReportRepository : ITableRepository<StandardReportV3Entity>
    {
        IEnumerable<StandardReportV3Entity> GetReportEntities(ReportQueryObject queryObject);

        Task<StandardReportV3Entity> GetLastReport(ReportQueryObject queryObject);
    }
}