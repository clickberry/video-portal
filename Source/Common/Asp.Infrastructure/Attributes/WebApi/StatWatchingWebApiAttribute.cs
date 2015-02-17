// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Net;
using System.Web.Http.Filters;
using AsyncEventAggregator;
using EventAggregator.EventArgs;
using Portal.BLL.Statistics.Aggregator;
using Portal.Domain.StatisticContext;

namespace Asp.Infrastructure.Attributes.WebApi
{
    public class StatWatchingWebApiAttribute : StatAttributeBase
    {
        [Import]
        public IActionDataService ActionDataService { get; set; }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            if (!CheckActionContext(actionExecutedContext, "StatWatchingWebApiAttribute"))
            {
                return;
            }

            if (actionExecutedContext.Response.StatusCode != HttpStatusCode.OK)
            {
                return;
            }

            var isReady = (bool)actionExecutedContext.Request.Properties["isReady"];
            if (!isReady)
            {
                return;
            }

            object obj;
            actionExecutedContext.ActionContext.ActionArguments.TryGetValue("projectId", out obj);
            var projectId = obj as string;
            if (projectId == null)
            {
                return;
            }

            try
            {
                string userAgent = actionExecutedContext.Request.Headers.UserAgent.ToString();
                HttpStatusCode statusCode = actionExecutedContext.Response.StatusCode;
                DomainActionData actionData = ActionDataService.GetActionData(userAgent, (int)statusCode);

                this.Publish(new WatchingEventArg(actionData, projectId).AsTask());
            }
            catch (Exception ex)
            {
                Trace.TraceError("Failed to log video watching: {0}", ex);
            }
        }
    }
}