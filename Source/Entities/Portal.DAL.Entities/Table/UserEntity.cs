// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    [CollectionName("User")]
    [BsonIgnoreExtraElements]
    public sealed class UserEntity : IEntity
    {
        public UserEntity()
        {
            Memberships = new List<UserMembershipEntity>();
            Roles = new List<string>();
            Followers = new List<FollowerEntity>();
        }

        public string AppName { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public string Name { get; set; }

        public string NameSort { get; set; }

        [BsonIgnoreIfNull]
        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public long MaximumStorageSpace { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Timezone { get; set; }

        public int ProductId { get; set; }

        public List<UserMembershipEntity> Memberships { get; set; }

        public List<string> Roles { get; set; }

        public int State { get; set; }

        [BsonId(IdGenerator = typeof (StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonDefaultValue(true)]
        public bool NotifyOnVideoComments { get; set; }

        public List<FollowerEntity> Followers { get; set; }
    }
}