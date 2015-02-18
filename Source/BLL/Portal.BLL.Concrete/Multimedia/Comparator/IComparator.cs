// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.Calculator;
using Portal.Domain.BackendContext.Entity;

namespace Portal.BLL.Concrete.Multimedia.Comparator
{
    public interface IComparator
    {
        bool VideoParamCompare(VideoAdjusterParam videoAdjusterParam, VideoParam videoParam, string newContainer, IVideoSize videoSize);
        bool AudioParamCompare(AudioAdjusterParam audioAdjusterParam, AudioParam audioParam);
    }
}