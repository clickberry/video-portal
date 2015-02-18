// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.Domain.BackendContext.Entity.Base;

namespace Portal.BLL.Concrete.Multimedia.AdjusterParam
{
    public class AudioAdjusterParam : AudioParamBase
    {
        public int AudioChannels { get; set; }

        public bool IsExistAudioStream { get; set; }

        public int AudioSampleRate { get; set; }
    }
}