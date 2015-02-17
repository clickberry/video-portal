// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.Adjusters;
using Portal.Domain.BackendContext.Entity;
using Portal.Domain.ProcessedVideoContext;

namespace Portal.BLL.Concrete.Multimedia.Builder
{
    public class ProcessdScreenshotBuilder : IProcessedScreenshotBuilder
    {
        private readonly IScreenshotAdjuster _screenshotAdjuster;

        public ProcessdScreenshotBuilder(IScreenshotAdjuster screenshotAdjuster)
        {
            _screenshotAdjuster = screenshotAdjuster;
        }

        public DomainProcessedScreenshot BuildProcessedScreenshot(ScreenshotAdjusterParam screenshotAdjusterParam, string imageFormat, string contentType)
        {
            ScreenshotParam screenshotParam = _screenshotAdjuster.AdjustScreenshotParam(screenshotAdjusterParam);
            DomainProcessedScreenshot processedScreenshot = CreateProcessedScreenshot(screenshotParam, imageFormat, contentType);

            return processedScreenshot;
        }

        private DomainProcessedScreenshot CreateProcessedScreenshot(ScreenshotParam screenshotParam, string imageParam, string contentType)
        {
            return new DomainProcessedScreenshot
            {
                ImageFormat = imageParam,
                ScreenshotParam = screenshotParam,
                ContentType = contentType,
                Created = DateTime.UtcNow,
                Started = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
                Completed = DateTime.UtcNow
            };
        }
    }
}