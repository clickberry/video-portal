// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using Microsoft.Data.OData;
using Portal.Exceptions.CRUD;

namespace Asp.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ODataValidationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var httpResponseException = context.Exception as HttpResponseException;
            // Handle OData exceptions caused by empty or null values in query parameters
            if (context.Exception is ODataException ||
                context.Exception is ArgumentException ||
                context.Exception is OverflowException ||
                (httpResponseException != null &&
                 httpResponseException.Response.StatusCode == HttpStatusCode.BadRequest))
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, context.Exception.Message);
                context.Exception = new BadRequestException(context.Exception.Message);
            }
        }
    }
}