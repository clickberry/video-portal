// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.DTO.Twitter
{
    /// <summary>
    ///     Twitter request data.
    /// </summary>
    public class TwitterRequest
    {
        public virtual string Token { get; set; }

        public virtual string TokenSecret { get; set; }
    }
}