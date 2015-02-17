// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Portal.BackEnd.Encoder.Interface;
using Wrappers.Interface;

namespace Portal.BackEnd.Encoder.LocalFileSystem
{
    public class TempFileManager : ITempFileManager
    {
        public const string OriginalFile = "OriginalFile.tmp";
        public const string EncodingFile = "EncodingFile.tmp";

        private readonly IFileSystemWrapper _fileSystem;

        public TempFileManager(IFileSystemWrapper fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public string GetOriginalTempFilePath()
        {
            string tempPath = CreateTempDirectory();
            string tempFilePath = _fileSystem.PathCombine(tempPath, OriginalFile);

            return tempFilePath;
        }

        public string GetEncodingTempFilePath()
        {
            string tempPath = CreateTempDirectory();
            string tempFilePath = _fileSystem.PathCombine(tempPath, EncodingFile);

            return tempFilePath;
        }

        public void DeleteAllTempFiles()
        {
            string tempPath = CreateTempDirectory();
            _fileSystem.DeleteDirectory(tempPath);
        }

        public bool ExistsEncodingFile()
        {
            string tempFilePath = GetEncodingTempFilePath();
            bool isExist = _fileSystem.FileExists(tempFilePath);

            return isExist;
        }

        private string CreateTempDirectory()
        {
            string tempPath = _fileSystem.GetTempPath();
            string newTempPath = _fileSystem.PathCombine(tempPath, Environment.MachineName);
            _fileSystem.CreateDirectory(newTempPath);

            return newTempPath;
        }
    }
}