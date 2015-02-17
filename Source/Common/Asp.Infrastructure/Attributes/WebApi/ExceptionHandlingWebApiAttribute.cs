// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using Portal.Exceptions.CRUD;
using Portal.Resources.Api;

namespace Asp.Infrastructure.Attributes.WebApi
{
    public class ExceptionHandlingWebApiAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            Exception exception = context.Exception;

            // Skip cancelled tasks exceptions
            if (exception is OperationCanceledException)
            {
                return;
            }

            // Handle special portal exceptions
            if (exception is BadRequestException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest);
            }
            else if (exception is UnauthorizedException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, ResponseMessages.UnathorizedRequest);
            }
            else if (exception is PaymentRequiredException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.PaymentRequired, ResponseMessages.PaymentRequired);
            }
            else if (exception is ForbiddenException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.Forbidden, ResponseMessages.Forbidden);
            }
            else if (exception is NotFoundException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.NotFound, ResponseMessages.ResourceNotFound);
            }
            else if (exception is ConflictException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.Conflict, ResponseMessages.EntityAlreadyExists);
            }
            else if (exception is EntityTooLargeException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.RequestEntityTooLarge, ResponseMessages.EntityTooLarge);
            }
            else if (exception is TooManyRequestsException)
            {
                context.Response = context.Request.CreateErrorResponse((HttpStatusCode)429, ResponseMessages.TooManyRequests);
            }
            else if (exception is BadGatewayException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadGateway, ResponseMessages.BadGateway);
            }
            else
            {
                var responseException = exception as HttpResponseException;
                if (responseException != null)
                {
                    context.Response = responseException.Response;
                }
                else
                {
                    // unknown exception occured
                    Trace.TraceError(exception.Message + ": {0}", exception);

                    // sending response
                    context.Response = context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ResponseMessages.InternalServerError);
                }
            }
        }
    }
}