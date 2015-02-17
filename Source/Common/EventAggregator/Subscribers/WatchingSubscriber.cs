// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using AsyncEventAggregator;
using EventAggregator.EventArgs;
using Portal.BLL.Statistics.Aggregator;

namespace EventAggregator.Subscribers
{
    public class WatchingSubscriber : IEventSubscriber
    {
        private readonly IStatWatchingService _statWatchingAggregator;

        public WatchingSubscriber(IStatWatchingService statWatchingAggregator)
        {
            _statWatchingAggregator = statWatchingAggregator;
        }

        public Task SubscribeEvent()
        {
            return this.Subscribe<WatchingEventArg>(p => _statWatchingAggregator.AddWatching(p.Result.ActionData, p.Result.ProjectId));
        }
    }
}