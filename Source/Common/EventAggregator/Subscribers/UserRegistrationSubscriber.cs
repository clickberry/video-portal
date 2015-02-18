// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading.Tasks;
using AsyncEventAggregator;
using EventAggregator.EventArgs;
using Portal.BLL.Statistics.Aggregator;

namespace EventAggregator.Subscribers
{
    public class UserRegistrationSubscriber : IEventSubscriber
    {
        private readonly IStatUserRegistrationService _statUserRegistrationAggregator;

        public UserRegistrationSubscriber(IStatUserRegistrationService statUserRegistrationAggregator)
        {
            _statUserRegistrationAggregator = statUserRegistrationAggregator;
        }

        public Task SubscribeEvent()
        {
            return this.Subscribe<UserRegistrationEventArg>(p => _statUserRegistrationAggregator.AddUserRegistration(p.Result.ActionData));
        }
    }
}