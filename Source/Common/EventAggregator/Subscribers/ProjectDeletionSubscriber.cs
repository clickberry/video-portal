// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading.Tasks;
using AsyncEventAggregator;
using EventAggregator.EventArgs;
using Portal.BLL.Statistics.Aggregator;

namespace EventAggregator.Subscribers
{
    public class ProjectDeletionSubscriber : IEventSubscriber
    {
        private readonly IStatProjectDeletionService _statProjectDeletionAggregator;

        public ProjectDeletionSubscriber(IStatProjectDeletionService statProjectDeletionAggregator)
        {
            _statProjectDeletionAggregator = statProjectDeletionAggregator;
        }

        public Task SubscribeEvent()
        {
            return this.Subscribe<ProjectDeletionEventArg>(p => _statProjectDeletionAggregator.AddProjectDeletion(p.Result.ActionData, p.Result.ProjectId));
        }
    }
}