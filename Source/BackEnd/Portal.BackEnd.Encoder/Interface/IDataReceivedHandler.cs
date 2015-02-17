// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.BackEnd.Encoder.Interface
{
    public interface IDataReceivedHandler
    {
        void ProcessData(string data);
        void Register(Action action);
        void Register(Action<int> action);
    }
}