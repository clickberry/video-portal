// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Portal.Domain;

namespace Portal.BLL.Services
{
    public interface ICsvPublishService
    {
        Uri GetAccessCsvUri(string fileName);

        Task PublishAsync(DataQueryOptions filter, string fileName, CancellationTokenSource cancellationTokenSource);
    }
}