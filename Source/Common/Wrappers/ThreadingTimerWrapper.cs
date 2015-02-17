// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wrappers
{
    public class ThreadingTimerWrapper : IDisposable
    {
        private readonly object _lockObj = new object();
        private bool _isDispose;
        private Timer _timer;

        public ThreadingTimerWrapper()
        {
            Period = 10000;
        }

        #region IDisposable Members

        public virtual void Dispose()
        {
            lock (_lockObj)
            {
                _timer.Dispose();
                _isDispose = true;
            }
        }

        #endregion

        public int Period { get; set; }

        /// <summary>
        ///     For use internal wrappers only.
        /// </summary>
        public Timer Timer
        {
            get { return _timer; }
            set { _timer = value; }
        }

        public virtual void SetCallback(Func<Task> func)
        {
            _timer = new Timer(o => func().Wait(), null, Period, Period);
            _isDispose = false;
        }

        public virtual void SetCallback(TimerCallback callback)
        {
            _timer = new Timer(callback);
            _isDispose = false;
        }

        public virtual void SetCallback(TimerCallback callback, object state, int dueTime, int period)
        {
            _timer = new Timer(callback, state, dueTime, period);
            _isDispose = false;
        }

        public virtual void SetCallback(TimerCallback callback, object state, long dueTime, long period)
        {
            _timer = new Timer(callback, state, dueTime, period);
            _isDispose = false;
        }

        public virtual void SetCallback(TimerCallback callback, object state, TimeSpan dueTime, TimeSpan period)
        {
            _timer = new Timer(callback, state, dueTime, period);
            _isDispose = false;
        }

        public virtual void SetCallback(TimerCallback callback, object state, uint dueTime, uint period)
        {
            _timer = new Timer(callback, state, dueTime, period);
            _isDispose = false;
        }

        public virtual bool Change(int dueTime, int period)
        {
            lock (_lockObj)
            {
                if (_isDispose)
                {
                    return false;
                }
                return _timer.Change(dueTime, period);
            }
        }

        public virtual bool Change(long dueTime, long period)
        {
            lock (_lockObj)
            {
                if (_isDispose)
                {
                    return false;
                }
                return _timer.Change(dueTime, period);
            }
        }

        public virtual bool Change(TimeSpan dueTime, TimeSpan period)
        {
            lock (_lockObj)
            {
                if (_isDispose)
                {
                    return false;
                }
                return _timer.Change(dueTime, period);
            }
        }

        public virtual bool Change(uint dueTime, uint period)
        {
            lock (_lockObj)
            {
                if (_isDispose)
                {
                    return false;
                }
                return _timer.Change(dueTime, period);
            }
        }

        public virtual bool Dispose(WaitHandle notifyObject)
        {
            lock (_lockObj)
            {
                return _timer.Dispose(notifyObject);
            }
        }
    }
}