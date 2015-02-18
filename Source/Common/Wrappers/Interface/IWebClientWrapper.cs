// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Wrappers.Interface
{
    public interface IWebClientWrapper : IDisposable
    {
        void DownloadFile(string address, string fileName);
    }
}