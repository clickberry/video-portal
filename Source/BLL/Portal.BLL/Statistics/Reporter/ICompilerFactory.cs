// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BLL.Statistics.Helper;

namespace Portal.BLL.Statistics.Reporter
{
    public interface ICompilerFactory
    {
        ICompiler Create(Interval interval);
    }
}