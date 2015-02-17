// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    [CollectionName("ProcessedScreenshot")]
    public sealed class ProcessedScreenshotEntity : Entity
    {
        public string SourceFileId { get; set; }

        public string TimeOffset { get; set; }

        public string UserId { get; set; }

        public string TaskId { get; set; }

        public DateTime Created { get; set; }

        public DateTime Started { get; set; }

        public DateTime Modified { get; set; }

        public DateTime Completed { get; set; }

        public string DestinationFileId { get; set; }

        public string ContentType { get; set; }

        public double Progress { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int ProcessingState { get; set; }

        public string ImageFormat { get; set; }

        public int AttemptsCount { get; set; }

        public double VideoRotation { get; set; }


        [Obsolete("For backward compatibility only")]
        public string VideoFileHash { get; set; }

        [Obsolete("For backward compatibility only")]
        public string DestinationFileHash { get; set; }
    }
}