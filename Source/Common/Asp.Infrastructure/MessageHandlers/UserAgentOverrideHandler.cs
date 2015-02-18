// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Asp.Infrastructure.MessageHandlers
{
    /// <summary>
    ///     Overrides user agent value from query into the request header.
    /// </summary>
    public sealed class UserAgentOverrideHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string userAgent = request.RequestUri.ParseQueryString().Get("ua");

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.Headers.UserAgent.Clear();
                request.Headers.UserAgent.TryParseAdd(userAgent);
                HttpContext.Current.Request.Headers["User-Agent"] = userAgent;
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}