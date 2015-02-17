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
    public class StatProjectUploadingWebApiAttribute : StatAttributeBase
    {
        [Import]
        public IActionDataService ActionDataService { get; set; }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            if (!CheckActionContext(actionExecutedContext, "StatProjectUploadingWebApiAttribute"))
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
                string userAgent = actionExecutedContext.Request.Headers.UserAgent.ToString();
                HttpStatusCode statusCode = actionExecutedContext.Response.StatusCode;
                DomainActionData actionData = ActionDataService.GetActionData(userAgent, (int)statusCode);

                this.Publish(new ProjectUploadingEventArg(actionData, project.Id, project.Name, project.ProjectType, project.ProjectSubtype).AsTask());
            }
            catch (Exception ex)
            {
                Trace.TraceError("Failed to log project uploading: {0}", ex);
            }
        }
    }
}