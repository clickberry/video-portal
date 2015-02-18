// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.BLL.Statistics.Reporter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Concrete.Statistics.Reporter
{
    public class ReportAccumulator : IReportAccumulator
    {
        public void Accumulate(DomainReport dayReport, DomainReport accumulator)
        {
            accumulator.AllRegistrations += dayReport.AllRegistrations;
            accumulator.CicIPadDeletions += dayReport.CicIPadDeletions;
            accumulator.CicIPadRegistrations += dayReport.CicIPadRegistrations;
            accumulator.CicIPadSuccessfulUploads += dayReport.CicIPadSuccessfulUploads;
            accumulator.CicIPadUploadCancels += dayReport.CicIPadUploadCancels;
            accumulator.CicMacDeletions += dayReport.CicMacDeletions;
            accumulator.CicMacRegistrations += dayReport.CicMacRegistrations;
            accumulator.CicMacSuccessfulUploads += dayReport.CicMacSuccessfulUploads;
            accumulator.CicMacUploadCancels += dayReport.CicMacUploadCancels;
            accumulator.CicPcDeletions += dayReport.CicPcDeletions;
            accumulator.CicPcRegistrations += dayReport.CicPcRegistrations;
            accumulator.CicPcSuccessfulUploads += dayReport.CicPcSuccessfulUploads;
            accumulator.CicPcUploadCancels += dayReport.CicPcUploadCancels;
            accumulator.EmailRegistrations += dayReport.EmailRegistrations;
            accumulator.EmbeddedViews += dayReport.EmbeddedViews;
            accumulator.FacebookRegistrations += dayReport.FacebookRegistrations;
            accumulator.GoogleRegistrations += dayReport.GoogleRegistrations;
            accumulator.ImageShackDeletions += dayReport.ImageShackDeletions;
            accumulator.ImageShackRegistrations += dayReport.ImageShackRegistrations;
            accumulator.ImageShackSuccessfulUploads += dayReport.ImageShackSuccessfulUploads;
            accumulator.ImageShackUploadCancels += dayReport.ImageShackUploadCancels;
            accumulator.TaggerIPhoneDeletions += dayReport.TaggerIPhoneDeletions;
            accumulator.TaggerIPhoneRegistrations += dayReport.TaggerIPhoneRegistrations;
            accumulator.TaggerIPhoneUploads += dayReport.TaggerIPhoneUploads;
            accumulator.TaggerIPhoneUploadCancels += dayReport.TaggerIPhoneUploadCancels;
            accumulator.TotalViews += dayReport.TotalViews;
            accumulator.WindowsLiveRegistrations += dayReport.WindowsLiveRegistrations;
            accumulator.YahooRegistrations += dayReport.YahooRegistrations;
            accumulator.TwitterRegistrations += dayReport.TwitterRegistrations;
            accumulator.OdnoklassnikiRegistrations += dayReport.OdnoklassnikiRegistrations;
            accumulator.BrowserRegistrations += dayReport.BrowserRegistrations;
            accumulator.OtherRegistrations += dayReport.OtherRegistrations;
            accumulator.PlayerDeletions += dayReport.PlayerDeletions;
            accumulator.PlayerRegistrations += dayReport.PlayerRegistrations;
            accumulator.PlayerSuccessfulUploads += dayReport.PlayerSuccessfulUploads;
            accumulator.PlayerUploadCancels += dayReport.PlayerUploadCancels;
            accumulator.StandaloneDeletions += dayReport.StandaloneDeletions;
            accumulator.StandaloneRegistrations += dayReport.StandaloneRegistrations;
            accumulator.StandaloneSuccessfulUploads += dayReport.StandaloneSuccessfulUploads;
            accumulator.StandaloneUploadCancels += dayReport.StandaloneUploadCancels;
            accumulator.TaggerAndroidDeletions += dayReport.TaggerAndroidDeletions;
            accumulator.TaggerAndroidRegistrations += dayReport.TaggerAndroidRegistrations;
            accumulator.TaggerAndroidSuccessfulUploads += dayReport.TaggerAndroidSuccessfulUploads;
            accumulator.TaggerAndroidUploadCancels += dayReport.TaggerAndroidUploadCancels;
            accumulator.DailyMotionDeletions += dayReport.DailyMotionDeletions;
            accumulator.DailyMotionRegistrations += dayReport.DailyMotionRegistrations;
            accumulator.DailyMotionSuccessfulUploads += dayReport.DailyMotionSuccessfulUploads;
            accumulator.DailyMotionUploadCancels += dayReport.DailyMotionUploadCancels;
            accumulator.VkRegistrations += dayReport.VkRegistrations;
            accumulator.JwPlayerDeletions += dayReport.JwPlayerDeletions;
            accumulator.JwPlayerRegistrations += dayReport.JwPlayerRegistrations;
            accumulator.JwPlayerSuccessfulUploads += dayReport.JwPlayerSuccessfulUploads;
            accumulator.JwPlayerUploadCancels += dayReport.JwPlayerUploadCancels;
        }
    }
}