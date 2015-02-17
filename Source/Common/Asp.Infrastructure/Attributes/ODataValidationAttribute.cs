// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData.Query;

namespace Asp.Infrastructure.Attributes
{
    public sealed class ODataValidationAttribute : ActionFilterAttribute
    {
        public int MinSkipValue { get; set; }

        public int MinTopValue { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            var options = actionContext.ActionArguments.Where(p => p.Value is ODataQueryOptions).Select(p => p.Value).OfType<ODataQueryOptions>().FirstOrDefault();

            if (options == null)
            {
                return;
            }

            var modelDictionary = new ModelStateDictionary();

            if (options.Skip != null)
            {
                if (options.Skip.Value < MinSkipValue)
                {
                    modelDictionary.AddModelError("Skip", String.Format("Value must be greater than or equal {0}", MinSkipValue));
                }
            }

            if (options.Top != null)
            {
                if (options.Top.Value < MinTopValue)
                {
                    modelDictionary.AddModelError("Top", String.Format("Value must be greater than or equal {0}", MinTopValue));
                }
            }

            if (modelDictionary.Count > 0)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelDictionary);
            }
        }
    }
}