// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.Domain.EncoderContext;

namespace Portal.BLL.Concrete.Multimedia.Factory
{
    public interface IMultimediaAdjusterParamFactory
    {
        VideoAdjusterParam CreateVideoParam(IVideoMetadata videoMetadata);
        AudioAdjusterParam CreateAudioParam(IVideoMetadata videoMetadata);
    }
}