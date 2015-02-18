// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

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
    public class StatProjectCreatingAttribute : StatAttributeBase
    {
        [Import]
        public IActionDataService ActionDataService { get; set; }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            if (!CheckActionContext(actionExecutedContext, "StatProjectCreatingStateAttribute"))
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

            try
            {
                string projectId = project.Id;
                string userAgent = actionExecutedContext.Request.Headers.UserAgent.ToString();
                HttpStatusCode statusCode = actionExecutedContext.Response.StatusCode;
                DomainActionData actionData = ActionDataService.GetActionData(userAgent, (int)statusCode);

                this.Publish(new StatProjectStateEventArg(actionData, projectId, StatActionType.Project).AsTask());
            }
            catch (Exception ex)
            {
                Trace.TraceError("Failed to log project creation: {0}", ex);
            }
        }
    }
}