// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Wrappers.Interface
{
    public interface IProcessAsync
    {
        Task Start(string arguments, Action<string> processData, CancellationTokenWrapper token);
    }
}