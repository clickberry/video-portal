// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Configuration.Azure.Concrete
{
    public sealed class PortalFrontendSettings : PortalSettings, IPortalFrontendSettings
    {
        private readonly IConfigurationProvider _provider;

        public PortalFrontendSettings(IConfigurationProvider provider) : base(provider)
        {
            _provider = provider;
        }

        public long DefaultMaxUserCapacity
        {
            get { return _provider.Get<long>("DefaultMaxUserCapacity"); }
        }

        public string CdnUri
        {
            get { return _provider.Get("CdnUri"); }
        }

        public string PlayerUrl
        {
            get { return _provider.Get("PlayerUrl"); }
        }

        public bool StorageReadOnly
        {
            get { return _provider.Get<bool>("StorageReadOnly"); }
        }

        public string AcsNamespace
        {
            get { return _provider.Get("AcsNamespace"); }
        }

        public bool EmailNotifications
        {
            get { return _provider.Get<bool>("EmailNotifications"); }
        }

        public string EmailAddressInfo
        {
            get { return _provider.Get("EmailAddressInfo"); }
        }

        public string EmailAddressSupport
        {
            get { return _provider.Get("EmailAddressSupport"); }
        }

        public string EmailAddressAbuse
        {
            get { return _provider.Get("EmailAddressAbuse"); }
        }

        public string FacebookApplicationId
        {
            get { return _provider.Get("FacebookApplicationId"); }
        }

        public string FacebookApplicationSecret
        {
            get { return _provider.Get("FacebookApplicationSecret"); }
        }

        public string TwitterConsumerKey
        {
            get { return _provider.Get("TwitterConsumerKey"); }
        }

        public string TwitterConsumerSecret
        {
            get { return _provider.Get("TwitterConsumerSecret"); }
        }

        public string VkApplicationId
        {
            get { return _provider.Get("VkApplicationId"); }
        }

        public string VkApplicationSecret
        {
            get { return _provider.Get("VkApplicationSecret"); }
        }

        public string OkApplicationId
        {
            get { return _provider.Get("OkApplicationId"); }
        }

        public string OkApplicationSecret
        {
            get { return _provider.Get("OkApplicationSecret"); }
        }

        public string OkApplicationPublic
        {
            get { return _provider.Get("OkApplicationPublic"); }
        }

        public string ExtensionUri
        {
            get { return _provider.Get("ExtensionUri"); }
        }

        public string YoutubePlayerUrl
        {
            get { return _provider.Get("YoutubePlayerUrl"); }
        }

        public string DailymotionPlayerUrl
        {
            get { return _provider.Get("DailymotionPlayerUrl"); }
        }

        public string YoutubeHtml5PlayerUrl
        {
            get { return _provider.Get("YoutubeHtml5PlayerUrl"); }
        }

        public string JwFlashPlayerUrl
        {
            get { return _provider.Get("JwFlashPlayerUrl"); }
        }

        public string NotificationHubConnectionString
        {
            get { return _provider.Get("NotificationHubConnectionString"); }
        }

        public string NotificationHubName
        {
            get { return _provider.Get("NotificationHubName"); }
        }

        public string PortalUIPackageUri
        {
            get { return _provider.Get("PortalUIPackageUri"); }
        }

        public string GoogleAnalyticsId
        {
            get { return _provider.Get("GoogleAnalyticsId"); }
        }

        public string LinkTrackerUri
        {
            get { return _provider.Get("LinkTrackerUri"); }
        }

        public string FacebookRegistrationMessage
        {
            get { return _provider.Get("FacebookRegistrationMessage"); }
        }

        public string StripeApiKey
        {
            get { return _provider.Get("StripeApiKey"); }
        }

        public string StripePublicKey
        {
            get { return _provider.Get("StripePublicKey"); }
        }

        public string AccountSetPasswordPath
        {
            get { return _provider.Get("AccountSetPasswordPath"); }
        }

        public string[] CassandraNodes
        {
            get { return _provider.Get("CassandraNodes").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); }
        }

        public string CassandraUsername
        {
            get { return _provider.Get("CassandraUsername"); }
        }

        public string CassandraPassword
        {
            get { return _provider.Get("CassandraPassword"); }
        }

        public string CassandraKeyspace
        {
            get { return _provider.Get("CassandraKeyspace"); }
        }

        public string[] CassandraPrivateAddresses
        {
            get { return _provider.Get("CassandraPrivateAddresses").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); }
        }

        public List<BannerSetting> FrontPageBanners
        {
            get
            {
                string setting = _provider.Get("FrontPageBanners");
                if (string.IsNullOrEmpty(setting))
                {
                    return new List<BannerSetting>();
                }

                try
                {
                    JObject json = JObject.Parse(setting);
                    var result = new List<BannerSetting>();
                    foreach (var i in json)
                    {
                        string key = i.Key;
                        string value = i.Value.ToString();
                        result.Add(new BannerSetting { ImageUrl = key, LinkUrl = value });
                    }

                    return result;
                }
                catch (Exception)
                {
                    return new List<BannerSetting>();
                }
            }
        }

        public string VideoViewBanner
        {
            get { return _provider.Get("VideoViewBanner"); }
        }

        public string ContentBannerLeft
        {
            get { return _provider.Get("ContentBannerLeft"); }
        }

        public string ContentBannerRight
        {
            get { return _provider.Get("ContentBannerRight"); }
        }

        public string DefaultAvatarUri
        {
            get { return _provider.Get("DefaultAvatarUri"); }
        }

        public string SocialNetworks
        {
            get { return _provider.Get("SocialNetworks"); }
        }

        public DownloadLinks DownloadLinks
        {
            get
            {
                var result = new DownloadLinks();

                string setting = _provider.Get("DownloadLinks");
                if (!string.IsNullOrEmpty(setting))
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject<DownloadLinks>(setting);
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError("Failed to parse DownloadLinks setting: {0}", e);
                    }
                }

                return result;
            }
        }
    }
}