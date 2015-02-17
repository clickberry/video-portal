// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;
using Wrappers.Interface;

namespace Wrappers.Implementation
{
    public class WebClientWrapper : IWebClientWrapper
    {
        private readonly WebClient _webClient;

        public WebClientWrapper(string cookie)
        {
            _webClient = new WebClient();
            _webClient.Headers.Add(HttpRequestHeader.Cookie, cookie);
        }

        public void DownloadFile(string address, string fileName)
        {
            _webClient.DownloadFile(address, fileName);
        }

        public void Dispose()
        {
            _webClient.Dispose();
        }
    }
}