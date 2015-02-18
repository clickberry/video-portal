// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Portal.BackEnd.Encoder.Status;
using Wrappers;

namespace Portal.BackEnd.Encoder.Interface
{
    public interface IFfmpeg
    {
        Task<EncoderStatus> Start(string arguments, CancellationTokenSourceWrapper tokenSource, Action<string> processedData);
    }
}