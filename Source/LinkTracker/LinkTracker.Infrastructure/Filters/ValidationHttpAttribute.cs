// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace LinkTracker.Infrastructure.Filters
{
    /// <summary>
    ///     Validates model state and returs errors description.
    /// </summary>
    public sealed class ValidationHttpAttribute : ActionFilterAttribute
    {
        public ValidationHttpAttribute()
        {
            BadRequestText = "Bad request.";
        }

        public string BadRequestText { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            if (actionContext.ActionArguments.Any(p => p.Value == null))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, BadRequestText);
            }

            ModelStateDictionary modelState = actionContext.ModelState;
            if (modelState.IsValid)
            {
                return;
            }

            var modelDictionary = new ModelStateDictionary();
            foreach (var kv in modelState.Where(kv => kv.Value.Errors.Count != 0))
            {
                modelDictionary.AddModelError(kv.Key.Split('.').Last(), kv.Value.Errors.First().ErrorMessage);
            }

            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                modelDictionary);
        }
    }
}