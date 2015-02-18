// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http.Filters;
using Portal.DTO.Common;

namespace Portal.Api.Infrastructure.Attributes
{
    /// <summary>
    ///     Temp file cleaner.
    /// </summary>
    public class ClearFilesHttpAttribute : ActionFilterAttribute
    {
        private static readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> AttributeUsageCache
            = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            foreach (var argument in actionExecutedContext.ActionContext.ActionArguments)
            {
                if (argument.Value == null)
                {
                    continue;
                }

                IEnumerable<PropertyInfo> properties = GetProperties(argument.Value.GetType());

                foreach (PropertyInfo propertyInfo in properties)
                {
                    var value = propertyInfo.GetValue(argument.Value, null) as FileEntity;

                    if (value == null)
                    {
                        continue;
                    }

                    try
                    {
                        File.Delete(value.Uri);
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError("Failed to delete temp file: {0}", e);
                    }
                }
            }
        }

        private IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return AttributeUsageCache.GetOrAdd(type, type.GetProperties().Where(p => p.GetIndexParameters().Length == 0));
        }
    }
}