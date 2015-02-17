// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.Domain.StatisticContext
{
    public class DomainStatWatching
    {
        public string EventId { get; set; }

        public string Tick { get; set; }

        public string ProjectId { get; set; }

        public string UserId { get; set; }

        public string AnonymousId { get; set; }

        public bool IsAuthenticated { get; set; }

        public DateTime DateTime { get; set; }

        public string UrlReferrer { get; set; }

        public string UserAgent { get; set; }
    }
}