// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.BLL.Infrastructure
{
    public interface IUserUriProvider
    {
        Uri BaseUri { get; set; }

        string GetUri(string userId);
    }
}