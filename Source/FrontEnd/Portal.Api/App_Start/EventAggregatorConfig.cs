// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Web.Http.Dependencies;
using EventAggregator;
using EventAggregator.Subscribers;

namespace Portal.Api
{
    public static class EventAggregatorConfig
    {
        public static void Configure(IDependencyResolver dependencyResolver)
        {
            var list = new List<IEventSubscriber>
            {
                (IEventSubscriber)dependencyResolver.GetService(typeof (WatchingSubscriber)),
                (IEventSubscriber)dependencyResolver.GetService(typeof (UserRegistrationSubscriber)),
                (IEventSubscriber)dependencyResolver.GetService(typeof (ProjectUploadingSubscriber)),
                (IEventSubscriber)dependencyResolver.GetService(typeof (ProjectDeletionSubscriber)),
                (IEventSubscriber)dependencyResolver.GetService(typeof (StatProjectStateSubscriber)),
                (IEventSubscriber)dependencyResolver.GetService(typeof (UserLoginSubscriber)),
            };

            foreach (IEventSubscriber eventSubscriber in list)
            {
                eventSubscriber.SubscribeEvent().Wait();
            }
        }
    }
}