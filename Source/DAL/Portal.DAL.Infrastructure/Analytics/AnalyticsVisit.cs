// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.DAL.Infrastructure.Analytics
{
    public sealed class AnalyticsVisit
    {
        public string UserAgent { get; set; }

        /// <summary>
        ///     Anonymous user identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     User IP address.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        ///     Document referrer.
        /// </summary>
        public string Referrer { get; set; }

        /// <summary>
        ///     Document path.
        /// </summary>
        public string Path { get; set; }
    }
}