// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Web.Mvc;
using AsyncEventAggregator;
using EventAggregator.EventArgs;
using Portal.BLL.Statistics.Aggregator;
using Portal.Domain.StatisticContext;

namespace Asp.Infrastructure.Attributes.Mvc
{
    public class StatWatchingMvcAttribute : ActionFilterAttribute
    {
        [Import]
        public IActionDataService ActionDataService { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (filterContext == null)
            {
                Trace.TraceError("StatWatchingMvcAttribute: an ActionExecutedContext is NULL.");
                return;
            }

            if (!(filterContext.Result is ViewResult))
            {
                return;
            }
            if ((filterContext.Result as ViewResult).Model == null)
            {
                return;
            }

            bool isReady = filterContext.HttpContext.Items.Contains("isReady") &&
                           (bool)filterContext.HttpContext.Items["isReady"];

            if (!isReady)
            {
                return;
            }

            ValueProviderResult result = filterContext.Controller.ValueProvider.GetValue("id");
            string projectId = result != null ? result.RawValue as string : null;
            if (projectId == null)
            {
                return;
            }

            try
            {
                string userAgent = filterContext.HttpContext.Request.UserAgent;
                int statusCode = filterContext.HttpContext.Response.StatusCode;
                DomainActionData actionData = ActionDataService.GetActionData(userAgent, statusCode);

                this.Publish(new WatchingEventArg(actionData, projectId).AsTask());
            }
            catch (Exception ex)
            {
                Trace.TraceError("Failed to log video watching: {0}", ex);
            }
        }
    }
}