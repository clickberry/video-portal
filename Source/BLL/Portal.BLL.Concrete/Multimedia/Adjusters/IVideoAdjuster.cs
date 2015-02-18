// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.Calculator;
using Portal.Domain.BackendContext.Entity;

namespace Portal.BLL.Concrete.Multimedia.Adjusters
{
    public interface IVideoAdjuster
    {
        VideoParam AdjustVideoParam(VideoAdjusterParam videoAdjusterParam, string mediaContainer, IVideoSize videoSize);
    }
}