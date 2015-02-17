// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Configuration
{
    /// <summary>
    ///     Configuration settings names.
    /// </summary>
    public interface IPortalFrontendSettings : IPortalSettings
    {
        /// <summary>
        ///     Default value of maximum user storage disk space.
        /// </summary>
        long DefaultMaxUserCapacity { get; }

        /// <summary>
        ///     URI of Azure CDN endpoint.
        /// </summary>
        string CdnUri { get; }

        /// <summary>
        ///     URI of player root location.
        /// </summary>
        string PlayerUrl { get; }

        /// <summary>
        ///     Defines whether persistance should be in the read only mode.
        /// </summary>
        bool StorageReadOnly { get; }

        /// <summary>
        ///     Defines whether persistance should be in the read only mode.
        /// </summary>
        string AcsNamespace { get; }

        /// <summary>
        ///     Defines whether to send email notification.
        /// </summary>
        bool EmailNotifications { get; }

        /// <summary>
        ///     Defines info e-mail address.
        /// </summary>
        string EmailAddressInfo { get; }

        /// <summary>
        ///     Defines a support e-mail address.
        /// </summary>
        string EmailAddressSupport { get; }

        /// <summary>
        ///     Defines a e-mail address for abuse reports.
        /// </summary>
        string EmailAddressAbuse { get; }

        /// <summary>
        ///     Gets or sets a facebook app identifier.
        /// </summary>
        string FacebookApplicationId { get; }

        /// <summary>
        ///     Gets or sets a facebook app secret.
        /// </summary>
        string FacebookApplicationSecret { get; }

        string TwitterConsumerKey { get; }
        string TwitterConsumerSecret { get; }
        string VkApplicationId { get; }
        string VkApplicationSecret { get; }
        string OkApplicationId { get; }
        string OkApplicationSecret { get; }
        string OkApplicationPublic { get; }
        string ExtensionUri { get; }
        string YoutubePlayerUrl { get; }
        string DailymotionPlayerUrl { get; }
        string YoutubeHtml5PlayerUrl { get; }
        string JwFlashPlayerUrl { get; }
        string NotificationHubConnectionString { get; }
        string NotificationHubName { get; }

        /// <summary>
        ///     Version of Portal UI.
        /// </summary>
        string PortalUIPackageUri { get; }

        /// <summary>
        ///     Keeps Google Analytics Identifier.
        /// </summary>
        string GoogleAnalyticsId { get; }

        string LinkTrackerUri { get; }

        /// <summary>
        ///     Json object with facebook registration message data.
        /// </summary>
        string FacebookRegistrationMessage { get; }

        string StripeApiKey { get; }
        string StripePublicKey { get; }

        /// <summary>
        ///     Portal path to recover account password.
        /// </summary>
        string AccountSetPasswordPath { get; }

        /// <summary>
        ///     Gets a list of cassandra node addresses.
        /// </summary>
        string[] CassandraNodes { get; }

        /// <summary>
        ///     Cassandra username.
        /// </summary>
        string CassandraUsername { get; }

        /// <summary>
        ///     Cassandra password.
        /// </summary>
        string CassandraPassword { get; }

        /// <summary>
        ///     Cassandra data keyspace (database name).
        /// </summary>
        string CassandraKeyspace { get; }

        /// <summary>
        ///     Gets a list of cassandra node addresses behind firewall.
        ///     Note: it should match sequence of CassandraNodes addresses.
        /// </summary>
        string[] CassandraPrivateAddresses { get; }

        /// <summary>
        ///     Gets a list of banners to display on the front page.
        /// </summary>
        List<BannerSetting> FrontPageBanners { get; }

        /// <summary>
        ///     Gets an HTML markup for banner to display alongside with video.
        /// </summary>
        string VideoViewBanner { get; }

        /// <summary>
        ///     Gets an HTML markup for banners to display to the left of the content pages (Editor's pick, Trends).
        /// </summary>
        string ContentBannerLeft { get; }

        /// <summary>
        ///     Gets an HTML markup for banners to display to the right of the content pages (Editor's pick, Trends).
        /// </summary>
        string ContentBannerRight { get; }

        /// <summary>
        ///     Gets an URI for default avatar image.
        /// </summary>
        string DefaultAvatarUri { get; }

        /// <summary>
        ///     Gets a list of available social networks.
        /// </summary>
        string SocialNetworks { get; }

        /// <summary>
        ///     Gets a list of dowload links.
        /// </summary>
        DownloadLinks DownloadLinks { get; }
    }
}