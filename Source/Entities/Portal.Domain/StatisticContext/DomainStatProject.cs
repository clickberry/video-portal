// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.Domain.StatisticContext
{
    public class DomainStatProject
    {
        protected DomainStatProject()
        {
        }

        public DomainStatProject(DomainStatUser user, string projectId, string projectName, string projectType, string projectSubtype)
        {
            User = user;
            ProjectId = projectId;
            ProjectName = projectName;
            ProjectType = projectType;
            ProjectSubtype = projectSubtype;
        }

        public DomainStatUser User { get; protected set; }

        public string ProjectId { get; protected set; }

        public string ProjectName { get; protected set; }

        public string ProjectType { get; protected set; }

        public string ProjectSubtype { get; protected set; }
    }
}