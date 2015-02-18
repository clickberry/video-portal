// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using Portal.Domain.BillingContext;

namespace Portal.DAL.Entities.Table
{
    [CollectionName("BillingEvent")]
    [BsonIgnoreExtraElements]
    public sealed class BillingEventEntity : IEntity
    {
        public string Id { get; set; }

        public DateTime Created { get; set; }

        public bool LiveMode { get; set; }

        public EventType Type { get; set; }

        public string ObjectId { get; set; }
    }
}