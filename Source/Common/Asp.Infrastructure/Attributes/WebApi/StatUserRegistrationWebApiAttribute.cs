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
    public class StatUserRegistrationWebApiAttribute : StatAttributeBase
    {
        [Import]
        public IActionDataService ActionDataService { get; set; }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            if (!CheckActionContext(actionExecutedContext, "StatUserRegistrationWebApiAttribute"))
            {
                return;
            }

            if (actionExecutedContext.Response.StatusCode != HttpStatusCode.Created)
            {
                return;
            }

            try
            {
                string userAgent = actionExecutedContext.Request.Headers.UserAgent.ToString();
                HttpStatusCode statusCode = actionExecutedContext.Response.StatusCode;
                DomainActionData actionData = ActionDataService.GetActionData(userAgent, (int)statusCode);

                this.Publish(new UserRegistrationEventArg(actionData).AsTask());
            }
            catch (Exception ex)
            {
                Trace.TraceError("Failed to log use registration: {0}", ex);
            }
        }
    }
}