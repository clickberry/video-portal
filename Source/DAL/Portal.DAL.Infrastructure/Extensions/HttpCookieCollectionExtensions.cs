// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Web;

namespace Portal.DAL.Infrastructure.Extensions
{
    public static class HttpCookieCollectionExtensions
    {
        public static HttpCookie GetItem(this HttpCookieCollection collection, string name)
        {
            return collection.AllKeys.Contains(name) ? collection.Get(name) : null;
        }
    }
}