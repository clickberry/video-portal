// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Portal.Domain.EncoderContext;

namespace Portal.Domain.ProcessedVideoContext
{
    public class ProcessedMediaModel
    {
        public ProcessedMediaModel(List<DomainProcessedVideo> domainProcessedVideos,
            List<DomainProcessedScreenshot> domainProcessedScreenshots,
            VideoMetadata videoMetadata)
        {
            DomainProcessedVideos = domainProcessedVideos;
            DomainProcessedScreenshots = domainProcessedScreenshots;
            VideoMetadata = videoMetadata;
        }

        public VideoMetadata VideoMetadata { get; private set; }

        public List<DomainProcessedVideo> DomainProcessedVideos { get; private set; }

        public List<DomainProcessedScreenshot> DomainProcessedScreenshots { get; private set; }
    }
}