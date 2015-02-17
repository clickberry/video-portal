// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoRepository;

namespace LinkTracker.DAL.Entities
{
    [CollectionName("TrackingUrl")]
    public class TrackingUrlEntity : IEntity
    {
        [BsonId(IdGenerator = typeof (StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        public string ProjectId { get; set; }

        public string RedirectUrl { get; set; }

        public string SubscriptionId { get; set; }
    }
}