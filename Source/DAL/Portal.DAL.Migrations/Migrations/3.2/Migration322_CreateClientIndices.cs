// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration322_CreateClientIndices : Migration, IPortalMigration
    {
        public Migration322_CreateClientIndices()
            : base("3.2.2")
        {
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> companyCollection = Database.GetCollection("Company");
            companyCollection.CreateIndex(new IndexKeysBuilder().Ascending("Users"), new IndexOptionsBuilder().SetSparse(true));
            companyCollection.CreateIndex(new IndexKeysBuilder().Ascending("Subscriptions._id"), new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
        }
    }
}