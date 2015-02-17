// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VkSdk
{
    internal static class VkExtensions
    {
        public static Dictionary<string, string> ToDictionary(this object @object)
        {
            List<PropertyInfo> properties = @object.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.GetField).ToList();
            var result = new Dictionary<string, string>(properties.Count);
            foreach (PropertyInfo property in properties)
            {
                result.Add(property.Name, property.GetValue(@object).ToString());
            }

            return result;
        }
    }
}