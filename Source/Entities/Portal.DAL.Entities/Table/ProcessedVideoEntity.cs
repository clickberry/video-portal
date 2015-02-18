// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    [CollectionName("ProcessedVideo")]
    public sealed class ProcessedVideoEntity : Entity
    {
        public string SourceFileId { get; set; }

        public string OutputFormat { get; set; }

        public string UserId { get; set; }

        public string TaskId { get; set; }

        public DateTime Created { get; set; }

        public DateTime Started { get; set; }

        public DateTime Modified { get; set; }

        public DateTime Completed { get; set; }

        public string MediaContainer { get; set; }

        public string VideoCodec { get; set; }

        public int VideoBitrate { get; set; }

        public double FrameRate { get; set; }

        public int KeyFrameRate { get; set; }

        public int VideoWidth { get; set; }

        public int VideoHeight { get; set; }

        public string VideoProfile { get; set; }

        public string AudioCodec { get; set; }

        public int AudioBitrate { get; set; }

        public double Progress { get; set; }

        public string DestinationFileId { get; set; }

        public string ContentType { get; set; }

        public int ProcessingState { get; set; }

        public int AttemptsCount { get; set; }

        public bool IsAudioCopy { get; set; }

        public bool IsVideoCopy { get; set; }

        public double VideoRotation { get; set; }


        [Obsolete("For backward compatibility only")]
        public string OriginalVideoFileHash { get; set; }

        [Obsolete("For backward compatibility only")]
        public string DestinationFileHash { get; set; }
    }
}