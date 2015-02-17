// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.WindowsAzure.Storage;

namespace Portal.BLL.Concrete.Infrastructure
{
    public sealed class DistributedFileUriProvider : FileUriProvider
    {
        private readonly Uri _requestedUri;

        public DistributedFileUriProvider(CloudStorageAccount account, Uri requestedUri)
            : base(account)
        {
            _requestedUri = requestedUri;
        }

        public override string CreateUri(string fileId)
        {
            string baseUri = base.CreateUri(fileId);

            // Dont change storage emulator hosts and ports
            if (_requestedUri.Port > 80 && _requestedUri.Port < 90 ||
                _requestedUri.Port > 443 && _requestedUri.Port < 460)
            {
                return baseUri;
            }

            var fileUri = new UriBuilder(baseUri)
            {
                Scheme = _requestedUri.Scheme,
                Port = _requestedUri.Scheme == Uri.UriSchemeHttp ? 80 : 443
            };

            // Required azure storage improvements:
            // - use multiple custom blob storage domains: http://feedback.azure.com/forums/217298-storage/suggestions/955579
            // - set ssl certificates for custom domains: http://feedback.azure.com/forums/217298-storage/suggestions/3007732

            return fileUri.Uri.ToString();
        }
    }
}