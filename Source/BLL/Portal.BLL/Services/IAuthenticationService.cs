// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.Domain.ProfileContext;

namespace Portal.BLL.Services
{
    public interface IAuthenticationService
    {
        Task<string> SetUserAsync(DomainUser user, TokenData tokenData, bool isPersistent = false);

        Task SetDataAsync(string data);

        void UpdateIdentityClaims(DomainUser user);

        void Clear();
    }
}