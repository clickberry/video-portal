// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.Domain.BackendContext.Entity.Base;

namespace Portal.BLL.Concrete.Multimedia.AdjusterParam
{
    public class VideoAdjusterParam : VideoParamBase
    {
        public string FrameRateMode { get; set; }
    }
}