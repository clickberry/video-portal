// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Runtime.Serialization;

namespace Portal.DTO.Reports
{
    [DataContract(Namespace = "")]
    public class Report
    {
        [DataMember(Order = 100)]
        public ReportInterval Interval { get; set; }

        [DataMember(Order = 10100)]
        public long AllRegistrations { get; set; }

        [DataMember(Order = 10200)]
        public long EmailRegistrations { get; set; }

        [DataMember(Order = 10300)]
        public long FacebookRegistrations { get; set; }

        [DataMember(Order = 10400)]
        public long GoogleRegistrations { get; set; }

        [DataMember(Order = 10500)]
        public long WindowsLiveRegistrations { get; set; }

        [DataMember(Order = 10600)]
        public long YahooRegistrations { get; set; }

        [DataMember(Order = 10700)]
        public long TwitterRegistrations { get; set; }

        [DataMember(Order = 10800)]
        public long VkRegistrations { get; set; }

        [DataMember(Order = 10900)]
        public long OdnoklassnikiRegistrations { get; set; }


        [DataMember(Order = 20100)]
        public long CicIPadRegistrations { get; set; }

        [DataMember(Order = 20200)]
        public long CicPcRegistrations { get; set; }

        [DataMember(Order = 20300)]
        public long CicMacRegistrations { get; set; }

        [DataMember(Order = 20400)]
        public long TaggerIPhoneRegistrations { get; set; }

        [DataMember(Order = 20500)]
        public long ImageShackRegistrations { get; set; }

        [DataMember(Order = 20600)]
        public long TaggerAndroidRegistrations { get; set; }

        [DataMember(Order = 20700)]
        public long StandaloneRegistrations { get; set; }

        [DataMember(Order = 20800)]
        public long PlayerRegistrations { get; set; }

        [DataMember(Order = 20900)]
        public long DailyMotionRegistrations { get; set; }

        [DataMember(Order = 21000)]
        public long JwPlayerRegistrations { get; set; }

        [DataMember(Order = 21100)]
        public long BrowserRegistrations { get; set; }

        [DataMember(Order = 21200)]
        public long OtherRegistrations { get; set; }


        [DataMember(Order = 30100)]
        public long CicIPadSuccessfulUploads { get; set; }

        [DataMember(Order = 30200)]
        public long CicMacSuccessfulUploads { get; set; }

        [DataMember(Order = 30300)]
        public long CicPcSuccessfulUploads { get; set; }

        [DataMember(Order = 30400)]
        public long TaggerIPhoneSuccessfulUploads { get; set; }

        [DataMember(Order = 30500)]
        public long ImageShackSuccessfulUploads { get; set; }

        [DataMember(Order = 30600)]
        public long TaggerAndroidSuccessfulUploads { get; set; }

        [DataMember(Order = 30700)]
        public long StandaloneSuccessfulUploads { get; set; }

        [DataMember(Order = 30800)]
        public long PlayerSuccessfulUploads { get; set; }

        [DataMember(Order = 30900)]
        public long DailyMotionSuccessfulUploads { get; set; }

        [DataMember(Order = 31000)]
        public long JwPlayerSuccessfulUpload { get; set; }


        [DataMember(Order = 40100)]
        public long CicIPadDeletions { get; set; }

        [DataMember(Order = 40200)]
        public long CicMacDeletions { get; set; }

        [DataMember(Order = 40300)]
        public long CicPcDeletions { get; set; }

        [DataMember(Order = 40400)]
        public long TaggerIPhoneDeletions { get; set; }

        [DataMember(Order = 40500)]
        public long ImageShackDeletions { get; set; }

        [DataMember(Order = 40600)]
        public long TaggerAndroidDeletions { get; set; }

        [DataMember(Order = 40700)]
        public long StandaloneDeletions { get; set; }

        [DataMember(Order = 40800)]
        public long PlayerDeletions { get; set; }

        [DataMember(Order = 40900)]
        public long DailyMotionDeletions { get; set; }

        [DataMember(Order = 41000)]
        public long JwPlayerDeletions { get; set; }


        [DataMember(Order = 50100)]
        public long CicIPadUploadCancels { get; set; }

        [DataMember(Order = 50200)]
        public long CicMacUploadCancels { get; set; }

        [DataMember(Order = 50300)]
        public long CicPcUploadCancels { get; set; }

        [DataMember(Order = 50400)]
        public long TaggerIPhoneUploadCancels { get; set; }

        [DataMember(Order = 50500)]
        public long ImageShackUploadCancels { get; set; }

        [DataMember(Order = 50600)]
        public long TaggerAndroidUploadCancels { get; set; }

        [DataMember(Order = 50700)]
        public long StandaloneUploadCancels { get; set; }

        [DataMember(Order = 50800)]
        public long PlayerUploadCancels { get; set; }

        [DataMember(Order = 50900)]
        public long DailyMotionUploadCancels { get; set; }

        [DataMember(Order = 51000)]
        public long JwPlayerUploadCancels { get; set; }


        [DataMember(Order = 60100)]
        public long TotalViews { get; set; }

        [DataMember(Order = 60200)]
        public long EmbeddedViews { get; set; }
    }
}