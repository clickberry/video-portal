// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Portal.DAL.Entities.Table;
using Portal.DAL.Factories;

namespace Portal.DAL.Infrastructure.Factories
{
    public class PasswordRecoveryFactory : IPasswordRecoveryFactory
    {
        public PasswordRecoveryEntity CreateDefault(string userId, string linkText, string email, DateTime expirationDate)
        {
            return new PasswordRecoveryEntity
            {
                UserId = userId,
                LinkData = linkText,
                Email = email,
                Expires = expirationDate,
                IsConfirmed = false,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };
        }

        public PasswordRecoveryEntity Create(string appName, string linkText, string email, DateTime expirationDate,
            bool isConfirmed, DateTime created, DateTime modified)
        {
            return new PasswordRecoveryEntity
            {
                UserId = appName,
                IsConfirmed = isConfirmed,
                Created = created,
                Email = email,
                Expires = expirationDate,
                Modified = modified,
                LinkData = linkText
            };
        }
    }
}