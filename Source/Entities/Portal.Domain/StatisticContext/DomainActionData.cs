// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.Domain.StatisticContext
{
    public class DomainActionData
    {
        public string HttpMethod { get; set; }

        public int StatusCode { get; set; }

        public string Url { get; set; }

        public string UrlReferrer { get; set; }

        public string UserAgent { get; set; }

        public string UserHostAddress { get; set; }

        public string UserHostName { get; set; }

        public string[] UserLanguages { get; set; }

        public string UserId { get; set; }

        public string AnonymousId { get; set; }

        public bool IsAuthenticated { get; set; }

        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public string IdentityProvider { get; set; }
    }
}