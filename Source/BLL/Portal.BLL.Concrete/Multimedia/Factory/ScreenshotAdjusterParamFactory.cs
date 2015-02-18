// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.Domain.EncoderContext;
using Portal.Exceptions.Multimedia;

namespace Portal.BLL.Concrete.Multimedia.Factory
{
    public class ScreenshotAdjusterParamFactory : IScreenshotAdjusterParamFactory
    {
        public ScreenshotAdjusterParam CreateScreenshotAdjusterParam(IVideoMetadata videoMetadata)
        {
            CheckVideoStream(videoMetadata.VideoStreamsCount);
            ScreenshotAdjusterParam screenshotAdjusterParam = CreateParam(videoMetadata);

            return screenshotAdjusterParam;
        }

        private ScreenshotAdjusterParam CreateParam(IVideoMetadata videoMetadata)
        {
            return new ScreenshotAdjusterParam
            {
                ImageWidth = videoMetadata.VideoWidth,
                ImageHeight = videoMetadata.VideoHeight,
                Duration = videoMetadata.VideoDuration/1000.0,
                VideoRotation = videoMetadata.VideoRotation
            };
        }

        private void CheckVideoStream(int videoStreamCount)
        {
            if (videoStreamCount != 1)
            {
                throw new AggregateException(new VideoFormatException(ParamType.VideoStreamCount));
            }
        }
    }
}