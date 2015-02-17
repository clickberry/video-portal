// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using LinkTracker.Domain;

namespace LinkTracker.BLL
{
    public interface IUrlTrackingService
    {
        Task<DomainTrackingUrl> AddAsync(DomainTrackingUrl trackingUrl);

        Task<UrlTrackingResult> TrackAsync(DomainTrackingUrl trackingUrl);
    }
}