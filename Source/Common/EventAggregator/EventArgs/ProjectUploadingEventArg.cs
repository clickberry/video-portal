// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.Domain.ProjectContext;
using Portal.Domain.StatisticContext;

namespace EventAggregator.EventArgs
{
    public class ProjectUploadingEventArg : ActionDataEventArg
    {
        public ProjectUploadingEventArg(DomainActionData actionData, string projectId, string projectName, ProjectType projectType, ProjectSubtype projectSubtype)
            : base(actionData)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            ProjectType = projectType;
            ProjectSubtype = projectSubtype;
        }

        public string ProjectId { get; private set; }

        public string ProjectName { get; private set; }

        public ProjectType ProjectType { get; private set; }

        public ProjectSubtype ProjectSubtype { get; private set; }
    }
}