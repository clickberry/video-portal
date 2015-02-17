// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.Domain.BackendContext.Entity
{
    public class VideoEncodeParam
    {
        public VideoParam VideoParam { get; set; }

        public AudioParam AudioParam { get; set; }

        public bool IsVideoCopy { get; set; }

        public bool IsAudioCopy { get; set; }
    }
}