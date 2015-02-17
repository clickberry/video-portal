// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OkSdk
{
    internal static class OkExtensions
    {
        public static Dictionary<string, string> ToDictionary(this object @object)
        {
            if (@object == null)
            {
                return new Dictionary<string, string>();
            }

            List<PropertyInfo> properties = @object.GetType().GetProperties().ToList();
            var result = new Dictionary<string, string>(properties.Count);
            foreach (PropertyInfo property in properties)
            {
                result.Add(property.Name, property.GetValue(@object).ToString());
            }

            return result;
        }
    }
}