// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    [CollectionName("File")]
    [BsonIgnoreExtraElements]
    public sealed class FileEntity : IEntity
    {
        /// <summary>
        ///     Gets or sets a user identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Gets or sets a file name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a file length.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        ///     Gets or sets a file creation date.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     Gets or sets a file modification date.
        /// </summary>
        public DateTime Modified { get; set; }

        /// <summary>
        ///     Gets or sets a file content type.
        /// </summary>
        public string ContentType { get; set; }


        /// <summary>
        ///     Gets a value indication whether file is artifact.
        /// </summary>
        public bool IsArtifact { get; set; }

        /// <summary>
        ///     Get or sets file identifier.
        /// </summary>
        [BsonId(IdGenerator = typeof (StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }
    }
}