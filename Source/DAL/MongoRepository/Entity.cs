// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoRepository
{
    /// <summary>
    ///     Abstract Entity for all the BusinessEntities.
    ///     Uses ObjectId for unique Id generation.
    /// </summary>
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class Entity : IEntity
    {
        /// <summary>
        ///     Gets or sets the id for this object (the primary record for an entity).
        /// </summary>
        /// <value>
        ///     The id for this object (the primary record for an entity).
        /// </value>
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }
    }
}