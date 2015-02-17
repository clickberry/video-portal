// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Wrappers.Interface;

namespace Wrappers.Implementation
{
    public class GuidWrapper : IGuidWrapper
    {
        public string Generate()
        {
            return Guid.NewGuid().ToString();
        }
    }
}