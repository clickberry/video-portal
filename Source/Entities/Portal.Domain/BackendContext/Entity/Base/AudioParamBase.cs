// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.Domain.BackendContext.Entity.Base
{
    public abstract class AudioParamBase
    {
        public string AudioCodec { get; set; }

        public int AudioBitrate { get; set; }
    }
}