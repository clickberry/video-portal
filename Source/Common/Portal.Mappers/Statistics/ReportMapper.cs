// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Portal.BLL.Statistics.Helper;
using Portal.DAL.Entities.Table;
using Portal.Domain.StatisticContext;
using Portal.DTO.Reports;

namespace Portal.Mappers.Statistics
{
    public class ReportMapper : IReportMapper
    {
        private readonly ITableValueConverter _tableValueConverter;

        public ReportMapper(ITableValueConverter tableValueConverter)
        {
            _tableValueConverter = tableValueConverter;
        }

        public Report DomainReportToDto(DomainReport domain, Interval interval)
        {
            return new Report
            {
                Interval = new ReportInterval
                {
                    Start = interval.Start,
                    End = new DateTime(interval.Finish.Ticks - 1)
                },
                AllRegistrations = domain.AllRegistrations,
                CicIPadDeletions = domain.CicIPadDeletions,
                CicIPadRegistrations = domain.CicIPadRegistrations,
                CicIPadSuccessfulUploads = domain.CicIPadSuccessfulUploads,
                CicIPadUploadCancels = domain.CicIPadUploadCancels,
                CicMacDeletions = domain.CicMacDeletions,
                CicMacRegistrations = domain.CicMacRegistrations,
                CicMacSuccessfulUploads = domain.CicMacSuccessfulUploads,
                CicMacUploadCancels = domain.CicMacUploadCancels,
                CicPcDeletions = domain.CicPcDeletions,
                CicPcRegistrations = domain.CicPcRegistrations,
                CicPcSuccessfulUploads = domain.CicPcSuccessfulUploads,
                CicPcUploadCancels = domain.CicPcUploadCancels,
                EmailRegistrations = domain.EmailRegistrations,
                EmbeddedViews = domain.EmbeddedViews,
                FacebookRegistrations = domain.FacebookRegistrations,
                GoogleRegistrations = domain.GoogleRegistrations,
                TaggerIPhoneDeletions = domain.TaggerIPhoneDeletions,
                TaggerIPhoneRegistrations = domain.TaggerIPhoneRegistrations,
                TaggerIPhoneSuccessfulUploads = domain.TaggerIPhoneUploads,
                TaggerIPhoneUploadCancels = domain.TaggerIPhoneUploadCancels,
                TotalViews = domain.TotalViews,
                WindowsLiveRegistrations = domain.WindowsLiveRegistrations,
                YahooRegistrations = domain.YahooRegistrations,
                ImageShackDeletions = domain.ImageShackDeletions,
                ImageShackRegistrations = domain.ImageShackRegistrations,
                ImageShackSuccessfulUploads = domain.ImageShackSuccessfulUploads,
                ImageShackUploadCancels = domain.ImageShackUploadCancels,
                TwitterRegistrations = domain.TwitterRegistrations,
                OdnoklassnikiRegistrations = domain.OdnoklassnikiRegistrations,
                BrowserRegistrations = domain.BrowserRegistrations,
                OtherRegistrations = domain.OtherRegistrations,
                PlayerDeletions = domain.PlayerDeletions,
                PlayerRegistrations = domain.PlayerRegistrations,
                PlayerSuccessfulUploads = domain.PlayerSuccessfulUploads,
                PlayerUploadCancels = domain.PlayerUploadCancels,
                StandaloneDeletions = domain.StandaloneDeletions,
                StandaloneRegistrations = domain.StandaloneRegistrations,
                StandaloneSuccessfulUploads = domain.StandaloneSuccessfulUploads,
                StandaloneUploadCancels = domain.StandaloneUploadCancels,
                TaggerAndroidDeletions = domain.TaggerAndroidDeletions,
                TaggerAndroidRegistrations = domain.TaggerAndroidRegistrations,
                TaggerAndroidSuccessfulUploads = domain.TaggerAndroidSuccessfulUploads,
                TaggerAndroidUploadCancels = domain.TaggerAndroidUploadCancels,
                DailyMotionDeletions = domain.DailyMotionDeletions,
                DailyMotionRegistrations = domain.DailyMotionRegistrations,
                DailyMotionSuccessfulUploads = domain.DailyMotionSuccessfulUploads,
                DailyMotionUploadCancels = domain.DailyMotionUploadCancels,
                VkRegistrations = domain.VkRegistrations,
                JwPlayerDeletions = domain.JwPlayerDeletions,
                JwPlayerRegistrations = domain.JwPlayerRegistrations,
                JwPlayerSuccessfulUpload = domain.JwPlayerSuccessfulUploads,
                JwPlayerUploadCancels = domain.JwPlayerUploadCancels
            };
        }

        public DomainReport ReportEntityToDomain(StandardReportV3Entity entity)
        {
            return new DomainReport
            {
                Tick = _tableValueConverter.TickToDateTime(entity.Tick),
                Interval = entity.Interval,
                AllRegistrations = entity.AllRegistrations,
                CicIPadDeletions = entity.CicIPadDeletions,
                CicIPadRegistrations = entity.CicIPadRegistrations,
                CicIPadSuccessfulUploads = entity.CicIPadSuccessfulUploads,
                CicIPadUploadCancels = entity.CicIPadUploadCancels,
                CicMacDeletions = entity.CicMacDeletions,
                CicMacRegistrations = entity.CicMacRegistrations,
                CicMacSuccessfulUploads = entity.CicMacSuccessfulUploads,
                CicMacUploadCancels = entity.CicMacUploadCancels,
                CicPcDeletions = entity.CicPcDeletions,
                CicPcRegistrations = entity.CicPcRegistrations,
                CicPcSuccessfulUploads = entity.CicPcSuccessfulUploads,
                CicPcUploadCancels = entity.CicPcUploadCancels,
                EmailRegistrations = entity.EmailRegistrations,
                EmbeddedViews = entity.EmbeddedViews,
                FacebookRegistrations = entity.FacebookRegistrations,
                GoogleRegistrations = entity.GoogleRegistrations,
                TaggerIPhoneDeletions = entity.TaggerIPhoneDeletions,
                TaggerIPhoneRegistrations = entity.TaggerIPhoneRegistrations,
                TaggerIPhoneUploads = entity.TaggerIPhoneSuccessfulUploads,
                TaggerIPhoneUploadCancels = entity.TaggerIPhoneUploadCancels,
                TotalViews = entity.TotalViews,
                WindowsLiveRegistrations = entity.WindowsLiveRegistrations,
                YahooRegistrations = entity.YahooRegistrations,
                ImageShackDeletions = entity.ImageShackDeletions,
                ImageShackRegistrations = entity.ImageShackRegistrations,
                ImageShackSuccessfulUploads = entity.ImageShackSuccessfulUploads,
                ImageShackUploadCancels = entity.ImageShackUploadCancels,
                TwitterRegistrations = entity.TwitterRegistrations,
                OdnoklassnikiRegistrations = entity.OdnoklassnikiRegistrations,
                BrowserRegistrations = entity.BrowserRegistrations,
                OtherRegistrations = entity.OtherRegistrations,
                PlayerDeletions = entity.PlayerDeletions,
                PlayerRegistrations = entity.PlayerRegistrations,
                PlayerSuccessfulUploads = entity.PlayerSuccessfulUploads,
                PlayerUploadCancels = entity.PlayerUploadCancels,
                StandaloneDeletions = entity.StandaloneDeletions,
                StandaloneRegistrations = entity.StandaloneRegistrations,
                StandaloneSuccessfulUploads = entity.StandaloneSuccessfulUploads,
                StandaloneUploadCancels = entity.StandaloneUploadCancels,
                TaggerAndroidDeletions = entity.TaggerAndroidDeletions,
                TaggerAndroidRegistrations = entity.TaggerAndroidRegistrations,
                TaggerAndroidSuccessfulUploads = entity.TaggerAndroidSuccessfulUploads,
                TaggerAndroidUploadCancels = entity.TaggerAndroidUploadCancels,
                DailyMotionDeletions = entity.DailyMotionDeletions,
                DailyMotionRegistrations = entity.DailyMotionRegistrations,
                DailyMotionSuccessfulUploads = entity.DailyMotionSuccessfulUploads,
                DailyMotionUploadCancels = entity.DailyMotionUploadCancels,
                VkRegistrations = entity.VkRegistrations,
                JwPlayerDeletions = entity.JwPlayerDeletions,
                JwPlayerRegistrations = entity.JwPlayerRegistrations,
                JwPlayerSuccessfulUploads = entity.JwPlayerSuccessfulUploads,
                JwPlayerUploadCancels = entity.JwPlayerUploadCancels
            };
        }

        public StandardReportV3Entity DomainReportToEntity(DomainReport domainReport, string tick)
        {
            return new StandardReportV3Entity
            {
                Tick = tick,
                Interval = domainReport.Interval,
                AllRegistrations = domainReport.AllRegistrations,
                CicIPadDeletions = domainReport.CicIPadDeletions,
                CicIPadRegistrations = domainReport.CicIPadRegistrations,
                CicIPadSuccessfulUploads = domainReport.CicIPadSuccessfulUploads,
                CicIPadUploadCancels = domainReport.CicIPadUploadCancels,
                CicMacDeletions = domainReport.CicMacDeletions,
                CicMacRegistrations = domainReport.CicMacRegistrations,
                CicMacSuccessfulUploads = domainReport.CicMacSuccessfulUploads,
                CicMacUploadCancels = domainReport.CicMacUploadCancels,
                CicPcDeletions = domainReport.CicPcDeletions,
                CicPcRegistrations = domainReport.CicPcRegistrations,
                CicPcSuccessfulUploads = domainReport.CicPcSuccessfulUploads,
                CicPcUploadCancels = domainReport.CicPcUploadCancels,
                EmailRegistrations = domainReport.EmailRegistrations,
                EmbeddedViews = domainReport.EmbeddedViews,
                FacebookRegistrations = domainReport.FacebookRegistrations,
                GoogleRegistrations = domainReport.GoogleRegistrations,
                TaggerIPhoneDeletions = domainReport.TaggerIPhoneDeletions,
                TaggerIPhoneRegistrations = domainReport.TaggerIPhoneRegistrations,
                TaggerIPhoneSuccessfulUploads = domainReport.TaggerIPhoneUploads,
                TaggerIPhoneUploadCancels = domainReport.TaggerIPhoneUploadCancels,
                TotalViews = domainReport.TotalViews,
                WindowsLiveRegistrations = domainReport.WindowsLiveRegistrations,
                YahooRegistrations = domainReport.YahooRegistrations,
                ImageShackDeletions = domainReport.ImageShackDeletions,
                ImageShackRegistrations = domainReport.ImageShackRegistrations,
                ImageShackSuccessfulUploads = domainReport.ImageShackSuccessfulUploads,
                ImageShackUploadCancels = domainReport.ImageShackUploadCancels,
                TwitterRegistrations = domainReport.TwitterRegistrations,
                OdnoklassnikiRegistrations = domainReport.OdnoklassnikiRegistrations,
                BrowserRegistrations = domainReport.BrowserRegistrations,
                OtherRegistrations = domainReport.OtherRegistrations,
                PlayerDeletions = domainReport.PlayerDeletions,
                PlayerRegistrations = domainReport.PlayerRegistrations,
                PlayerSuccessfulUploads = domainReport.PlayerSuccessfulUploads,
                PlayerUploadCancels = domainReport.PlayerUploadCancels,
                StandaloneDeletions = domainReport.StandaloneDeletions,
                StandaloneRegistrations = domainReport.StandaloneRegistrations,
                StandaloneSuccessfulUploads = domainReport.StandaloneSuccessfulUploads,
                StandaloneUploadCancels = domainReport.StandaloneUploadCancels,
                TaggerAndroidDeletions = domainReport.TaggerAndroidDeletions,
                TaggerAndroidRegistrations = domainReport.TaggerAndroidRegistrations,
                TaggerAndroidSuccessfulUploads = domainReport.TaggerAndroidSuccessfulUploads,
                TaggerAndroidUploadCancels = domainReport.TaggerAndroidUploadCancels,
                DailyMotionDeletions = domainReport.DailyMotionDeletions,
                DailyMotionRegistrations = domainReport.DailyMotionRegistrations,
                DailyMotionSuccessfulUploads = domainReport.DailyMotionSuccessfulUploads,
                DailyMotionUploadCancels = domainReport.DailyMotionUploadCancels,
                VkRegistrations = domainReport.VkRegistrations,
                JwPlayerDeletions = domainReport.JwPlayerDeletions,
                JwPlayerRegistrations = domainReport.JwPlayerRegistrations,
                JwPlayerSuccessfulUploads = domainReport.JwPlayerSuccessfulUploads,
                JwPlayerUploadCancels = domainReport.JwPlayerUploadCancels
            };
        }
    }
}