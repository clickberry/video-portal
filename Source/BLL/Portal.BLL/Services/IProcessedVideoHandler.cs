// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.Domain.ProcessedVideoContext;
using Portal.Domain.ProjectContext;

namespace Portal.BLL.Services
{
    public interface IProcessedVideoHandler
    {
        /// <summary>
        ///     Adds a video.
        /// </summary>
        /// <param name="projectId">Project identifier.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="entity">Video.</param>
        /// <param name="processedMediaModel">Model.</param>
        Task AddVideo(string projectId, string userId, DomainVideo entity, ProcessedMediaModel processedMediaModel);

        /// <summary>
        ///     Removes a video.
        /// </summary>
        /// <param name="projectId">Project identifier.</param>
        Task RemoveVideo(string projectId);
    }
}