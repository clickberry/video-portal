// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.Domain.BackendContext.Entity.Base
{
    public abstract class ScreenshotParamBase
    {
        public int ImageWidth { get; set; }

        public int ImageHeight { get; set; }

        public double VideoRotation { get; set; }
    }
}