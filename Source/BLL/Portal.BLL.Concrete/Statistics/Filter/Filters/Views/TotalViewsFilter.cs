// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text.RegularExpressions;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Concrete.Statistics.Filter.Filters.Views
{
    public class TotalViewsFilter : StatFilterBase<IStatWatchingFilter>, IStatWatchingFilter
    {
        private static readonly Regex RegularClient = new Regex(
            @"(mozilla(?!.*https?://).*)|(opera)|(tagger)|(jw\+player\+extension)|(lynx)|(samsung)|(ucbrowser)|(tsm.*browser)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public void Call(DomainStatWatching domainStatWatching, DomainReport domainReport)
        {
            if (!String.IsNullOrEmpty(domainStatWatching.UserAgent) && RegularClient.IsMatch(domainStatWatching.UserAgent))
            {
                domainReport.TotalViews++;
            }
            if (Filter != null)
            {
                Filter.Call(domainStatWatching, domainReport);
            }
        }
    }
}