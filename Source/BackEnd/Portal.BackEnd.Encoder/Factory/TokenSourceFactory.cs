// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BackEnd.Encoder.Interface;
using Wrappers;

namespace Portal.BackEnd.Encoder.Factory
{
    public class TokenSourceFactory : ITokenSourceFactory
    {
        public CancellationTokenSourceWrapper CreateTokenSource()
        {
            return new CancellationTokenSourceWrapper();
        }
    }
}