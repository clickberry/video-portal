// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Bson.Serialization.Attributes;

namespace MongoRepository
{
    /// <summary>
    ///     Use it for your entities if you need specific Id serialization strategy like custom strings.
    ///     For most cases use Entity abstract class for ObjectId Id's.
    /// </summary>
    public interface IEntity
    {
        [BsonId]
        string Id { get; set; }
    }
}