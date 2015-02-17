// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.Domain.ProcessedVideoContext;

namespace Portal.BLL.Concrete.Multimedia.Builder
{
    public interface IProcessedScreenshotBuilder
    {
        DomainProcessedScreenshot BuildProcessedScreenshot(ScreenshotAdjusterParam screenshotAdjusterParam, string imageFormat, string contentType);
    }
}