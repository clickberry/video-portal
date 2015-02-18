// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.Calculator;
using Portal.Domain.ProcessedVideoContext;

namespace Portal.BLL.Concrete.Multimedia.Lists
{
    public interface IProcessedVideoList
    {
        IEnumerable<DomainProcessedVideo> CreateProcessedVideos(VideoAdjusterParam videoAdjusterParam, AudioAdjusterParam audioAdjusterParam, string newContainer, List<IVideoSize> sizeList,
            string contentType);
    }
}