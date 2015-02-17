// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.Domain.AccountContext;

namespace Portal.BLL.Services
{
    public interface IRecoveryLinkService
    {
        string CreateRecoveryLinkText(RecoveryLink link, string linkRoot);
        RecoveryLink DecryptParameters(string expires, string id);
    }
}