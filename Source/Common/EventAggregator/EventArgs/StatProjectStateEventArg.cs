// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.Domain.StatisticContext;

namespace EventAggregator.EventArgs
{
    public class StatProjectStateEventArg : ActionDataEventArg
    {
        public StatProjectStateEventArg(DomainActionData actionData, string projectId, string actionType)
            : base(actionData)
        {
            ProjectId = projectId;
            ActionType = actionType;
        }

        public string ProjectId { get; private set; }

        public string ActionType { get; private set; }
    }
}