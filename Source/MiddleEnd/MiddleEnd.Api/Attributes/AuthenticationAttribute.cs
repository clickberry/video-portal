// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.ClientServices;
using System.Web.Http.Filters;
using Configuration;
using Portal.DAL.Infrastructure.Authentication;

namespace MiddleEnd.Api.Attributes
{
    public class AuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        [Import]
        public IPortalMiddleendSettings Settings { get; set; }

        public bool AllowMultiple
        {
            get { return false; }
        }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            AuthenticationHeaderValue authorization = context.Request.Headers.Authorization;
            if (string.Equals(authorization.Scheme, "Bearer", StringComparison.OrdinalIgnoreCase) &&
                authorization.Parameter == Settings.BearerToken)
            {
                context.Principal = new ClientRolePrincipal(new SocialIdentity("Bearer"));
            }

            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}