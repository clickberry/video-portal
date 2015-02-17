// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.Calculator;
using Portal.BLL.Concrete.Multimedia.Factory;
using Portal.BLL.Concrete.Multimedia.Lists;
using Portal.Domain.BackendContext.Constant;
using Portal.Domain.EncoderContext;
using Portal.Domain.ProcessedVideoContext;

namespace Portal.BLL.Concrete.Infrastructure.ProcessedEntity
{
    public sealed class ProcessedVideoGenerator : IProcessedEntityGenerator<DomainProcessedVideo>
    {
        private readonly IMultimediaAdjusterParamFactory _multimediaAdjusterParamFactory;
        private readonly IProcessedVideoList _processedVideoList;
        private readonly IResolutionCalculator _resolutionCalculator;

        public ProcessedVideoGenerator(IResolutionCalculator resolutionCalculator, IMultimediaAdjusterParamFactory multimediaAdjusterParamFactory, IProcessedVideoList processedVidioList)
        {
            _resolutionCalculator = resolutionCalculator;
            _multimediaAdjusterParamFactory = multimediaAdjusterParamFactory;
            _processedVideoList = processedVidioList;
        }

        public List<DomainProcessedVideo> Generate(IVideoMetadata videoMetadata)
        {
            int width = videoMetadata.VideoWidth;
            int height = videoMetadata.VideoHeight;

            List<IVideoSize> sizeList = _resolutionCalculator.Calculate(width, height);
            VideoAdjusterParam videoParam = _multimediaAdjusterParamFactory.CreateVideoParam(videoMetadata);
            AudioAdjusterParam audioParam = _multimediaAdjusterParamFactory.CreateAudioParam(videoMetadata);
            IEnumerable<DomainProcessedVideo> mp4ProcessedVideos = _processedVideoList.CreateProcessedVideos(videoParam, audioParam, MetadataConstant.Mp4Container, sizeList, ContentType.Mp4Content);
            IEnumerable<DomainProcessedVideo> webmProcessedVideos = _processedVideoList.CreateProcessedVideos(videoParam, audioParam, MetadataConstant.WebmContainer, sizeList, ContentType.WebmContent);

            var list = new List<DomainProcessedVideo>();
            list.AddRange(mp4ProcessedVideos);
            list.AddRange(webmProcessedVideos);

            return list;
        }
    }
}