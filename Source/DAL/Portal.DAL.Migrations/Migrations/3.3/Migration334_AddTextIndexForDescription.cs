// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration334_AddTextIndexForDescription : Migration, IPortalMigration
    {
        public Migration334_AddTextIndexForDescription()
            : base("3.3.4")
        {
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> projectCollection = Database.GetCollection("Project");
            projectCollection.DropIndex(new IndexKeysBuilder().Text("NameSort"));
            projectCollection.CreateIndex(new IndexKeysBuilder().Text("NameSort", "Description"));
        }
    }
}