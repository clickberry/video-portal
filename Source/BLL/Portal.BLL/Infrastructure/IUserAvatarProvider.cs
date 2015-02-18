// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.Domain.ProfileContext;

namespace Portal.BLL.Infrastructure
{
    public interface IUserAvatarProvider
    {
        string GetAvatar(DomainUser user, string scheme = "http");

        string GetAvatar(string email, string scheme = "http");
    }
}