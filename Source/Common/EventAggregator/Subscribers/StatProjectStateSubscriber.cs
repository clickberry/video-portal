// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using AsyncEventAggregator;
using EventAggregator.EventArgs;
using Portal.BLL.Statistics.Aggregator;

namespace EventAggregator.Subscribers
{
    public class StatProjectStateSubscriber : IEventSubscriber
    {
        private readonly IStatProjectStateService _statProjectStateService;

        public StatProjectStateSubscriber(IStatProjectStateService statProjectStateService)
        {
            _statProjectStateService = statProjectStateService;
        }

        public Task SubscribeEvent()
        {
            return this.Subscribe<StatProjectStateEventArg>(
                p => _statProjectStateService.AddProjectState(
                    p.Result.ActionData,
                    p.Result.ProjectId,
                    p.Result.ActionType));
        }
    }
}