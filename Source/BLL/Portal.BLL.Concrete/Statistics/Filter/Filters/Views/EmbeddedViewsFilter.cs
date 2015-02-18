// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Concrete.Statistics.Filter.Filters.Views
{
    public class EmbeddedViewsFilter : StatFilterBase<IStatWatchingFilter>, IStatWatchingFilter
    {
        private readonly string _portalUrl;

        public EmbeddedViewsFilter(string portalUrl)
        {
            _portalUrl = portalUrl;
        }

        public void Call(DomainStatWatching domainStatWatching, DomainReport domainReport)
        {
            if (!string.IsNullOrEmpty(domainStatWatching.UrlReferrer) && !new Uri(domainStatWatching.UrlReferrer).Host.Equals(_portalUrl, StringComparison.OrdinalIgnoreCase))
            {
                domainReport.EmbeddedViews++;
            }
            if (Filter != null)
            {
                Filter.Call(domainStatWatching, domainReport);
            }
        }
    }
}