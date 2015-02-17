// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using LinkTracker.DAL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoRepository;

namespace LinkTracker.DAL.Mongo.IdGenerators
{
    /// <summary>
    ///     Generates sequence number for new TrackingUrl entity.
    /// </summary>
    public class TrackingUrlIdGenerator : ITrackingUrlIdGenerator
    {
        private readonly MongoRepository<SequenceEntity> _sequenceRepository;

        private const string TrackingUrlSequenceName = "TrackingUrl";

        /// <summary>
        ///     The size of the reserved by app domain integers for id sequence.
        /// </summary>
        private const int IdBlockLength = 1000;

        private long _idBlockBegin;
        private long _idBlockOffset;

        private bool _isIdBlockReserved;

        /// <summary>
        ///     User-mode thread sync construct for performance
        /// </summary>
        private SpinLock _lock = new SpinLock(false);


        public TrackingUrlIdGenerator(MongoUrl mongoUrl)
        {
            _sequenceRepository = new MongoRepository<SequenceEntity>(mongoUrl);
        }


        public long GenerateId()
        {
            bool lockTaken = false;
            try
            {
                // Entering single-thread block
                _lock.Enter(ref lockTaken);

                // Incrementing id if it belongs to reserved block
                if (_isIdBlockReserved && ++_idBlockOffset < IdBlockLength)
                {
                    return _idBlockBegin + _idBlockOffset;
                }

                // Reserving new id block as atomic operation
                MongoCollection<SequenceEntity> sequenceCollection = _sequenceRepository.Collection;
                IMongoQuery query = Query.EQ("Name", TrackingUrlSequenceName);
                IMongoSortBy sortBy = SortBy.Null;
                UpdateBuilder update = Update.Inc("Current", IdBlockLength);
                var argument = new FindAndModifyArgs { Query = query, SortBy = sortBy, Update = update, VersionReturned = FindAndModifyDocumentVersion.Modified };
                FindAndModifyResult result = sequenceCollection.FindAndModify(argument);
                BsonDocument sequence = result.ModifiedDocument;

                if (sequence == null)
                {
                    // first block
                    _idBlockBegin = 0;
                    _idBlockOffset = 0;

                    sequenceCollection.Insert(new SequenceEntity { Name = TrackingUrlSequenceName, Current = 0 });
                }
                else
                {
                    _idBlockBegin = sequence.GetValue("Current").AsInt64;
                    _idBlockOffset = 0;
                }

                _isIdBlockReserved = true;

                long newId = _idBlockBegin + _idBlockOffset;
                return newId;
            }
            finally
            {
                if (lockTaken)
                {
                    // Exiting single-thread block
                    _lock.Exit();
                }
            }
        }
    }
}