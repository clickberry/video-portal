// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using AsyncEventAggregator;
using EventAggregator.EventArgs;
using Portal.BLL.Statistics.Aggregator;

namespace EventAggregator.Subscribers
{
    public class ProjectUploadingSubscriber : IEventSubscriber
    {
        private readonly IStatProjectUploadingService _statProjectUploadingAggregator;

        public ProjectUploadingSubscriber(IStatProjectUploadingService statProjectUploadingAggregator)
        {
            _statProjectUploadingAggregator = statProjectUploadingAggregator;
        }

        public Task SubscribeEvent()
        {
            return this.Subscribe<ProjectUploadingEventArg>(
                p => _statProjectUploadingAggregator.AddProjectUploading(
                    p.Result.ActionData,
                    p.Result.ProjectId,
                    p.Result.ProjectName,
                    p.Result.ProjectType,
                    p.Result.ProjectSubtype));
        }
    }
}