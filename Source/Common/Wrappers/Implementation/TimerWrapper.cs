// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Timers;
using Wrappers.Interface;

namespace Wrappers.Implementation
{
    public class TimerWrapper : ITimerWrapper
    {
        private readonly Timer _timer = new Timer();

        public double Interval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        public Action Callback { get; set; }

        public void Start()
        {
            _timer.Elapsed += (s, e) => Callback();
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Close()
        {
            _timer.Close();
        }
    }
}