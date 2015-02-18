// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Web.Http.Filters;

namespace Asp.Infrastructure.Attributes.WebApi
{
    public abstract class StatAttributeBase : ActionFilterAttribute
    {
        protected bool CheckActionContext(HttpActionExecutedContext actionExecutedContext, string prefix = "")
        {
            if (actionExecutedContext == null)
            {
                Trace.TraceError("{0}: a HttpActionExecutedContext is NULL", prefix);
                return false;
            }
            if (actionExecutedContext.Response == null)
            {
                Trace.TraceInformation("{0}: a Response is NULL", prefix);
                return false;
            }

            return true;
        }
    }
}