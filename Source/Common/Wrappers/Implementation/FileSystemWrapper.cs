// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Wrappers.Interface;

namespace Wrappers.Implementation
{
    public class FileSystemWrapper : IFileSystemWrapper
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public void DeleteDirectory(string path)
        {
            IOException exception = null;
            int attemptsCount = 0;
            while (attemptsCount < 3)
            {
                try
                {
                    Directory.Delete(path, true);
                    return;
                }
                catch (IOException ex)
                {
                    exception = ex;
                }
                attemptsCount++;
            }
            throw new Exception(String.Format("Delete '{0}' is failed. Attempts count is: {1}", path, attemptsCount), exception);
        }

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        public string GetTempPath()
        {
            return Path.GetTempPath();
        }

        public string GetTempFilePath()
        {
            return Path.GetTempFileName();
        }

        public string PathCombine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}