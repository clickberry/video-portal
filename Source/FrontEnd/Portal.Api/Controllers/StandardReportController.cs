// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Portal.BLL.Statistics.Helper;
using Portal.BLL.Statistics.Reporter;
using Portal.DTO.Reports;
using Portal.Exceptions.CRUD;

namespace Portal.Api.Controllers
{
    [Route("standardreport")]
    public class StandardReportController : ApiControllerBase
    {
        private readonly IIntervalHelper _helper;
        private readonly IStandardReportBuilder _standardReportBuilder;

        public StandardReportController(IStandardReportBuilder standardReportBuilder, IIntervalHelper helper)
        {
            _standardReportBuilder = standardReportBuilder;
            _helper = helper;
        }

        public async Task<HttpResponseMessage> Get()
        {
            DateTime currentDateTime = DateTime.UtcNow;
            StandardReport standardReport;

            // Trying build report for one day before
            try
            {
                DateTime lastDay1 = _helper.GetLastDate(1, currentDateTime);
                standardReport = await _standardReportBuilder.BuildReport(lastDay1);

                return Request.CreateResponse(HttpStatusCode.OK, standardReport);
            }
            catch (NotFoundException)
            {
            }

            // Trying build report for two days before
            DateTime lastDay2 = _helper.GetLastDate(2, currentDateTime);
            standardReport = await _standardReportBuilder.BuildReport(lastDay2);

            return Request.CreateResponse(HttpStatusCode.OK, standardReport);
        }

        public HttpResponseMessage Get(DateTime start, DateTime end)
        {
            IEnumerable<Report> standardReports = _standardReportBuilder.BuildReports(start, end);
            return Request.CreateResponse(HttpStatusCode.OK, standardReports);
        }
    }
}