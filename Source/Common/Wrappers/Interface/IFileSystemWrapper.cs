// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Wrappers.Interface
{
    public interface IFileSystemWrapper
    {
        void CreateDirectory(string path);
        bool DirectoryExists(string path);
        void DeleteDirectory(string path);
        void DeleteFile(string filePath);

        bool FileExists(string path);

        string GetTempPath();
        string GetTempFilePath();
        string PathCombine(string path1, string path2);
    }
}