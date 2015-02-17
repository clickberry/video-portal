// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using System.Text;
using Portal.BLL.Infrastructure;

namespace Portal.BLL.Concrete.Infrastructure
{
    public sealed class CryptoService : ICryptoService
    {
        #region ICryptoService Members

        public string GenerateSalt()
        {
            return BitConverter.ToString(Guid.NewGuid().ToByteArray()).Replace("-", string.Empty);
        }

        public string EncodePassword(string password, string salt)
        {
            return BitConverter.ToString(new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(password + salt))).Replace("-", string.Empty);
        }

        #endregion
    }
}