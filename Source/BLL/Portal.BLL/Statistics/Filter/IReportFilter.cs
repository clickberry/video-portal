// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.Domain.StatisticContext;

namespace Portal.BLL.Statistics.Filter
{
    public interface IReportFilter : IStatFilter<IReportFilter>
    {
        void Call(DomainReport domainReport);
    }
}