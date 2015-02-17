// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Portal.BLL.Statistics.Generator
{
    public interface IReportGenerator
    {
        Task Generate(DateTime dateTime);
        Task GenerateIfNotExist(DateTime dateTime);
    }
}