// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.DAL.Infrastructure.Authentication
{
    public sealed class CookieNames
    {
        public const string CookieName = "cbat";
        public const string AnonymousCookieName = "cbuid";
        public const string AuthenticationCookieName = "cbac";
        public const string RolesCookieName = "cbroles";
    }
}