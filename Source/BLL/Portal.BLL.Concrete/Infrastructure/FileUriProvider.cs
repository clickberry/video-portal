// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.WindowsAzure.Storage;
using Portal.BLL.Infrastructure;

namespace Portal.BLL.Concrete.Infrastructure
{
    /// <summary>
    ///     Handles tiny URIs.
    /// </summary>
    public class FileUriProvider : IFileUriProvider
    {
        private readonly StorageUri _blobUri;
        private readonly TinyUrl _tinyUrl;

        public FileUriProvider(CloudStorageAccount account)
        {
            _tinyUrl = new TinyUrl();
            _blobUri = account.BlobStorageUri;
        }

        public virtual string CreateUri(string fileId)
        {
            var fileUri = new UriBuilder(_blobUri.PrimaryUri);
            var filePath = GetFilePath(fileId);
            fileUri.Path = fileUri.Path == "/" ? filePath : fileUri.Path + filePath;
            return fileUri.Uri.ToString();
        }

        public string Decompress(string fileId)
        {
            return _tinyUrl.Decompress(fileId).ToString();
        }

        protected string GetFilePath(string fileId)
        {
            return string.Format("/{0}/file", fileId);
        }
    }
}