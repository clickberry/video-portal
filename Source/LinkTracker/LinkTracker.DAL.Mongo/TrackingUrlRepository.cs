// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LinkTracker.DAL.Entities;
using LinkTracker.DAL.Mongo.IdGenerators;
using MongoDB.Driver;
using MongoRepository;

namespace LinkTracker.DAL.Mongo
{
    public class TrackingUrlRepository : ITrackingUrlRepository
    {
        private readonly MongoRepository<TrackingUrlEntity> _collection;
        private readonly ITrackingUrlIdGenerator _idGenerator;

        public TrackingUrlRepository(MongoUrl mongoUrl, ITrackingUrlIdGenerator idGenerator)
        {
            _collection = new MongoRepository<TrackingUrlEntity>(mongoUrl);
            _idGenerator = idGenerator;
        }


        public IQueryable<TrackingUrlEntity> AsQueryable()
        {
            return _collection.Context;
        }

        public Task<TrackingUrlEntity> GetAsync(TrackingUrlEntity entity)
        {
            return _collection.GetAsync(entity.Id);
        }

        public async Task<TrackingUrlEntity> AddAsync(TrackingUrlEntity entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                // Generating new integer id
                entity.Id = _idGenerator.GenerateId().ToString(CultureInfo.InvariantCulture);
            }

            TrackingUrlEntity result;
            try
            {
                result = await _collection.AddAsync(entity);
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Mongo driver failure: {0}", e));
            }

            return result;
        }

        public async Task<TrackingUrlEntity> EditAsync(TrackingUrlEntity entity)
        {
            TrackingUrlEntity result;
            try
            {
                result = await _collection.UpdateAsync(entity);
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Mongo driver failure: {0}", e));
            }

            return result;
        }

        public async Task DeleteAsync(TrackingUrlEntity entity)
        {
            try
            {
                await _collection.DeleteAsync(entity.Id);
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Mongo driver failure: {0}", e));
            }
        }
    }
}