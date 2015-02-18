// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Security.Cryptography;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.Domain.AccountContext;
using Portal.Exceptions.CRUD;

namespace Portal.BLL.Concrete.Services
{
    public class RecoveryLinkService : IRecoveryLinkService
    {
        private readonly IStringEncryptor _stringEncryptor;

        public RecoveryLinkService(IStringEncryptor stringEncryptor)
        {
            _stringEncryptor = stringEncryptor;
        }

        public string CreateRecoveryLinkText(RecoveryLink link, string linkRoot)
        {
            long ticks = (link.ExpirationDate - DateTime.MinValue).Ticks;
            string expiresEncrypted = _stringEncryptor.EncryptString(ticks.ToString(CultureInfo.InvariantCulture));
            string userIdEncrypted = _stringEncryptor.EncryptString(link.Id);

            return String.Format("{0}/?e={1}&i={2}", linkRoot, expiresEncrypted, userIdEncrypted);
        }

        public RecoveryLink DecryptParameters(string expires, string id)
        {
            DateTime dt = DateTime.MinValue;
            string uid = "";
            try
            {
                long ticks = long.Parse(_stringEncryptor.DecryptString(expires));
                dt += TimeSpan.FromTicks(ticks);
                uid = _stringEncryptor.DecryptString(id);
            }
            catch (ArgumentNullException)
            {
            }
            catch (OverflowException)
            {
            }
            catch (FormatException)
            {
            }
            catch (CryptographicException)
            {
                throw new BadRequestException();
            }
            return new RecoveryLink { ExpirationDate = dt, Id = uid };
        }
    }
}