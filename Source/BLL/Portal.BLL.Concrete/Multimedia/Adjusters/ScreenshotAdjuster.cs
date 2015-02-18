// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.AdjustmentParameters;
using Portal.Domain.BackendContext.Entity;
using Portal.Exceptions.Multimedia;

namespace Portal.BLL.Concrete.Multimedia.Adjusters
{
    public class ScreenshotAdjuster : IScreenshotAdjuster
    {
        private readonly IAdjustmentScreenshotMetadata _adjustmentScreenshotMetadata;

        public ScreenshotAdjuster(IAdjustmentScreenshotMetadata adjustmentScreenshotMetadata)
        {
            _adjustmentScreenshotMetadata = adjustmentScreenshotMetadata;
        }

        public ScreenshotParam AdjustScreenshotParam(ScreenshotAdjusterParam screenshotAdjusterParam)
        {
            var exceptionList = new List<VideoFormatException>();

            int width = AdjustScreenshotWidth(screenshotAdjusterParam.ImageWidth, exceptionList);
            int height = AdjustScreenshotHeight(screenshotAdjusterParam.ImageHeight, exceptionList);
            double timeOffset = _adjustmentScreenshotMetadata.AdjustScreenshotTimeOffset(screenshotAdjusterParam.Duration);
            ScreenshotParam adjustScreenshotParam = CreateParam(width, height, timeOffset, screenshotAdjusterParam.VideoRotation);

            CheckForException(exceptionList);

            return adjustScreenshotParam;
        }

        private ScreenshotParam CreateParam(int width, int height, double timeOffset, double videoRotation)
        {
            return new ScreenshotParam
            {
                ImageWidth = width,
                ImageHeight = height,
                TimeOffset = timeOffset,
                VideoRotation = videoRotation
            };
        }

        private void CheckForException(ICollection<VideoFormatException> exceptionList)
        {
            if (exceptionList.Count > 0)
            {
                throw new AggregateException(exceptionList);
            }
        }

        private int AdjustScreenshotWidth(int videoWidth, ICollection<VideoFormatException> exceptionList)
        {
            int adjustWidth = 0;
            try
            {
                adjustWidth = _adjustmentScreenshotMetadata.AdjustScreenshotWidth(videoWidth);
            }
            catch (VideoFormatException ex)
            {
                exceptionList.Add(ex);
            }
            return adjustWidth;
        }

        private int AdjustScreenshotHeight(int videoHeight, ICollection<VideoFormatException> exceptionList)
        {
            int adjustHeight = 0;
            try
            {
                adjustHeight = _adjustmentScreenshotMetadata.AdjustScreenshotHeight(videoHeight);
            }
            catch (VideoFormatException ex)
            {
                exceptionList.Add(ex);
            }
            return adjustHeight;
        }
    }
}