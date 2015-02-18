// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Portal.Domain.BackendContext.Entity;
using Portal.Domain.BackendContext.Entity.Base;
using Portal.Domain.ProcessedVideoContext.States;

namespace Portal.Domain.ProcessedVideoContext
{
    public sealed class DomainProcessedScreenshot : ScreenshotEncodeParam, IProcessedEntity, IEncodeData
    {
        public DomainProcessedScreenshot()
        {
        }

        public DomainProcessedScreenshot(TaskState processingState)
        {
            State = processingState;
        }

        public string ObjectId { get; set; }

        public string ProjectId { get; set; }

        public string SourceFileId { get; set; }

        public string TaskId { get; set; }

        public DateTime Created { get; set; }

        public DateTime Started { get; set; }

        public DateTime Modified { get; set; }

        public DateTime Completed { get; set; }

        public string UserId { get; set; }

        public string DestinationFileId { get; set; }

        public string ContentType { get; set; }

        public double Progress { get; set; }

        public ProcessedEntityType EntityType
        {
            get { return ProcessedEntityType.Screenshot; }
        }

        public TaskState State { get; private set; }

        public int AttemptsCount { get; set; }

        public void SetState(IProcessingState state)
        {
            State = state.GetState(this);
            Modified = DateTime.UtcNow;
        }

        public void SetProgress(double value)
        {
            if (value < 0 || value > 100)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            Progress = value;
            Modified = DateTime.UtcNow;
        }
    }
}