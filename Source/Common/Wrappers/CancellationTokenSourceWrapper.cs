// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;

namespace Wrappers
{
    public class CancellationTokenSourceWrapper
    {
        private readonly CancellationTokenWrapper _tokenWrapper;

        private CancellationTokenSource _tokenSource;

        public CancellationTokenSourceWrapper()
        {
            _tokenSource = new CancellationTokenSource();
            _tokenWrapper = new CancellationTokenWrapper
            {
                CancellationToken = _tokenSource.Token
            };
        }

        public virtual bool IsCancellationRequested
        {
            get { return _tokenSource.IsCancellationRequested; }
        }

        public virtual CancellationTokenWrapper Token
        {
            get { return _tokenWrapper; }
        }

        /// <summary>
        ///     For use internal wrappers only.
        /// </summary>
        public CancellationTokenSource CancellationTokenSource
        {
            get { return _tokenSource; }
            set { _tokenSource = value; }
        }

        public virtual void Cancel()
        {
            _tokenSource.Cancel();
        }

        public virtual void Cancel(bool throwOnFirstException)
        {
            _tokenSource.Cancel(throwOnFirstException);
        }

        public virtual CancellationTokenSource CreateLinkedTokenSource(params CancellationToken[] tokens)
        {
            throw new NotImplementedException();
        }

        public virtual CancellationTokenSource CreateLinkedTokenSource(CancellationToken token1, CancellationToken token2)
        {
            throw new NotImplementedException();
        }

        public virtual void Dispose()
        {
            _tokenSource.Dispose();
        }
    }
}