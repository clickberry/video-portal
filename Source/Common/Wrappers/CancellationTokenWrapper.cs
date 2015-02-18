// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Threading;

namespace Wrappers
{
    public class CancellationTokenWrapper
    {
        private CancellationToken _token;

        public CancellationTokenWrapper()
        {
            _token = new CancellationToken();
        }

        public CancellationTokenWrapper(bool canceled)
        {
            _token = new CancellationToken(canceled);
        }

        public virtual bool CanBeCanceled
        {
            get { return _token.CanBeCanceled; }
        }

        public virtual bool IsCancellationRequested
        {
            get { return _token.IsCancellationRequested; }
        }

        /// <summary>
        ///     For use internal wrappers only.
        /// </summary>
        public CancellationToken CancellationToken
        {
            get { return _token; }
            set { _token = value; }
        }

        public virtual CancellationTokenRegistration Register(Action callback)
        {
            return _token.Register(callback);
        }

        public virtual void ThrowIfCancellationRequested()
        {
            _token.ThrowIfCancellationRequested();
        }
    }
}