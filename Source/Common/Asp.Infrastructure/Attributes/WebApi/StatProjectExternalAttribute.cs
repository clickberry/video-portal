// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using AsyncEventAggregator;
using EventAggregator.EventArgs;
using Portal.BLL.Statistics.Aggregator;
using Portal.Domain.StatisticContext;
using Portal.DTO.Projects;

namespace Asp.Infrastructure.Attributes.WebApi
{
    public class StatProjectExternalAttribute : StatAttributeBase
    {
        [Import]
        public IActionDataService ActionDataService { get; set; }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            if (!CheckActionContext(actionExecutedContext, "StatProjectExternalAttribute"))
            {
                return;
            }

            if (actionExecutedContext.Response.StatusCode != HttpStatusCode.Created)
            {
                return;
            }

            var objectContent = actionExecutedContext.Response.Content as ObjectContent;
            if (objectContent == null)
            {
                return;
            }

            var project = objectContent.Value as Project;
            if (project == null)
            {
                return;
            }

            bool isAvsx = actionExecutedContext.Request.Properties.ContainsKey("isAvsx");
            bool isScreenshot = actionExecutedContext.Request.Properties.ContainsKey("isScreenshot");

            try
            {
                string projectId = project.Id;
                string userAgent = actionExecutedContext.Request.Headers.UserAgent.ToString();
                HttpStatusCode statusCode = actionExecutedContext.Response.StatusCode;
                DomainActionData actionData = ActionDataService.GetActionData(userAgent, (int)statusCode);

                this.Publish(new StatProjectStateEventArg(actionData, projectId, StatActionType.Project).AsTask());
                this.Publish(new StatProjectStateEventArg(actionData, projectId, StatActionType.Video).AsTask());
                if (isAvsx)
                {
                    this.Publish(new StatProjectStateEventArg(actionData, projectId, StatActionType.Avsx).AsTask());
                }
                if (isScreenshot)
                {
                    this.Publish(new StatProjectStateEventArg(actionData, projectId, StatActionType.Screenshot).AsTask());
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Failed to log external project: {0}", ex);
            }
        }
    }
}