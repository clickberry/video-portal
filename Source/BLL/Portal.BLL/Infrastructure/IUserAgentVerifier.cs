// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.BLL.Infrastructure
{
    public interface IUserAgentVerifier
    {
        bool IsMobileDevice(string userAgent);

        bool IsSocialBot(string userAgent);

        bool IsBot(string userAgent);

        Device GetDevice(string userAgent);
    }
}