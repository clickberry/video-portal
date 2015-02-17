// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using SimpleInjector;

namespace LinkTracker.Infrastructure.Filters
{
    public class SimpleInjectorWebApiFilterProvider : IFilterProvider
    {
        private readonly Container _container;

        public SimpleInjectorWebApiFilterProvider(Container container)
        {
            _container = container;
        }

        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            IEnumerable<FilterInfo> controllerFilters =
                actionDescriptor.ControllerDescriptor.GetFilters()
                    .Select(instance => new FilterInfo(instance, FilterScope.Controller));
            IEnumerable<FilterInfo> actionFilters =
                actionDescriptor.GetFilters().Select(instance => new FilterInfo(instance, FilterScope.Action));

            return controllerFilters.Concat(actionFilters).Select(p =>
            {
                _container.InjectProperties(p.Instance);
                return p;
            });
        }
    }
}