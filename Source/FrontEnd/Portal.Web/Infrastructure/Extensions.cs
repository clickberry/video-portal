// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Specialized;

namespace Portal.Web.Infrastructure
{
    public static class Extensions
    {
        public static Dictionary<string, string> ToDictionary(this NameValueCollection collection)
        {
            var result = new Dictionary<string, string>(collection.Count);

            foreach (string key in collection.AllKeys)
            {
                result.Add(key, collection[key]);
            }

            return result;
        }
    }
}