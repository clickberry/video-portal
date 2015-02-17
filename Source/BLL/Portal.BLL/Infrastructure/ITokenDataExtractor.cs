// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.Domain.ProfileContext;
using Portal.DTO.User;

namespace Portal.BLL.Infrastructure
{
    public interface ITokenDataExtractor
    {
        TokenData Get(IpData data);
    }
}