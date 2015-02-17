// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Portal.BLL.Concrete.Statistics.Filter.Filters.CicIPad;
using Portal.BLL.Concrete.Statistics.Filter.Filters.CicMac;
using Portal.BLL.Concrete.Statistics.Filter.Filters.CicPc;
using Portal.BLL.Concrete.Statistics.Filter.Filters.DailyMotion;
using Portal.BLL.Concrete.Statistics.Filter.Filters.ImageShack;
using Portal.BLL.Concrete.Statistics.Filter.Filters.JwPlayer;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Player;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Providers;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Registrations;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Standalone;
using Portal.BLL.Concrete.Statistics.Filter.Filters.TaggerAndroid;
using Portal.BLL.Concrete.Statistics.Filter.Filters.TaggerIPhone;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Views;
using Portal.BLL.Statistics.Filter;

namespace Portal.BLL.Concrete.Statistics.Filter
{
    public class FiltersFactory : IFiltersFactory
    {
        private readonly string _portalUrl;

        public FiltersFactory(string portalUrl)
        {
            _portalUrl = portalUrl;
        }

        public List<IStatWatchingFilter> CreateStatWatchingFilters()
        {
            return new List<IStatWatchingFilter>
            {
                new EmbeddedViewsFilter(_portalUrl),
                new TotalViewsFilter()
            };
        }

        public List<IStatUserRegistrationFilter> CreateStatUserRegistrationFilters()
        {
            return new List<IStatUserRegistrationFilter>
            {
                new AllRegistrationsFilter(),
                new CicIPadRegistrationsFilter(),
                new CicMacRegistrationsFilter(),
                new CicPcRegistrationsFilter(),
                new EmailRegistrationsFilter(),
                new FacebookRegistrationsFilter(),
                new GoogleRegistrationsFilter(),
                new ImageShackRegistrationsFilter(),
                new TaggerIPhoneRegistrationsFilter(),
                new WindowsLiveRegistrationsFilter(),
                new YahooRegistrationsFilter(),
                new TwitterRegistrationsFilter(),
                new TaggerAndroidRegistrationsFilter(),
                new StandaloneRegistrationsFilter(),
                new PlayerRegistrationsFilter(),
                new BrowserRegistrationsFilter(),
                new OtherRegistrationsFilter(),
                new DailyMotionRegistrationsFilter(),
                new VkRegistrationsFilter(),
                new JwPlayerRegistrationsFilter(),
                new OdnoklassnikiRegistrationsFilter()
            };
        }

        public List<IStatProjectUploadingFilter> CreateStatProjectUploadingFilters()
        {
            return new List<IStatProjectUploadingFilter>
            {
                new CicIPadUploadsFilter(),
                new CicMacUploadsFilter(),
                new CicPcUploadsFilter(),
                new ImageShackUploadsFilter(),
                new TaggerIPhoneUploadsFilter(),
                new TaggerAndroidUploadsFilter(),
                new StandaloneUploadsFilter(),
                new PlayerUploadsFilter(),
                new DailyMotionUploadsFilter(),
                new JwPlayerUploadsFilter()
            };
        }

        public List<IStatProjectDeletionFilter> CreateStatProjectDeletionFilters()
        {
            return new List<IStatProjectDeletionFilter>
            {
                new CicIPadDeletionsFilter(),
                new CicMacDeletionsFilter(),
                new CicPcDeletionsFilter(),
                new ImageShackDeletionsFilter(),
                new TaggerIPhoneDeletionsFilter(),
                new TaggerAndroidDeletionsFilter(),
                new StandaloneDeletionsFilter(),
                new PlayerDeletionsFilter(),
                new DailyMotionDeletionsFilter(),
                new JwPlayerDeletionsFilter()
            };
        }

        public List<IStatProjectCancellationFilter> CreateStatProjectCancellationFilters()
        {
            return new List<IStatProjectCancellationFilter>
            {
                new CicIPadCancellationFilter(),
                new CicMacCancellationFilter(),
                new CicPcCancellationFilter(),
                new ImageShackCancellationFilter(),
                new TaggerIPhoneCancellationFilter(),
                new TaggerAndroidCancellationFilter(),
                new StandaloneCancellationFilter(),
                new PlayerCancellationFilter(),
                new DailyMotionCancellationFilter(),
                new JwPlayerCancellationFilter()
            };
        }
    }
}