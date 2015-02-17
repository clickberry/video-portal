// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Portal.DAL.Infrastructure.Analytics
{
    public interface IAnalytics
    {
        Task CollectVisitAsync(AnalyticsVisit data);
    }
}