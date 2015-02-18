// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Wrappers;

namespace Portal.BackEnd.Encoder.Interface
{
    public interface IWatchDogTimer
    {
        bool IsOverflowing { get; }

        void Reset();
        void Start(CancellationTokenSourceWrapper tokenSource);
        void Stop();
    }
}