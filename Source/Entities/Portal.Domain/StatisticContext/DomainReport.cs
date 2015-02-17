// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.Domain.StatisticContext
{
    public class DomainReport
    {
        public DateTime Tick { get; set; }

        public string Interval { get; set; }

        public long AllRegistrations { get; set; }

        public long CicIPadDeletions { get; set; }

        public long CicIPadRegistrations { get; set; }

        public long CicIPadSuccessfulUploads { get; set; }

        public long CicMacDeletions { get; set; }

        public long CicMacRegistrations { get; set; }

        public long CicMacSuccessfulUploads { get; set; }

        public long CicPcDeletions { get; set; }

        public long CicPcRegistrations { get; set; }

        public long CicPcSuccessfulUploads { get; set; }

        public long EmailRegistrations { get; set; }

        public long EmbeddedViews { get; set; }

        public long FacebookRegistrations { get; set; }

        public long GoogleRegistrations { get; set; }

        public long TaggerIPhoneDeletions { get; set; }

        public long TaggerIPhoneRegistrations { get; set; }

        public long TaggerIPhoneUploads { get; set; }

        public long TotalViews { get; set; }

        public long WindowsLiveRegistrations { get; set; }

        public long YahooRegistrations { get; set; }

        public long ImageShackRegistrations { get; set; }

        public long ImageShackSuccessfulUploads { get; set; }

        public long ImageShackDeletions { get; set; }

        public long TwitterRegistrations { get; set; }

        public long CicIPadUploadCancels { get; set; }

        public long CicMacUploadCancels { get; set; }

        public long CicPcUploadCancels { get; set; }

        public long TaggerIPhoneUploadCancels { get; set; }

        public long ImageShackUploadCancels { get; set; }

        public long TaggerAndroidRegistrations { get; set; }

        public long StandaloneRegistrations { get; set; }

        public long PlayerRegistrations { get; set; }

        public long TaggerAndroidSuccessfulUploads { get; set; }

        public long StandaloneSuccessfulUploads { get; set; }

        public long PlayerSuccessfulUploads { get; set; }

        public long TaggerAndroidUploadCancels { get; set; }

        public long StandaloneUploadCancels { get; set; }

        public long PlayerUploadCancels { get; set; }

        public long TaggerAndroidDeletions { get; set; }

        public long StandaloneDeletions { get; set; }

        public long PlayerDeletions { get; set; }

        public long BrowserRegistrations { get; set; }

        public long OtherRegistrations { get; set; }

        public long DailyMotionUploadCancels { get; set; }

        public long DailyMotionDeletions { get; set; }

        public long DailyMotionRegistrations { get; set; }

        public long DailyMotionSuccessfulUploads { get; set; }

        public long VkRegistrations { get; set; }

        public long JwPlayerUploadCancels { get; set; }

        public long JwPlayerDeletions { get; set; }

        public long JwPlayerRegistrations { get; set; }

        public long JwPlayerSuccessfulUploads { get; set; }

        public long OdnoklassnikiRegistrations { get; set; }
    }
}