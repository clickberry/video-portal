// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations
{
    public class Migration329_UnsetBlockedField : Migration, IPortalMigration
    {
        public Migration329_UnsetBlockedField()
            : base("3.2.9")
        {
        }

        public override void Update()
        {
            // Remove Blocked and IsBlocked fields

            MongoCollection<BsonDocument> userCollection = Database.GetCollection("User");
            userCollection.Update(Query.Null, MongoDB.Driver.Builders.Update.Unset("Blocked"), UpdateFlags.Multi);
            userCollection.Update(Query.Null, MongoDB.Driver.Builders.Update.Unset("IsBlocked"), UpdateFlags.Multi);

            MongoCollection<BsonDocument> projectCollection = Database.GetCollection("Project");
            projectCollection.Update(Query.Null, MongoDB.Driver.Builders.Update.Unset("IsBlocked"), UpdateFlags.Multi);
        }
    }
}