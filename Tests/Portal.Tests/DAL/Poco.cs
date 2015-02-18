// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;

namespace Portal.Tests.DAL
{
    [CollectionName("Pocos")]
    public sealed class Poco : IEntity
    {
        public string StringValue { get; set; }

        public long LongValue { get; set; }

        [BsonId]
        public string Id { get; set; }
    }
}