// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    [CollectionName("SendEmail")]
    [BsonNoId]
    [BsonIgnoreExtraElements]
    public sealed class SendEmailEntity : IEntity
    {
        public string UserId { get; set; }

        public DateTime Created { get; set; }

        public string Emails { get; set; }

        public string Subject { get; set; }

        public string Id { get; set; }
    }
}