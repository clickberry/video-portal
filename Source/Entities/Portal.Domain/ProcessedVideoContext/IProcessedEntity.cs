// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Portal.Domain.ProcessedVideoContext.States;

namespace Portal.Domain.ProcessedVideoContext
{
    public interface IProcessedEntity
    {
        string ProjectId { get; set; }

        string SourceFileId { get; set; }

        string TaskId { get; set; }

        DateTime Created { get; set; }

        DateTime Started { get; set; }

        DateTime Modified { get; set; }

        DateTime Completed { get; set; }

        string UserId { get; set; }

        string DestinationFileId { get; set; }

        string ContentType { get; set; }

        double Progress { get; }

        ProcessedEntityType EntityType { get; }

        TaskState State { get; }

        /// <summary>
        ///     Gets or sets a processesing attempts count.
        /// </summary>
        int AttemptsCount { get; set; }

        /// <summary>
        ///     Trying to change state.
        /// </summary>
        /// <param name="state">Processing state.</param>
        /// <exception cref="InvalidOperationException">Throws when invalid state was passed.</exception>
        void SetState(IProcessingState state);

        void SetProgress(double value);
    }
}