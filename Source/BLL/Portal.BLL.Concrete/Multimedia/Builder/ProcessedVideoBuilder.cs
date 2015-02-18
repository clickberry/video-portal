// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.Adjusters;
using Portal.BLL.Concrete.Multimedia.Calculator;
using Portal.BLL.Concrete.Multimedia.Comparator;
using Portal.Domain.BackendContext.Entity;
using Portal.Domain.ProcessedVideoContext;

namespace Portal.BLL.Concrete.Multimedia.Builder
{
    public class ProcessedVideoBuilder : IProcessedVideoBuilder
    {
        private readonly IAudioAdjuster _audioAdjuster;
        private readonly IComparator _comparator;
        private readonly IVideoAdjuster _videoAdjuster;

        public ProcessedVideoBuilder(IVideoAdjuster videoAdjuster, IAudioAdjuster audioAdjuster, IComparator comparator)
        {
            _videoAdjuster = videoAdjuster;
            _audioAdjuster = audioAdjuster;
            _comparator = comparator;
        }

        public DomainProcessedVideo BuildProcessedVideo(VideoAdjusterParam videoAdjusterParam, AudioAdjusterParam audioAdjusterParam, string newContainer, IVideoSize videoSize, string contentType)
        {
            VideoParam videoParam = _videoAdjuster.AdjustVideoParam(videoAdjusterParam, newContainer, videoSize);
            AudioParam audioParam = _audioAdjuster.AdjustAudioParam(audioAdjusterParam, newContainer, videoSize);
            bool isVideoCopy = _comparator.VideoParamCompare(videoAdjusterParam, videoParam, newContainer, videoSize);
            bool isAudioCopy = _comparator.AudioParamCompare(audioAdjusterParam, audioParam);
            DomainProcessedVideo processedVideo = CreateProcessedVideo(videoParam, audioParam, isVideoCopy, isAudioCopy, contentType);
            return processedVideo;
        }

        private DomainProcessedVideo CreateProcessedVideo(VideoParam videoParam, AudioParam audioParam, bool isVideoCopy, bool isAudioCopy, string contentType)
        {
            return new DomainProcessedVideo
            {
                VideoParam = videoParam,
                AudioParam = audioParam,
                IsVideoCopy = isVideoCopy,
                IsAudioCopy = isAudioCopy,
                OutputFormat = String.Format("{0}x{1}", videoParam.MediaContainer, videoParam.VideoWidth),
                ContentType = contentType,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
                Started = DateTime.UtcNow,
                Completed = DateTime.UtcNow
            };
        }
    }
}