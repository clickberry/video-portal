// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

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
    public class StatUserRegistrationMvcAttribute : ActionFilterAttribute
    {
        [Import]
        public IActionDataService ActionDataService { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (filterContext == null)
            {
                Trace.TraceError("StatUserRegistrationMvcAttribute: an ActionExecutedContext is NULL.");
                return;
            }

            if (!(filterContext.Result is RedirectToRouteResult))
            {
                return;
            }

            bool isRegister = filterContext.HttpContext.Items.Contains("isRegister");
            if (!isRegister)
            {
                return;
            }

            try
            {
                string userAgent = filterContext.HttpContext.Request.UserAgent;
                int statusCode = filterContext.HttpContext.Response.StatusCode;
                DomainActionData actionData = ActionDataService.GetActionData(userAgent, statusCode);

                this.Publish(new UserRegistrationEventArg(actionData).AsTask());
            }
            catch (Exception ex)
            {
                Trace.TraceError("Failed to log user registration: {0}", ex);
            }
        }
    }
}