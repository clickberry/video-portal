// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.IO;
using Wrappers.Interface;

namespace Wrappers.Implementation
{
    public class FileWrapper : IFileWrapper
    {
        public Stream OpenRead(string filePath)
        {
            return File.OpenRead(filePath);
        }

        public Stream OpenWrite(string filePath)
        {
            return File.OpenWrite(filePath);
        }
    }
}