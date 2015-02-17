// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    /// <summary>
    ///     Abstract Entity for all the BusinessEntities.
    ///     Uses ObjectId for unique Id generation.
    /// </summary>
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class StatEntity : IEntity
    {
        [BsonRepresentation(BsonType.String)]
        public string Tick { get; set; }

        /// <summary>
        ///     Gets or sets the id for this object (the primary record for an entity).
        /// </summary>
        /// <value>
        ///     The id for this object (the primary record for an entity).
        /// </value>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}