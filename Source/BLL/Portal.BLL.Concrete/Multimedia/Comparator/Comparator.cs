// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.Calculator;
using Portal.Domain.BackendContext.Constant;
using Portal.Domain.BackendContext.Entity;

namespace Portal.BLL.Concrete.Multimedia.Comparator
{
    public class Comparator : IComparator
    {
        public bool VideoParamCompare(VideoAdjusterParam videoAdjusterParam, VideoParam videoParam, string newContainer, IVideoSize videoSize)
        {
            return videoAdjusterParam.VideoCodec == videoParam.VideoCodec
                   && ((newContainer == MetadataConstant.Mp4Container && videoAdjusterParam.VideoProfile == videoParam.VideoProfile) || (MetadataConstant.Mp4Container != newContainer))
                   && videoAdjusterParam.VideoBitrate == videoParam.VideoBitrate
                   && videoSize.Width == videoParam.VideoWidth
                   && videoSize.Height == videoParam.VideoHeight
                   && videoAdjusterParam.FrameRate == videoParam.FrameRate
                   && videoAdjusterParam.KeyFrameRate == videoParam.KeyFrameRate;
        }

        public bool AudioParamCompare(AudioAdjusterParam audioAdjusterParam, AudioParam audioParam)
        {
            return audioAdjusterParam.AudioCodec == audioParam.AudioCodec
                   && audioAdjusterParam.AudioBitrate == audioParam.AudioBitrate;
        }
    }
}