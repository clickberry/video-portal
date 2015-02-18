// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.IO;

namespace FileInformation
{
    /// <summary>
    ///     Receive file information.
    /// </summary>
    public abstract class FileInformationBase
    {
        protected string Header { get; private set; }

        /// <summary>
        ///     Reads file information.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public void Read(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            using (FileStream fileStream = File.OpenRead(fileName))
            {
                if (fileStream.Length <= 20)
                {
                    return;
                }

                var buffer = new byte[20];
                fileStream.Read(buffer, 0, buffer.Length);
                Header = BitConverter.ToString(buffer).Replace("-", string.Empty).ToLowerInvariant();

                Update();
            }
        }

        protected abstract void Update();
    }
}