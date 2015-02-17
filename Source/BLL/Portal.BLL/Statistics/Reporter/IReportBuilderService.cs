// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Statistics.Reporter
{
    public interface IReportBuilderService
    {
        List<DomainReport> BuildReports(DateTime dateTime);
    }
}