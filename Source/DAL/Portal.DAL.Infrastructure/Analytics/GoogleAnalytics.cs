// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Portal.DAL.Infrastructure.Analytics
{
    public sealed class GoogleAnalytics : IAnalytics
    {
        private readonly Uri _collectUrl = new Uri("https://ssl.google-analytics.com/collect");
        private readonly string _googleAnalyticsId;

        public GoogleAnalytics(string googleAnalyticsId)
        {
            _googleAnalyticsId = googleAnalyticsId;
        }

        public async Task CollectVisitAsync(AnalyticsVisit data)
        {
            using (var client = new HttpClient())
            {
                HttpContent content = new FormUrlEncodedContent(
                    new Dictionary<string, string>
                    {
                        { "v", "1" }, // protocol version,
                        { "tid", _googleAnalyticsId }, // site identifier
                        { "cid", data.UserId }, // user identifier
                        { "uip", data.IpAddress }, // user IP address
                        { "dr", data.Referrer }, // user referrer
                        { "dp", data.Path }, // document path
                        { "t", "pageview" }, // type
                    });

                await client.PostAsync(_collectUrl, content).ConfigureAwait(false);
            }
        }
    }
}