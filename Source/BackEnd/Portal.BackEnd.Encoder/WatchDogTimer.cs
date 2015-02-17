// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Configuration;
using Portal.BackEnd.Encoder.Interface;
using Wrappers;

namespace Portal.BackEnd.Encoder
{
    public class WatchDogTimer : IWatchDogTimer
    {
        private readonly ThreadingTimerWrapper _timer;
        private readonly IPortalBackendSettings _settings;
        private CancellationTokenSourceWrapper _tokenSource;

        public WatchDogTimer(ThreadingTimerWrapper timer, IPortalBackendSettings settings)
        {
            _timer = timer;
            _settings = settings;
        }

        public bool IsOverflowing { get; private set; }

        public void Reset()
        {
            _timer.Change(_settings.Period, _settings.Period);
        }

        public void Start(CancellationTokenSourceWrapper tokenSource)
        {
            IsOverflowing = false;
            _tokenSource = tokenSource;
            _timer.SetCallback(Callback);
        }

        public void Stop()
        {
            _timer.Dispose();
        }

        private void Callback(object state)
        {
            _tokenSource.Cancel();
            IsOverflowing = true;
        }
    }
}