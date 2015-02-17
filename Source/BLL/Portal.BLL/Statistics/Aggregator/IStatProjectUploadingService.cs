// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.Domain.ProjectContext;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Statistics.Aggregator
{
    public interface IStatProjectUploadingService
    {
        Task AddProjectUploading(DomainActionData actionData, string projectId, string projectName, ProjectType projectType, ProjectSubtype projectSubtype);
    }
}