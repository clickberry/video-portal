// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration327_AddUserAndProjectTextIndicex : Migration, IPortalMigration
    {
        public Migration327_AddUserAndProjectTextIndicex()
            : base("3.2.7")
        {
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> projectCollection = Database.GetCollection("Project");
            projectCollection.DropIndex(new IndexKeysBuilder().Ascending("NameSort"));
            projectCollection.CreateIndex(new IndexKeysBuilder().Text("NameSort"));

            MongoCollection<BsonDocument> userCollection = Database.GetCollection("User");
            userCollection.DropIndex(new IndexKeysBuilder().Ascending("UserNameSort"));
            userCollection.Update(Query.Null, MongoDB.Driver.Builders.Update.Rename("UserName", "Name"), UpdateFlags.Multi);
            userCollection.Update(Query.Null, MongoDB.Driver.Builders.Update.Rename("UserNameSort", "NameSort"), UpdateFlags.Multi);
            userCollection.CreateIndex(new IndexKeysBuilder().Text("NameSort"));
        }
    }
}