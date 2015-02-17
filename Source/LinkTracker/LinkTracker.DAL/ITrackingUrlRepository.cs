// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using LinkTracker.DAL.Entities;

namespace LinkTracker.DAL
{
    public interface ITrackingUrlRepository
    {
        IQueryable<TrackingUrlEntity> AsQueryable();

        Task<TrackingUrlEntity> GetAsync(TrackingUrlEntity entity);

        Task<TrackingUrlEntity> AddAsync(TrackingUrlEntity entity);

        Task<TrackingUrlEntity> EditAsync(TrackingUrlEntity entity);

        Task DeleteAsync(TrackingUrlEntity entity);
    }
}