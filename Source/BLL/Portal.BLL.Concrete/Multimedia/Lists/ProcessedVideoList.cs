// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.Builder;
using Portal.BLL.Concrete.Multimedia.Calculator;
using Portal.Domain.ProcessedVideoContext;

namespace Portal.BLL.Concrete.Multimedia.Lists
{
    public class ProcessedVideoList : IProcessedVideoList
    {
        private readonly IProcessedVideoBuilder _processedVideoBuilder;

        public ProcessedVideoList(IProcessedVideoBuilder processedVideoBuilder)
        {
            _processedVideoBuilder = processedVideoBuilder;
        }

        public IEnumerable<DomainProcessedVideo> CreateProcessedVideos(VideoAdjusterParam videoAdjusterParam, AudioAdjusterParam audioAdjusterParam, string newContainer, List<IVideoSize> sizeList,
            string contentType)
        {
            return sizeList.Select(videoSize => _processedVideoBuilder.BuildProcessedVideo(videoAdjusterParam, audioAdjusterParam, newContainer, videoSize, contentType));
        }
    }
}