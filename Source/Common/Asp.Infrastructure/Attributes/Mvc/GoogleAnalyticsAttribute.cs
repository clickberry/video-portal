// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Web;
using System.Web.Mvc;
using Portal.DAL.Infrastructure.Analytics;

namespace Asp.Infrastructure.Attributes.Mvc
{
    public class GoogleAnalyticsAttribute : ActionFilterAttribute
    {
        [Import]
        public IAnalytics Analytics { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;

            HttpCookie idCookie = request.Cookies["cbuid"];
            string id = idCookie != null && !string.IsNullOrEmpty(idCookie.Value)
                ? idCookie.Value
                : Guid.NewGuid().ToString("N");

            Analytics.CollectVisitAsync(new AnalyticsVisit
            {
                UserId = id,
                IpAddress = request.UserHostAddress,
                Path = request.Url != null ? request.Url.PathAndQuery : null,
                Referrer = request.UrlReferrer != null ? request.UrlReferrer.ToString() : null,
                UserAgent = request.UserAgent
            }).Wait();
        }
    }
}