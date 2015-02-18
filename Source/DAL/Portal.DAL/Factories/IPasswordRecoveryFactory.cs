// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Factories
{
    public interface IPasswordRecoveryFactory
    {
        PasswordRecoveryEntity CreateDefault(string userId, string linkData, string email, DateTime expirationDate);
        PasswordRecoveryEntity Create(string appName, string linkText, string email, DateTime expirationDate, bool isConfirmed, DateTime created, DateTime modified);
    }
}