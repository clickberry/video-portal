// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.Domain.BackendContext.Entity.Base
{
    public abstract class ScreenshotParamBase
    {
        public int ImageWidth { get; set; }

        public int ImageHeight { get; set; }

        public double VideoRotation { get; set; }
    }
}