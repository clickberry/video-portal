// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.Domain.StatisticContext;

namespace EventAggregator.EventArgs
{
    public class ProjectDeletionEventArg : ActionDataEventArg
    {
        public ProjectDeletionEventArg(DomainActionData actionData, string projectId)
            : base(actionData)
        {
            ProjectId = projectId;
        }

        public string ProjectId { get; private set; }
    }
}