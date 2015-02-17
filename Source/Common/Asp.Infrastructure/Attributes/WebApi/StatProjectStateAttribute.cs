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
    public class StatProjectStateAttribute : StatAttributeBase
    {
        private readonly string _actionType;

        public StatProjectStateAttribute(string actionType)
        {
            _actionType = actionType;
        }

        [Import]
        public IActionDataService ActionDataService { get; set; }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            if (!CheckActionContext(actionExecutedContext, "StatProjectStateAttribute"))
            {
                return;
            }

            if (actionExecutedContext.Response.StatusCode != HttpStatusCode.Created)
            {
                return;
            }

            object obj;
            actionExecutedContext.ActionContext.ActionArguments.TryGetValue("id", out obj);
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

                this.Publish(new StatProjectStateEventArg(actionData, projectId, _actionType).AsTask());
            }
            catch (Exception ex)
            {
                Trace.TraceError("Failed to log project state: {0}", ex);
            }
        }
    }
}