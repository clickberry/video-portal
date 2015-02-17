// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.Domain.AccountContext;
using Portal.Domain.ProfileContext;

namespace Portal.BLL.Services
{
    public interface IPasswordRecoveryService
    {
        Task SendNewRecoveryMail(DomainUser user, string validationPath);

        RecoveryLink GetLink(string encryptedExpiration, string encryptedLinkId);

        Task ChangePassword(RecoveryLink recoveryLink, string newPassword);

        Task<bool> CheckIfLinkIsValid(RecoveryLink recoveryLink);
    }
}