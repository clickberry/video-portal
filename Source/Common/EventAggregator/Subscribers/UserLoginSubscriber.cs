// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using AsyncEventAggregator;
using EventAggregator.EventArgs;
using Portal.BLL.Statistics.Aggregator;

namespace EventAggregator.Subscribers
{
    public sealed class UserLoginSubscriber : IEventSubscriber
    {
        private readonly IStatUserLoginService _statUserLoginAggregator;

        public UserLoginSubscriber(IStatUserLoginService statUserLoginAggregator)
        {
            _statUserLoginAggregator = statUserLoginAggregator;
        }

        public Task SubscribeEvent()
        {
            return this.Subscribe<UserLoginEventArg>(p => _statUserLoginAggregator.AddUserLogin(p.Result.ActionData));
        }
    }
}