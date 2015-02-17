// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.Domain.BackendContext.Entity.Base
{
    public abstract class VideoParamBase
    {
        public string MediaContainer { get; set; }

        public string VideoCodec { get; set; }

        public int VideoBitrate { get; set; }

        public double FrameRate { get; set; }

        public int KeyFrameRate { get; set; }

        public string VideoProfile { get; set; }

        public double VideoRotation { get; set; }
    }
}