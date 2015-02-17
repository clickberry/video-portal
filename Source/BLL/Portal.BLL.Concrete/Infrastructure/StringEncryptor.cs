// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Portal.BLL.Infrastructure;

namespace Portal.BLL.Concrete.Infrastructure
{
    public class StringEncryptor : IStringEncryptor
    {
        public string EncryptString(string clearText)
        {
            byte[] clearTextBytes = Encoding.UTF8.GetBytes(clearText);

            SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();

            var ms = new MemoryStream();
            byte[] rgbIv = Encoding.ASCII.GetBytes("sttjvlzmdalyglrj");
            byte[] key = Encoding.ASCII.GetBytes("ooxilkqzzhczfeutlgbskdvqpzivmfuo");
            var cs = new CryptoStream(ms, rijn.CreateEncryptor(key, rgbIv),
                CryptoStreamMode.Write);

            cs.Write(clearTextBytes, 0, clearTextBytes.Length);

            cs.Close();

            return HttpServerUtility.UrlTokenEncode(ms.ToArray());
        }

        public string DecryptString(string encryptedText)
        {
            byte[] encryptedTextBytes = HttpServerUtility.UrlTokenDecode(encryptedText);

            if (encryptedTextBytes == null)
            {
                return null;
            }

            var ms = new MemoryStream();

            SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();


            byte[] rgbIv = Encoding.ASCII.GetBytes("sttjvlzmdalyglrj");
            byte[] key = Encoding.ASCII.GetBytes("ooxilkqzzhczfeutlgbskdvqpzivmfuo");

            var cs = new CryptoStream(ms, rijn.CreateDecryptor(key, rgbIv),
                CryptoStreamMode.Write);

            cs.Write(encryptedTextBytes, 0, encryptedTextBytes.Length);

            cs.Close();

            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}