// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Web.Mvc;
using EventAggregator;
using EventAggregator.Subscribers;

namespace Portal.Web.App_Start
{
    public static class EventAggregatorConfig
    {
        public static void Configure(IDependencyResolver dependencyResolver)
        {
            var list = new List<IEventSubscriber>
            {
                (IEventSubscriber)dependencyResolver.GetService(typeof (WatchingSubscriber)),
                (IEventSubscriber)dependencyResolver.GetService(typeof (UserRegistrationSubscriber)),
                (IEventSubscriber)dependencyResolver.GetService(typeof (UserLoginSubscriber))
            };

            foreach (IEventSubscriber eventSubscriber in list)
            {
                eventSubscriber.SubscribeEvent().Wait();
            }
        }
    }
}