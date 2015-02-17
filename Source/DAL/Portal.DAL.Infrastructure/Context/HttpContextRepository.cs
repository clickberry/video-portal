// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Web;
using Portal.DAL.Context;

namespace Portal.DAL.Infrastructure.Context
{
    public class HttpContextRepository : IHttpContextRepository
    {
        public string GetUrl()
        {
            Uri url = HttpContext.Current.Request.Url;
            return url == null ? String.Empty : url.AbsoluteUri;
        }

        public string GetUrlReferrer()
        {
            Uri referrer = HttpContext.Current.Request.UrlReferrer;
            return referrer == null ? String.Empty : referrer.AbsoluteUri;
        }

        public string GetUserHostAddress()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }

        public string GetUserHostName()
        {
            return HttpContext.Current.Request.UserHostName;
        }

        public string GetUserAgent()
        {
            return HttpContext.Current.Request.UserAgent;
        }

        public string[] GetUserLanguages()
        {
            return HttpContext.Current.Request.UserLanguages;
        }

        public string GetHttpMethod()
        {
            return HttpContext.Current.Request.HttpMethod;
        }

        public int GetStatusCode()
        {
            return HttpContext.Current.Response.StatusCode;
        }
    }
}