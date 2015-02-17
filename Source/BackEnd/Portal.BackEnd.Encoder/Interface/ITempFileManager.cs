// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.BackEnd.Encoder.Interface
{
    public interface ITempFileManager
    {
        string GetOriginalTempFilePath();

        string GetEncodingTempFilePath();

        void DeleteAllTempFiles();

        bool ExistsEncodingFile();
    }
}