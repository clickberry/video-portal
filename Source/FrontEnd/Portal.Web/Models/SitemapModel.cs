// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace Portal.Web.Models
{
    [Serializable]
    [XmlRoot("sitemapindex", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class SitemapIndexModel
    {
        public SitemapIndexModel()
        {
            Sitemaps = new List<SitemapIndexItem>();
        }

        [XmlElement("sitemap")]
        public List<SitemapIndexItem> Sitemaps { get; set; }
    }

    public class SitemapIndexItem
    {
        [XmlElement("loc")]
        public string Location { get; set; }

        [XmlIgnore]
        public DateTime? LastModified { get; set; }

        [XmlElement("lastmod")]
        public string LastModifiedString
        {
            get { return LastModified.HasValue ? LastModified.Value.ToString("yyyy-MM-dd") : null; }
            set
            {
                DateTime dateTime;
                if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out dateTime))
                {
                    LastModified = dateTime;
                }
                else
                {
                    LastModified = null;
                }
            }
        }
    }


    [Serializable]
    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class SitemapModel
    {
        public SitemapModel()
        {
            Urls = new List<SitemapItem>();
        }

        [XmlElement("url")]
        public List<SitemapItem> Urls { get; set; }
    }

    public class SitemapItem
    {
        public SitemapItem()
        {
            Priority = 0.5;
        }

        [XmlElement("loc")]
        public string Location { get; set; }

        [XmlIgnore]
        public DateTime? LastModified { get; set; }

        [XmlElement("lastmod")]
        public string LastModifiedString
        {
            get { return LastModified.HasValue ? LastModified.Value.ToString("yyyy-MM-dd") : null; }
            set
            {
                DateTime dateTime;
                if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out dateTime))
                {
                    LastModified = dateTime;
                }
                else
                {
                    LastModified = null;
                }
            }
        }

        [XmlElement("changefreq")]
        public SitemapUrlChangeFrequency ChangeFrequency { get; set; }

        [XmlElement("priority")]
        public double Priority { get; set; }
    }

    public enum SitemapUrlChangeFrequency
    {
        [XmlEnum(Name = "always")] Always,
        [XmlEnum(Name = "hourly")] Hourly,
        [XmlEnum(Name = "daily")] Daily,
        [XmlEnum(Name = "weekly")] Weekly,
        [XmlEnum(Name = "monthly")] Monthly,
        [XmlEnum(Name = "yearly")] Yearly,
        [XmlEnum(Name = "never")] Never
    }
}