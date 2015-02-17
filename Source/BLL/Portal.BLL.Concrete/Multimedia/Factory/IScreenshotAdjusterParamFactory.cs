// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.Domain.EncoderContext;

namespace Portal.BLL.Concrete.Multimedia.Factory
{
    public interface IScreenshotAdjusterParamFactory
    {
        ScreenshotAdjusterParam CreateScreenshotAdjusterParam(IVideoMetadata videoMetadata);
    }
}