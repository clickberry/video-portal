// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.DTO.Twitter
{
    /// <summary>
    ///     Twitter status data.
    /// </summary>
    public class TwitterStatus
    {
        public virtual string Message { get; set; }

        public virtual string ScreenshotUrl { get; set; }

        public virtual string Token { get; set; }

        public virtual string TokenSecret { get; set; }
    }
}