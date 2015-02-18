// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.BLL.Infrastructure
{
    public interface IStringEncryptor
    {
        string EncryptString(string clearText);
        string DecryptString(string encryptedText);
    }
}