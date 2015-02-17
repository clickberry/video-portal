// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Asp.Infrastructure.MessageHandlers
{
    /// <summary>
    ///     Handles X-HTTP-Method-Override request's headers.
    /// </summary>
    public class MethodOverrideHandler : DelegatingHandler
    {
        private const string Header = "X-HTTP-Method-Override";
        private readonly string[] _methods = { "DELETE", "HEAD", "PUT" };

        /// <summary>
        ///     Changes http method if required.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Check for HTTP POST with the X-HTTP-Method-Override header.
            if (request.Method == HttpMethod.Post && request.Headers.Contains(Header))
            {
                // Check if the header value is in our methods list.
                string method = request.Headers.GetValues(Header).FirstOrDefault();

                if (!string.IsNullOrEmpty(method) && _methods.Contains(method, StringComparer.InvariantCultureIgnoreCase))
                {
                    // Change the request method.
                    request.Method = new HttpMethod(method);
                }
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}